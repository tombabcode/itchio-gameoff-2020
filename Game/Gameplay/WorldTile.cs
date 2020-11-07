namespace GameJam.Gameplay {
    public sealed class WorldTile {

        public const int SIZE = 480;

        public int ID { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public bool IsWall { get; private set; }

        public int DisplayX => X * SIZE;
        public int DisplayY => Y * SIZE;

        public WorldTile(int id, int x, int y, bool isWall) {
            ID = id;
            X = x;
            Y = y;
            IsWall = isWall;
        }

    }
}
