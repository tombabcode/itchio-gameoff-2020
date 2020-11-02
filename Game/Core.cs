using GameJam.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam {
    public sealed class Core : Game {

        private InputService _input;
        private ContentService _content;
        private ConfigurationService _config;

        private SpriteBatch _canvas;
        private GraphicsDeviceManager _deviceMNG;

        public Core( ) {
            _deviceMNG = new GraphicsDeviceManager(this);

            IsMouseVisible = true;
            Content.RootDirectory = "Assets";
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
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
        }

    }
}
