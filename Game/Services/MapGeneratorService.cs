using System.Collections.Generic;
using GameJam.Gameplay;
using TBEngine.Types;
using TBEngine.Utils;

using UH = TBEngine.Utils.UtilsHelper;

namespace GameJam.Services {
    /// <summary>
    /// Generates world map for levels using cellular automaton cave algorithm
    /// </summary>
    public static class MapGeneratorService {

        private const float _generatorWallRoomRation = .4f;
        private const int _generatorSteps = 6;

        /// <summary>
        /// Creates map
        /// </summary>
        /// <param name="width">Width of the map</param>
        /// <param name="height">Height of the map</param>
        /// <returns>Array of tiles</returns>
        public static WorldTile[] GenerateMap(int width, int height) {
            bool[,] cells = new bool[width, height];

            // Fill map with 1 or 0 randomly
            UH.Loops(width, height, (x, y) => cells[x, y] = x == 0 || y == 0 || x == width - 1 || y == height - 1 || RandomService.GetRandomBool(_generatorWallRoomRation));

            // Simulation
            for (int i = 0; i < _generatorSteps; i++)
                cells = SimulationStep(cells);

            // Set map
            WorldTile[ ] map = new WorldTile[width * height];
            UH.Loops(width, height, (x, y) => map[y * width + x] = new WorldTile(y * width + x, x, y, cells[x, y], new CollisionType[0]));

            // Set walls' collisions
            UH.Loops(width, height, (x, y) => {
                WorldTile tile = map[y * width + x];
                if (!tile.IsWall)
                    return;

                List<CollisionType> collision = new List<CollisionType>( );

                if (x - 1 < 0 || !map[y * width + x - 1].IsWall) collision.Add(CollisionType.Left);
                if (x + 1 >= width || !map[y * width + x + 1].IsWall) collision.Add(CollisionType.Right);
                if (y - 1 < 0 || !map[(y - 1) * width + x].IsWall) collision.Add(CollisionType.Top);
                if (y + 1 >= height || !map[(y + 1) * width + x].IsWall) collision.Add(CollisionType.Bottom);

                tile.Collisions = collision.ToArray( );
            });

            return map;
        }

        /// <summary>
        /// Cellular automaton step
        /// </summary>
        /// <param name="original">Original walls map</param>
        /// <returns>Transformed walls map</returns>
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

        /// <summary>
        /// How many walls are around given point
        /// </summary>
        /// <param name="original">Original walls map</param>
        /// <param name="centerX">Point's X</param>
        /// <param name="centerY">Point's Y</param>
        /// <returns>Amount of walls around given point</returns>
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
