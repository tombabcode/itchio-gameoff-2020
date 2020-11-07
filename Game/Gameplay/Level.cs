using GameJam.Services;

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

    }
}
