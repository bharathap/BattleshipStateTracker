using System.Drawing;
using Battleship.Core;

namespace Battleship.Application
{
    public interface IGameManager
    {
        bool AddShip(int playerNumber, Point startPoint, int length, Orientation orientation);
        bool AddShip(params string[] args);
        GameResult Attack(int playerId, params string[] args);
        GameResult Attack(int playerNumber, Point startPoint);
        void StartGame();
        (int player1Ships, int player2Ships) GetStatus();
    }
}
