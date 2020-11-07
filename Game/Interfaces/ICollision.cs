using Microsoft.Xna.Framework;

namespace GameJam.Interfaces {
    public interface ICollision {

        float CollisionX { get; }
        float CollisionY { get; }

        Rectangle CollisionBounds { get; }

    }
}
