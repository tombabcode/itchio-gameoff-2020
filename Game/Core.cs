using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam {
    public sealed class Core : Game {

        private SpriteBatch _canvas;

        public Core( ) {
            _ = new GraphicsDeviceManager(this);
        }

        protected override void Initialize( ) {
            base.Initialize( );

            _canvas = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
        }

    }
}
