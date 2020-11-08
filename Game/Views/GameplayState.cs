using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using GameJam.Gameplay;
using GameJam.Services;
using GameJam.Types;
using GameJam.Utils;
using TBEngine.Components.State;
using TBEngine.Services;
using TBEngine.Types;
using TBEngine.Utils;

using DH = TBEngine.Utils.DisplayHelper;
using UH = TBEngine.Utils.UtilsHelper;

namespace GameJam.Views {
    /// <summary>
    /// Gameplay state
    /// </summary>
    public sealed class GameplayState : State {

        // References
        private readonly InputService _input;
        private readonly ContentService _content;
        private readonly ConfigurationService _config;
        private readonly StateService _state;
        private readonly GameConsole _console;
        private readonly SpriteFont _debugFont;

        // Gameplay's data
        private Player _player;
        private CameraService _camera;
        private Level _level;

        // Scenes
        private RenderTarget2D _gameplayScene;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content"><see cref="ContentService"/></param>
        /// <param name="input"><see cref="InputService"/></param>
        /// <param name="config"><see cref="ConfigurationService"/></param>
        /// <param name="state"><see cref="StateService"/></param>
        /// <param name="console"><see cref="GameConsole"/></param>
        public GameplayState(ContentService content, InputService input, ConfigurationService config, StateService state, GameConsole console) {
            _input = input;
            _state = state;
            _config = config;
            _content = content;
            _console = console;
            _debugFont = _content.GetFont(FontType.Tiny);

            Initialize(content);
        }

        /// <summary>
        /// State's initialize
        /// </summary>
        /// <param name="content"><see cref="ContentServiceBase"/></param>
        public override void Initialize(ContentServiceBase content) {
            Width = _config.ViewWidth;
            Height = _config.ViewHeight;
            _gameplayScene = new RenderTarget2D(content.Device, (int)Width, (int)Height);

            base.Initialize(content);
        }

        /// <summary>
        /// Creates a new game
        /// </summary>
        /// <param name="playerName">Player's name</param>
        public void NewGame(string playerName = null) {
            _camera = new CameraService(0.25f, new Vector2(_config.WindowWidth / 2, _config.WindowHeight / 2)) {
                MinScale = 0.20f,
                MaxScale = 0.75f,
                ScaleFactor = 0.05f
            };
            _level = new Level( );
            _level.GenerateMap(20, 20);
            List<WorldTile> tiles = _level.Map.Where(tile => !tile.IsWall).ToList( );
            WorldTile tile = tiles.ElementAt(RandomService.GetRandomInt(tiles.Count));
            _player = new Player(_content, _config, playerName ?? "Unknown", tile.DisplayX, tile.DisplayY);

            LogService.Add($"Player spawned at [{tile.X}, {tile.Y}] ({_player.X:0}, {_player.Y:0})");
        }

        /// <summary>
        /// State's update
        /// </summary>
        /// <param name="time"><see cref="GameTime"/></param>
        public override void Update(GameTime time) {
            if (!_console.IsVisible) {
                if (_input.HasScrolledDown( )) _camera.ZoomOut( );
                if (_input.HasScrolledUp( )) _camera.ZoomIn( );
                if (_input.IsKeyPressedOnce(_config.KEY_Pause)) {
                    _state.ChangeState(GameStateType.Pause);
                    ((PauseState)_state.GetCurrentState( )).OnResume = ( ) => _state.ChangeState(GameStateType.Gameplay);
                }
                if (_input.IsKeyPressedOnce(Keys.R)) NewGame( );
                if (_input.IsKeyPressedOnce(Keys.Space)) 
                    _level.Test( );

                _player.Update(_input, _config, time, _level);
                _camera.LookAt(-_player.X * _camera.Scale, -_player.Y * _camera.Scale);
            }

            _camera.Update( );
        }

