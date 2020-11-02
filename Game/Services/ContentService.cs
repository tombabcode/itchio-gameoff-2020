using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Linq;
using TBEngine.Services;
using TBEngine.Types;

namespace GameJam.Services {
    /// <summary>
    /// Content service for managing assets
    /// </summary>
    public sealed class ContentService : ContentServiceBase {

        private SpriteFont _fontStandard;
        private SpriteFont _fontStandardItalic;
        private SpriteFont _fontTiny;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content"><see cref="ContentManager"/></param>
        /// <param name="device"><see cref="GraphicsDevice"/></param>
        /// <param name="canvas"><see cref="SpriteBatch"/></param>
        public ContentService(ContentManager content, GraphicsDevice device, SpriteBatch canvas) : base(content, device, canvas) { }

        /// <summary>
        /// Loads content
        /// </summary>
        public override void LoadContent( ) {
            _fontTiny = Content.Load<SpriteFont>(Path.Combine("Fonts", "Tiny"));
            _fontStandard = Content.Load<SpriteFont>(Path.Combine("Fonts", "Standard"));
            _fontStandardItalic = Content.Load<SpriteFont>(Path.Combine("Fonts", "StandardItalic"));
        }

        /// <summary>
        /// Get correct font based on type and properties
        /// </summary>
        /// <param name="type">Type of the font</param>
        /// <param name="props">Flags (ex. bold, italic, etc)</param>
        /// <returns><see cref="SpriteFont"/></returns>
        public override SpriteFont GetFont(FontType type = FontType.Standard, FontPropertyType[] props = null) {
            bool isItalic = props != null && props.Contains(FontPropertyType.Italic);

            if (type == FontType.Standard) {
                if (isItalic) return _fontStandardItalic;
                return _fontStandard;
            }

            if (type == FontType.Tiny)
                return _fontTiny;

            return _fontStandard;
        }

    }
}
