using GameJam.Gameplay;
using GameJam.Services;
using GameJam.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBEngine.Components.State;
using TBEngine.Services;
using TBEngine.Types;
using DH = TBEngine.Utils.DisplayHelper;

namespace GameJam.Views {
    public sealed class GameplayState : State {

        // References
        private InputService _input;
        private ContentService _content;
        private ConfigurationService _config;
        private StateService _state;

        private Player _player;
        private CameraService _camera;
        private Level _level;

        private RenderTarget2D GameplayScene;

        public GameplayState(ContentService content, InputService input, ConfigurationService config, StateService state) {
            _input = input;
            _state = state;
            _config = config;
            _content = content;

            Initialize(content);
        }

        public void NewGame(string playerName = null) {
            _player = new Player(playerName ?? "Unknown");
            _camera = new CameraService(0.25f, new Vector2(_config.WindowWidth / 2, _config.WindowHeight / 2)) {
                MinScale = 0.20f,
                MaxScale = 0.75f,
                ScaleFactor = 0.05f
            };
            _level = new Level( );
            _level.GenerateMap(40, 21);
        }

        public override void Initialize(ContentServiceBase content) {
            Width = _config.ViewWidth;
            Height = _config.ViewHeight;
            GameplayScene = new RenderTarget2D(content.Device, (int)Width, (int)Height);
            
            base.Initialize(content);
        }

        public override void Update(GameTime time) {
            if (_input.HasScrolledDown( )) _camera.ZoomOut( );
            if (_input.HasScrolledUp( )) _camera.ZoomIn( );
            if (_input.IsKeyPressedOnce(_config.KEY_Pause)) {
                _state.ChangeState(GameStateType.Pause);
                ((PauseState)_state.GetCurrentState( )).OnResume = ( ) => _state.ChangeState(GameStateType.Gameplay);
            }

            _player.Update(_input, _config, time);
            _camera.Update( );
        }

        public override void Render(GameTime time) {
            DH.RenderScene(GameplayScene, _camera, ( ) => {
                foreach (WorldTile tile in _level.Map)
                    if (tile.ID == 1)
                        DH.Raw(_content.Pixel, tile.X * 8, tile.Y * 8, Color.Green);

                //DH.Raw(_content.Pixel, _player.CollisionBounds.X + _player.PositionX, _player.CollisionBounds.Y + _player.PositionY, _player.CollisionBounds.Width, _player.CollisionBounds.Height, align: AlignType.CM);
                //DH.Raw(_content.TEXTest.Texture, _player.GetDisplayData(time, _content), align: AlignType.CB);
            });

            DH.RenderScene(Scene, ( ) => {
                DH.Scene(GameplayScene);
                DH.Text(_content.GetFont( ), _player.Name, 15, 15, false);
            });
        }

    }
}
