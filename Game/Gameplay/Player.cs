using GameJam.Interfaces;
using GameJam.Services;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Gameplay {
    public sealed class Player : IGamePosition, ICollision {

        // Positions
        public float PositionX { get; private set; }
        public float PositionY { get; private set; }
        public float DisplayX => PositionX - 7;
        public float DisplayY => PositionY + 35;
        public float CollisionX => (int)PositionX + CollisionBounds.X;
        public float CollisionY => (int)PositionY + CollisionBounds.Y;
        public int OnMapX => (int)(PositionX / WorldTile.SIZE);
        public int OnMapY => (int)(PositionY / WorldTile.SIZE);

        // Collision
        public Rectangle CollisionBounds { get; private set; }

        // Attributes
        public string Name { get; private set; }
        public float Speed { get; private set; } = 512;

        public bool moving;

        public Player(string name) {
            Name = name;
            CollisionBounds = new Rectangle(-100, -25, 206, 56);
        }

        public void Spawn(float x, float y) {
            PositionX = x;
            PositionY = y;
        }

        public void Update(InputService input, ConfigurationService config, GameTime time) {
            if (input.IsKeyPressed(config.KEY_MoveUp)) PositionY -= Speed * (float)time.ElapsedGameTime.TotalSeconds;
            if (input.IsKeyPressed(config.KEY_MoveDown)) PositionY += Speed * (float)time.ElapsedGameTime.TotalSeconds;
            if (input.IsKeyPressed(config.KEY_MoveLeft)) PositionX -= Speed * (float)time.ElapsedGameTime.TotalSeconds;
            if (input.IsKeyPressed(config.KEY_MoveRight)) PositionX += Speed * (float)time.ElapsedGameTime.TotalSeconds;

            moving = input.IsAnyKeyPressed( );
        }

        public Rectangle GetDisplayData(GameTime time, ContentService content) {
            float value = (float)Math.Cos(MathHelper.ToRadians(360 * ((float)(time.TotalGameTime.TotalMilliseconds % (moving ? 400 : 1000)) / (moving ? 400 : 1000))));
            return new Rectangle(
                (int)DisplayX,
                (int)DisplayY,
                (int)(content.TEXCharacter.Width),
                (int)(content.TEXCharacter.Height + value * 20) 
            );;
        }

    }
}
