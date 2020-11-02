using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TBEngine.Services;

namespace GameJam.Services {
    /// <summary>
    /// Content service for managing assets
    /// </summary>
    public sealed class ContentService : ContentServiceBase {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content"><see cref="ContentManager"/></param>
        /// <param name="device"><see cref="GraphicsDevice"/></param>
        /// <param name="canvas"><see cref="SpriteBatch"/></param>
        public ContentService(ContentManager content, GraphicsDevice device, SpriteBatch canvas) : base(content, device, canvas) {
        }

    }
}
