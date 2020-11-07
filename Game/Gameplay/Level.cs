using System;
using TBEngine.Utils;

namespace GameJam.Gameplay {
    public sealed class Level {

        public WorldTile[] Map { get; private set; }

        public void GenerateMap(int width, int height) {
            Map = new WorldTile[width * height];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++) {
                    if (x == 0 || y == 0 || x == width - 1 || y == height - 1) {
                        Map[y * width + x] = new WorldTile(1, x, y);
                        continue;
                    }

                    Map[y * width + x] = new WorldTile(y == height / 2 ? 0 : (RandomService.GetRandomBool( ) ? 1 : 0), x, y);
                }
        }

    }
}
