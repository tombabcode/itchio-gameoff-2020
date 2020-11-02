using GameJam.Services;
using GameJam.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.ComponentModel;
using TBEngine.Services;
using TBEngine.Types;
using TBEngine.Utils;

namespace GameJam {
    public sealed class Core : Game {

        public const string VERSION = "0.1";

        private InputService _input;
        private ContentService _content;
        private ConfigurationService _config;

        private GameConsole _console;

        private SpriteBatch _canvas;
        private GraphicsDeviceManager _deviceMNG;

        public Core( ) {
            _deviceMNG = new GraphicsDeviceManager(this);

            IsMouseVisible = true;
            Content.RootDirectory = "Assets";
            Window.Title = $"Game Off 2020 | itch.io | ver. {VERSION}";
        }

        protected override void Initialize( ) {
            base.Initialize( );

            _config = new ConfigurationService(_deviceMNG);
            _config.LoadConfiguration( );

            _deviceMNG.PreferredBackBufferWidth = _config.WindowWidth;
            _deviceMNG.PreferredBackBufferHeight = _config.WindowHeight;
            _deviceMNG.ApplyChanges( );

            _canvas = new SpriteBatch(GraphicsDevice);

            _input = new InputService( );
            _content = new ContentService(Content, GraphicsDevice, _canvas);
            _console = new GameConsole(_input, _content, _config, _config.WindowWidth, _config.WindowHeight);
            _console.SetAction("hide", 0, (args) => _console.IsVisible = false);
            _console.SetAction("exit", 0, (args) => Exit( ));

            DisplayHelper.Content = _content;

            _content.LoadContent( );

            LogService.Add($"App ready, version {VERSION}", LogType.Info);
        }

        protected override void Update(GameTime gameTime) {
            _input.Update(gameTime);
            _console.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            _console.Render( );

            DisplayHelper.RenderScene(null, ( ) => {
                DisplayHelper.Text(_content.GetFont(FontType.Tiny), $"ver. {VERSION}", new Vector2(10, _config.WindowHeight - 10), false, ColorsManager.DarkGray, TBEngine.Types.AlignType.LM);
                DisplayHelper.Text(_content.GetFont(FontType.Tiny), $"Copyright (C) Tomasz Babiak, ASwan, Oliver F. for Game Off 2020, itch.io", new Vector2(_config.WindowWidth - 10, _config.WindowHeight - 10), false, ColorsManager.DarkGray, TBEngine.Types.AlignType.RM);

                _console.Display( );
            });
            base.Draw(gameTime);
        }

    }
}
