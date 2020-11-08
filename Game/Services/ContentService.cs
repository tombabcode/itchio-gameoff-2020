using System.Collections.Generic;
using System.IO;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using GameJam.Types;
using TBEngine.Services;
using TBEngine.Textures;

namespace GameJam.Services {
    /// <summary>
    /// Content service for managing assets
    /// </summary>
    public sealed class ContentService : ContentServiceBase {

        // Font data
        private SpriteFont _fontBig;
        private SpriteFont _fontStandard;
        private SpriteFont _fontSmall;
        private SpriteFont _fontTiny;

        // Textures
        public TextureStatic TEXCharacter { get; private set; }
        public TextureStatic TEXGround { get; private set; }

        // Sound effects
        public SoundEffect AUDIO_ButtonHover { get; private set; }

        // Music
        public List<Tuple<GameMoodType, Song>> AUDIO_Songs { get; private set; }

        // GUI
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

            TEXCharacter = new TextureStatic(Content.Load<Texture2D>(Path.Combine("Textures", "Characters", "test_texture")));
            TEXGround = new TextureStatic(Content.Load<Texture2D>(Path.Combine("Textures", "Ground", "tile_0")));

            AUDIO_ButtonHover = Content.Load<SoundEffect>(Path.Combine("Audio", "UI", "button_menu"));

            AUDIO_Songs = new List<Tuple<GameMoodType, Song>>( ) {
                new Tuple<GameMoodType, Song>(GameMoodType.Default, Content.Load<Song>(Path.Combine("Audio", "Music", "song_0")))
            };
        }

        /// <summary>
        /// Loads dynamic content that can change during app's lifetime
        /// </summary>
        /// <param name="config"><see cref="ConfigurationService"/></param>
        public void UpdateDynamicContent(ConfigurationService config) {
            // Cleanup
            if (TEXUI_MenuBG != null) TEXUI_MenuBG.Dispose( );
            
            // Loading
            TEXUI_MenuBG = FillTexture(Content.Load<Texture2D>(Path.Combine("Textures", "UI", "menu_box_bg")), 256, config.ViewHeight);
        }

        /// <summary>
        /// Get font based on given properties
        /// </summary>
        /// <param name="fontType">Font size</param>
        /// <returns><see cref="SpriteFont"/></returns>
        public SpriteFont GetFont(FontType fontType = FontType.Standard) {
            return fontType switch {
                FontType.Tiny => _fontTiny,
                FontType.Big => _fontBig,
                FontType.Standard => _fontStandard,
                FontType.Small => _fontSmall,
                _ => _fontStandard,
            };
        }

        /// <summary>
        /// Get random song based on game's mood
        /// </summary>
        /// <param name="mood"><see cref="GameMoodType"/></param>
        /// <returns><see cref="Song"/></returns>
        public Song GetRandomSong(GameMoodType mood = GameMoodType.Default) {
            Tuple<GameMoodType, Song> data = AUDIO_Songs.Find(song => song.Item1 == mood);
            if (data != null)
                return data.Item2;

            return null;
        }

    }
}
