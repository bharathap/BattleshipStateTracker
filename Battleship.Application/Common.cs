namespace Battleship.Application
{
    public struct GameCommands
    {
        public const string AddShip = "addship";
        public const string Start = "start";
        public const string Attack = "attack";
        public const string Restart = "restart";
        public const string Status = "status";
    }

    public enum GameResult
    {
        None = 0,
        Hit,
        Miss,
        Sink,
        GameOver
    }
}
