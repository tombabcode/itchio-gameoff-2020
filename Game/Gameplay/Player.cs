using GameJam.Services;
using Microsoft.Xna.Framework;
using System;
using TBEngine.Models.Gameplay;
using TBEngine.Types;
using TBEngine.Utils;

namespace GameJam.Gameplay {
    public sealed class Player : CharacterBase {

        // Texture display coords
        public float DisplayX => X + 8;
        public float DisplayY => Y + 34;

        // Positions
        public int OnMapX => (int)(X / WorldTile.SIZE);
        public int OnMapY => (int)(Y / WorldTile.SIZE);

        // Attributes
        public string Name { get; private set; }
        public float Speed { get; private set; } = 512;

        public Player(string name, float x, float y) {
            Name = name;
            X = x;
            Y = y;
            CollisionBounds = new Rectangle(-75, -23, 134, 42);
        }

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

        public Rectangle GetDisplayData(GameTime time, ContentService content) {
            float value = (float)Math.Cos(MathHelper.ToRadians(360 * ((float)(time.TotalGameTime.TotalMilliseconds % 1000) / 1000)));
            return new Rectangle(
                (int)DisplayX,
                (int)DisplayY,
                (int)(content.TEXCharacter.Width),
                (int)(content.TEXCharacter.Height + value * 20) 
            );;
        }

    }
}