        /// <summary>
        /// Render state
        /// </summary>
        /// <param name="time"><see cref="GameTime"/></param>
        public override void Render(GameTime time) {
            DH.RenderScene(_gameplayScene, _camera, ( ) => {
                foreach (WorldTile tile in _level.Map)
                    if (!tile.IsWall) {
                        float distance = (float)Math.Sqrt(Math.Pow(tile.DisplayX - _player.X, 2) + Math.Pow(tile.DisplayY - _player.Y, 2));
                        float percentage = 1 - (distance < 350 ? 0 : (distance - 350) / 700);
                        DH.Raw(_content.TEXGround.Texture, tile.DisplayX - 16, tile.DisplayY - 16, color: ColorsManager.Get(percentage));
                    }

                if (_config.DebugMode)
                    foreach (WorldTile tile in _level.Map) {
                        if (tile.Collisions.Contains(CollisionType.Left))
                            DH.Line(tile.DisplayX, tile.DisplayY, tile.DisplayX, tile.DisplayY + tile.Size, 8, Color.Red);
                        if (tile.Collisions.Contains(CollisionType.Right))
                            DH.Line(tile.DisplayX + tile.Size, tile.DisplayY, tile.DisplayX + tile.Size, tile.DisplayY + tile.Size, 8, Color.Red);
                        if (tile.Collisions.Contains(CollisionType.Top))
                            DH.Line(tile.DisplayX, tile.DisplayY, tile.DisplayX + tile.Size, tile.DisplayY, 8, Color.Red);
                        if (tile.Collisions.Contains(CollisionType.Bottom))
                            DH.Line(tile.DisplayX, tile.DisplayY + tile.Size, tile.DisplayX + tile.Size, tile.DisplayY + tile.Size, 8, Color.Red);
                    }

                _player.Display(time);
            });

            DH.RenderScene(Scene, ( ) => {
                DH.Scene(_gameplayScene);
                DH.Text(_content.GetFont( ), _player.Name, 15, 15, false);

                // Mini-map
                UH.Loops(_level.Width, _level.Height, (x, y) => {
                if (!_level.Map[y * _level.Width + x].IsWall)
                    DH.Raw(_content.Pixel, 
                        _config.ViewWidth - 16 - _level.Width * 4 + x * 4, 
                        _config.ViewHeight - 16 - _level.Height * 4 + y  * 4, 
                        4, 4, 
                        (_player.OnMapX == x && _player.OnMapY == y ? Color.Red : Color.Gray) * .5f
                    );
                });

                if (_config.DebugMode) {
                    DH.Text(_debugFont, $"{(int)(1 / time.ElapsedGameTime.TotalSeconds)} FPS", _config.WindowWidth - 10, 10, false, ColorsManager.DarkGray, AlignType.RT);
                    DH.Text(_debugFont, $"Mouse ({_input.MouseX}, {_input.MouseY})", _config.WindowWidth - 10, 25, false, ColorsManager.DarkGray, AlignType.RT);
                    DH.Text(_debugFont, $"Player ({_player.X:0.0}, {_player.Y:0.0}) ({_player.OnMapX}, {_player.OnMapY})", _config.WindowWidth - 10, 40, false, ColorsManager.DarkGray, AlignType.RT);
                    DH.Text(_debugFont, $"Camera ({_camera.Target.X:0.0}, {_camera.Target.Y:0.0})", _config.WindowWidth - 10, 55, false, ColorsManager.DarkGray, AlignType.RT);
                    DH.Text(_debugFont, $"Scale {_camera.Scale:0.00}x", _config.WindowWidth - 10, 70, false, ColorsManager.DarkGray, AlignType.RT);

                    DH.Line(0, _config.WindowHeight / 2, _config.WindowWidth, _config.WindowHeight / 2, 1, ColorsManager.DarkestGray * .5f);
                    DH.Line(_config.WindowWidth / 2, 0, _config.WindowWidth / 2, _config.WindowHeight, 1, ColorsManager.DarkestGray * .5f);
                }
            });
        }

    }
}
