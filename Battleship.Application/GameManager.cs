using System.Drawing;
using System.Linq;
using Battleship.Core;
using Battleship.Core.Entities;
using Battleship.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Battleship.Application
{
    public class GameManager : IGameManager
    {
        private Game _game;
        readonly ILogger<GameManager> _logger;
        
        public GameManager(ILogger<GameManager> logger, int? width = null, int? height = null)
        {
            _logger = logger;
            StartGame();
        }

        public void StartGame()
        {
            _game = new Game();
            _logger.LogInformation("A 2 player battleship game is initialised with 10*10 boards");
        }

        public bool AddShip(params string[] args)
        {
            if (args.Length != 5 || !int.TryParse(args[0], out var playerNumber) || !int.TryParse(args[1], out var x) 
                || !int.TryParse(args[2], out var y) || !int.TryParse(args[3], out var length) || (!args[4].Equals("v") && !args[4].Equals("h"))
                || playerNumber > 2)
            {
                throw new BattleshipValidationException("Invalid parameters");
            }

            return AddShip(playerNumber, new Point(x - 1, y - 1), length,
                args[4].Equals("v") ? Orientation.Vertical : Orientation.Horizontal);
        }

        public bool AddShip(int playerNumber, Point startPoint, int length, Orientation orientation)
        {
            var board = _game.GameBoards[playerNumber];
            if (board == null)
            {
                throw new BattleshipValidationException("Invalid player number");
            }

            if (startPoint.X < 0 || startPoint.Y < 0 || 
                startPoint.X + (orientation == Orientation.Horizontal ? length : 0) > board.State.GetUpperBound(0) + 1 ||
                startPoint.Y + (orientation == Orientation.Horizontal ? 0 : length) > board.State.GetUpperBound(1) + 1)
            {
                throw new BattleshipValidationException("Given coordinates are outside the playing board");
            }

            return board.AddShip(startPoint, length, orientation);
        }

        public GameResult Attack(int playerNumber, params string[] args)
        {
            if (args.Length != 2 || !int.TryParse(args[0], out var x) || !int.TryParse(args[1], out var y))
            {
                throw new BattleshipValidationException("Invalid parameters..");
            }

            return Attack(playerNumber, new Point(x - 1, y - 1));
        }

        public GameResult Attack(int playerNumber, Point startPoint)
        {
            var result = GameResult.Miss;
            var board = _game.GameBoards[3 - playerNumber];

            if (board == null)
            {
                throw new BattleshipValidationException("Invalid player number");
            }

            if (startPoint.X < 0 || startPoint.Y < 0 ||
                startPoint.X > board.State.GetUpperBound(0) ||
                startPoint.Y > board.State.GetUpperBound(1))
            {
                throw new BattleshipValidationException("Given coordinates are outside the playing board");
            }

            var shipId = board.Attack(startPoint);
            if (shipId != null)
            {
                result = board.Ships.FirstOrDefault(s => s.Id.Equals(shipId))?.IsSunk ?? false ? GameResult.Sink : GameResult.Hit;
                if (result == GameResult.Sink && board.Ships.All(s => s.IsSunk))
                {
                    result = GameResult.GameOver;
                }
            }

            return result;
        }

        public (int player1Ships, int player2Ships) GetStatus()
        {
            var player1Count = _game.GameBoards[1].Ships.Count(s => !s.IsSunk);
            var player2Count = _game.GameBoards[2].Ships.Count(s => !s.IsSunk);
            return new(player1Count, player2Count);
        }
    }
}