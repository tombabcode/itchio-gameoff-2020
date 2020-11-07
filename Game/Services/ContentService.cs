using System.Linq;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TBEngine.Services;
using TBEngine.Textures;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using GameJam.Types;

namespace GameJam.Services {
    /// <summary>
    /// Content service for managing assets
    /// </summary>
    public sealed class ContentService : ContentServiceBase {

        // Local
        private SpriteFont _fontBig;
        private SpriteFont _fontStandard;
        private SpriteFont _fontStandardItalic;
        private SpriteFont _fontSmall;
        private SpriteFont _fontTiny;

        public TextureStatic TEXTest { get; private set; }

        public SoundEffect AUDIO_ButtonHover { get; private set; }

        public List<Tuple<GameMoodType, Song>> AUDIO_Songs { get; private set; }

        public Texture2D TEXUI_MenuBG { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content"><see cref="ContentManager"/></param>
        /// <param name="device"><see cref="GraphicsDevice"/></param>
        /// <param name="canvas"><see cref="SpriteBatch"/></param>
        public ContentService(ContentManager content, GraphicsDevice device, SpriteBatch canvas) : base(content, device, canvas) { }

        /// <summary>
        /// Loads app's content
        /// </summary>
        public override void LoadContent( ) {
            _fontBig = Content.Load<SpriteFont>(Path.Combine("Fonts", "Big"));
            _fontSmall = Content.Load<SpriteFont>(Path.Combine("Fonts", "Small"));
            _fontTiny = Content.Load<SpriteFont>(Path.Combine("Fonts", "Tiny"));
            _fontStandard = Content.Load<SpriteFont>(Path.Combine("Fonts", "Standard"));
            _fontStandardItalic = Content.Load<SpriteFont>(Path.Combine("Fonts", "StandardItalic"));

            TEXTest = new TextureStatic(Content.Load<Texture2D>(Path.Combine("Textures", "Characters", "test_texture")));

            AUDIO_ButtonHover = Content.Load<SoundEffect>(Path.Combine("Audio", "UI", "button_menu"));

            AUDIO_Songs = new List<Tuple<GameMoodType, Song>>( ) {
                new Tuple<GameMoodType, Song>(GameMoodType.Default, Content.Load<Song>(Path.Combine("Audio", "Music", "song_0")))
            };
        }

        public void UpdateDynamicContent(ConfigurationService config) {
            TEXUI_MenuBG = FillTexture(Content.Load<Texture2D>(Path.Combine("Textures", "UI", "menu_box_bg")), 256, config.ViewHeight);
        }

        public SpriteFont GetFont(FontType fontType = FontType.Standard) {
            return fontType switch {
                FontType.Tiny => _fontTiny,
                FontType.Big => _fontBig,
                FontType.Standard => _fontStandard,
                FontType.Small => _fontSmall,
                _ => _fontStandard,
            };
        }

        public Song GetRandomSong(GameMoodType mood = GameMoodType.Default) {
            Tuple<GameMoodType, Song> data = AUDIO_Songs.Find(song => song.Item1 == mood);
            if (data != null)
                return data.Item2;

            return null;
        }

    }
}
