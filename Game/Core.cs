using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameJam.Services;
using GameJam.Utils;
using TBEngine.Services;
using TBEngine.Types;

using ALIGN = TBEngine.Types.AlignType;
using COLOR = GameJam.Utils.ColorsManager;
using DH = TBEngine.Utils.DisplayHelper;
using GameJam.Types;
using GameJam.Views;
using TBEngine.Utils;
using GameJam.Models;

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
        private StateService _state;
        private InputService _input;
        private ContentService _content;
        private ConfigurationService _config;

        // Components and views
        private GameConsole _console;

        // Helpers
        private SpriteFont Font => _content.GetFont(FontType.Tiny);
        private int Width => _config.WindowWidth;
        private int Height => _config.WindowHeight;

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
            _console = new GameConsole(_input, _content, _config, _config.WindowWidth, Height);
            _console.SetAction("hide", 0, (args) => _console.IsVisible = false);
            _console.SetAction("exit", 0, (args) => Exit( ));
            _console.SetAction("translate", 1, (args) => LogService.Add(TranslationService.Get(args[0]), LogType.Success));
            _console.SetAction("new_game", 0, (args) => {
                _state.ChangeState(GameStateType.Gameplay);
                ((GameplayState)_state.GetCurrentState( )).NewGame(args.Length >= 1 ? args[0] : null);
            });

            DH.Content = _content;

            _content.LoadContent( );

            _state = new StateService( );
            _state.Register(GameStateType.MainMenu, new MainMenuState(_content, _input, _config, _console));
            _state.Register(GameStateType.Gameplay, new GameplayState(_content, _input, _config));
            _state.ChangeState(GameStateType.MainMenu);

            TranslationService.LoadTranslations<LanguageModel>("en");
            AudioHelper.SoundVolume = .25f;

            LogService.Add($"App ready, version {VERSION}", LogType.Info);
        }

        /// <summary>
        /// App's update
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        protected override void Update(GameTime gameTime) {
            _input.Update(gameTime);
            _console.Update(gameTime);
            _state.GetCurrentState( ).Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Display graphics
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        protected override void Draw(GameTime gameTime) {
            // View renders
            _console.Render(gameTime);
            _state.GetCurrentState( ).Render(gameTime);

            // Display
            DH.RenderScene(null, ( ) => {
                _state.GetCurrentState( ).Display( );
                
                DH.Text(Font, $"FPS {(int)(1 / gameTime.ElapsedGameTime.TotalSeconds)}", 10, 10, false, COLOR.DarkGray);
                DH.Text(Font, $"ver. {VERSION}", 10, Height - 10, false, COLOR.DarkGray, ALIGN.LM);
                DH.Text(Font, $"Copyright (C) Tomasz Babiak, ASwan, Oliver F. for Game Off 2020, itch.io", Width - 10, Height - 10, false, COLOR.DarkGray, ALIGN.RM);

                _console.Display( );
            });

            base.Draw(gameTime);
        }

    }
}
