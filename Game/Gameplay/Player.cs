using System;
using Microsoft.Xna.Framework;
using GameJam.Services;
using TBEngine.Models.Gameplay;
using TBEngine.Types;
using TBEngine.Utils;

using DH = TBEngine.Utils.DisplayHelper;
using TBEngine.Textures;

namespace GameJam.Gameplay {
    /// <summary>
    /// Player
    /// </summary>
    public sealed class Player : CharacterBase {

        // References
        private readonly ConfigurationService _config;
        private readonly TextureStatic _texture;

        // Texture display coords
        public float DisplayX => X + 8;
        public float DisplayY => Y + 34;

        // Positions
        public int OnMapX => (int)(X / WorldTile.SIZE);
        public int OnMapY => (int)(Y / WorldTile.SIZE);

        // Attributes
        public string Name { get; private set; }
        public float Speed { get; private set; } = 512;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Player's name</param>
        /// <param name="x">Spawn point X</param>
        /// <param name="y">Spawn point Y</param>
        public Player(ContentService content, ConfigurationService config, string name, float x, float y) {
            _config = config;
            _texture = content.TEXCharacter;

            Name = name;
            X = x;
            Y = y;
            CollisionBounds = new Rectangle(-75, -23, 134, 42);
        }

        /// <summary>
        /// Player's update movement
        /// </summary>
        /// <param name="input"><see cref="InputService"/></param>
        /// <param name="config"><see cref="ConfigurationService"/></param>
        /// <param name="time"><see cref="GameTime"/></param>
        /// <param name="level"><see cref="Level"/></param>
        public void Update(InputService input, ConfigurationService config, GameTime time, Level level) {
            OldX = X;
            OldY = Y;

            VelocityX = 0;
            VelocityY = 0;

            if (input.IsKeyPressed(config.KEY_MoveUp)) VelocityY = -Speed * (float)time.ElapsedGameTime.TotalSeconds;
            if (input.IsKeyPressed(config.KEY_MoveDown)) VelocityY = Speed * (float)time.ElapsedGameTime.TotalSeconds;
            if (input.IsKeyPressed(config.KEY_MoveLeft)) VelocityX = -Speed * (float)time.ElapsedGameTime.TotalSeconds;
            if (input.IsKeyPressed(config.KEY_MoveRight)) VelocityX = Speed * (float)time.ElapsedGameTime.TotalSeconds;

            X += VelocityX;
            Y += VelocityY;

            // Collision detection on move
            bool horizontalCollision = false;
            bool verticalCollision = false;
            UtilsHelper.Loops(3, 3, (x, y) => {
                int posx = OnMapX - 1 + x;
                int posy = OnMapY - 1 + y;

                if (posx < 0 || posy < 0 || posx >= level.Width || posy >= level.Height || !level.Map[posy * level.Width + posx].IsWall)
                    return;

                CollisionType collision = UtilsHelper.CheckCollision(this, level.Map[posy * level.Width + posx]);

                if (!horizontalCollision && (collision == CollisionType.Left || collision == CollisionType.Right)) {
                    horizontalCollision = true;
                    X = OldX;
                }

                if (!verticalCollision && (collision == CollisionType.Top || collision == CollisionType.Bottom)) {
                    verticalCollision = true;
                    Y = OldY;
                }
            });
        }

        /// <summary>
        /// Get 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public void Display(GameTime time) {
            if (_config.DebugMode)
                DH.Border(CollisionPosition, thickness: 4);

            float value = (float)Math.Cos(MathHelper.ToRadians(360 * ((float)(time.TotalGameTime.TotalMilliseconds % 1000) / 1000)));
            DH.Raw(_texture.Texture, DisplayX, DisplayY, _texture.Width, _texture.Height + (value * 20), align: AlignType.CB);
        }

    }
}
