namespace GameJam.Gameplay {
    public sealed class WorldTile {

        public const int SIZE = 32;

        public int ID { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public int DisplayX => X * 32;
        public int DisplayY => Y * 32;

        public WorldTile(int id, int x, int y) {
            ID = id;
            X = x;
            Y = y;
        }

    }
}
