using GameJam.Interfaces;
using GameJam.Services;
using Microsoft.Xna.Framework;

namespace GameJam.Gameplay {
    public sealed class Player : IGamePosition, ICollision {

        public float PositionX { get; private set; }
        public float PositionY { get; private set; }
        public Rectangle CollisionBounds { get; private set; }

        public string Name { get; private set; }

        public Player(string name) {
            Name = name;
            CollisionBounds = new Rectangle(0, 0, 64, 48);
        }

        public void Update(InputService input, ConfigurationService config, GameTime time) {
            if (input.IsKeyPressed(config.KEY_MoveUp)) PositionY -= 1;
            if (input.IsKeyPressed(config.KEY_MoveDown)) PositionY += 1;
            if (input.IsKeyPressed(config.KEY_MoveLeft)) PositionX -= 1;
            if (input.IsKeyPressed(config.KEY_MoveRight)) PositionX += 1;
        }

    }
}
