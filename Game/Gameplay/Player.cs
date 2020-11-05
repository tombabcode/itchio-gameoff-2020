using GameJam.Interfaces;
using GameJam.Services;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Gameplay {
    public sealed class Player : IGamePosition, ICollision {

        public float PositionX { get; private set; }
        public float PositionY { get; private set; }
        public Rectangle CollisionBounds { get; private set; }

        public string Name { get; private set; }

        public bool moving;

        public Player(string name) {
            Name = name;
            CollisionBounds = new Rectangle(0, 0, 64, 48);
        }

        public void Update(InputService input, ConfigurationService config, GameTime time) {
            if (input.IsKeyPressed(config.KEY_MoveUp)) PositionY -= 1;
            if (input.IsKeyPressed(config.KEY_MoveDown)) PositionY += 1;
            if (input.IsKeyPressed(config.KEY_MoveLeft)) PositionX -= 1;
            if (input.IsKeyPressed(config.KEY_MoveRight)) PositionX += 1;

            moving = input.IsAnyKeyPressed( );
        }

        public Rectangle GetDisplayData(GameTime time, ContentService content) {
            float value = (float)Math.Cos(MathHelper.ToRadians(360 * ((float)(time.TotalGameTime.TotalMilliseconds % (moving ? 400 : 1000)) / (moving ? 400 : 1000))));
            return new Rectangle(
                (int)PositionX,
                (int)PositionY,
                (int)(content.TEXTest.Width),
                (int)(content.TEXTest.Height + value * 20) 
            );;
        }

    }
}
