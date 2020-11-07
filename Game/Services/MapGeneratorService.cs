using GameJam.Gameplay;
using TBEngine.Utils;

using UH = TBEngine.Utils.UtilsHelper;

namespace GameJam.Services {
    public static class MapGeneratorService {

        private const float _generatorWallRoomRation = .4f;
        private const int _generatorSteps = 6;

        public static WorldTile[] GenerateMap(int width, int height) {
            bool[,] cells = new bool[width, height];

            // Fill map with 1 or 0 randomly
            UH.Loops(width, height, (x, y) => cells[x, y] = x == 0 || y == 0 || x == width - 1 || y == height - 1 ? true : RandomService.GetRandomBool(_generatorWallRoomRation));

            // Simulation
            for (int i = 0; i < _generatorSteps; i++)
                cells = SimulationStep(cells);

            // Set map
            WorldTile[ ] map = new WorldTile[width * height];
            UH.Loops(width, height, (x, y) => map[y * width + x] = new WorldTile(y * width + x, x, y, cells[x, y]));

            return map;
        }

        private static bool[,] SimulationStep(bool[,] original) {
            int width = original.GetLength(0);
            int height = original.GetLength(1);

            bool[,] data = new bool[width, height];

            UH.Loops(width, height, (x, y) => {
                int walls = WallCountAround(original, x, y);

                if (walls > 4)
                    data[x, y] = true;
                else if (walls < 4)
                    data[x, y] = false;
            });

            return data;
        }

        private static int WallCountAround(bool[,] original, int centerX, int centerY) {
            int width = original.GetLength(0);
            int height = original.GetLength(1);
            int result = 0;

            UH.Loops(3, 3, (x, y) => {
                int targetX = centerX - 1 + x;
                int targetY = centerY - 1 + y;

                if (targetX == centerX && targetY == centerY)
                    return;

                if (targetX < 0 || targetX >= width || targetY < 0 || targetY >= height || original[targetX, targetY])
                    result++;
            });

            return result;
        }

    }
}
