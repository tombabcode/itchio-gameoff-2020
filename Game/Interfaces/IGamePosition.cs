namespace GameJam.Interfaces {
    public interface IGamePosition {

        // Original position coordinates
        float PositionX { get; }
        float PositionY { get; }

        // Center for the texture
        float DisplayX { get; }
        float DisplayY { get; }

    }
}
