using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameJam.Services;
using GameJam.Utils;
using TBEngine.Services;
using TBEngine.Types;

using ALIGN = TBEngine.Types.AlignType;
using COLOR = GameJam.Utils.ColorsManager;
using DH = TBEngine.Utils.DisplayHelper;

namespace GameJam {
    /// <summary>
    /// Application core logic
    /// </summary>
    public sealed class Core : Game {

        // Const
        public const string VERSION = "0.1";

        // Local
        private readonly GraphicsDeviceManager _deviceMNG;
        private SpriteBatch _canvas;

        // Services
        private InputService _input;
        private ContentService _content;
        private ConfigurationService _config;

        // Components and views
        private GameConsole _console;

        // Helpers
        private SpriteFont _font => _content.GetFont(FontType.Tiny);
        private int _width => _config.WindowWidth;
        private int _height => _config.WindowHeight;

        /// <summary>
        /// Constructor
        /// </summary>
        public Core( ) {
            _deviceMNG = new GraphicsDeviceManager(this);

            IsMouseVisible = true;
            Content.RootDirectory = "Assets";
            Window.Title = $"Game Off 2020 | itch.io | ver. {VERSION}";
        }

        /// <summary>
        /// Initialize
        /// </summary>
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
            _console = new GameConsole(_input, _content, _config, _config.WindowWidth, _height);
            _console.SetAction("hide", 0, (args) => _console.IsVisible = false);
            _console.SetAction("exit", 0, (args) => Exit( ));

            DH.Content = _content;

            _content.LoadContent( );

            LogService.Add($"App ready, version {VERSION}", LogType.Info);
        }

        /// <summary>
        /// App's update
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        protected override void Update(GameTime gameTime) {
            _input.Update(gameTime);
            _console.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Display graphics
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        protected override void Draw(GameTime gameTime) {
            // View renders
            _console.Render( );

            // Display
            DH.RenderScene(null, ( ) => {
                DH.Text(_font, $"FPS {(int)(1 / gameTime.ElapsedGameTime.TotalSeconds)}", 10, 10, false, COLOR.DarkGray);
                DH.Text(_font, $"ver. {VERSION}", 10, _height - 10, false, COLOR.DarkGray, ALIGN.LM);
                DH.Text(_font, $"Copyright (C) Tomasz Babiak, ASwan, Oliver F. for Game Off 2020, itch.io", _width - 10, _height - 10, false, COLOR.DarkGray, ALIGN.RM);

                _console.Display( );
            });

            base.Draw(gameTime);
        }

    }
}
