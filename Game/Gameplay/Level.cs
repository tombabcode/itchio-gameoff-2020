using GameJam.Services;
using GameJam.Types;
using System.Collections.Generic;
using TBEngine.Types;
using TBEngine.Utils;

namespace GameJam.Gameplay {
    public sealed class Level {

        public WorldTile[] Map { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public void GenerateMap(int width, int height) {
            Width = width;
            Height = height;

            Map = MapGeneratorService.GenerateMap(Width, Height);
        }

        public void Test( ) {
            UtilsHelper.Loops(Width, Height, (x, y) => {
                WorldTile tile = Map[y * Width + x];
                if (!tile.IsWall)
                    return;

                List<CollisionType> collision = new List<CollisionType>( );

                if (x - 1 < 0 || !Map[y * Width + x - 1].IsWall)
                    collision.Add(CollisionType.Left);
                if (x + 1 >= Width || !Map[y * Width + x + 1].IsWall)
                    collision.Add(CollisionType.Right);
                if (y - 1 < 0 || !Map[(y - 1) * Width + x].IsWall)
                    collision.Add(CollisionType.Top);
                if (y + 1 >= Height || !Map[(y + 1) * Width + x].IsWall)
                    collision.Add(CollisionType.Bottom);

                tile.Collisions = collision.ToArray( );
            });
        }

    }
}
