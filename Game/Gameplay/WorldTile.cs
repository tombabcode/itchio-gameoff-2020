using TBEngine.Models.Gameplay;
using TBEngine.Types;

namespace GameJam.Gameplay {
    /// <summary>
    /// In-game map's tile
    /// </summary>
    public sealed class WorldTile : SquareWallBase {

        public const int SIZE = 224;

        public int ID { get; private set; }
        public bool IsWall { get; private set; }

        public WorldTile(int id, int x, int y, bool isWall, CollisionType[] collisions = null) : base(x, y, SIZE, collisions) {
            ID = id;
            IsWall = isWall;
        }

    }
}
