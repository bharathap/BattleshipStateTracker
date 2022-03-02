using System.Linq;
using Battleship.Application;
using Battleship.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace BattleshipChallenge
{
    public class CommandController : ICommandController
    {
        private readonly ILogger<CommandController> _logger;
        private readonly IGameManager _gameManager;
        private bool _gameStarted = false;
        private int _currentPlayerId;

        public CommandController(ILogger<CommandController> logger, IGameManager gameManager)
        {
            _logger = logger;
            _gameManager = gameManager;
        }

        public string ExecuteAction(string command)
        {
            var args = command?.Split(null);
            if (args.Length == 0)
            {
                _logger.LogWarning("Invalid command..");
            }
            
            switch (args[0].ToLower())
            {
                case GameCommands.AddShip:
                    if (_gameStarted)
                    {
                        throw new BattleshipValidationException("Cannot add ships, the game has already started");
                    }
                    return _gameManager.AddShip(args.Skip(1).ToArray()) ? "Ship successfully added" : "Error occurred while adding ship, please check details and try again";
                case GameCommands.Start:
                    var gameStatus = _gameManager.GetStatus();
                    if (gameStatus.player1Ships <= 0 || gameStatus.player2Ships <= 0 )
                    {
                        throw new BattleshipValidationException("Cannot start the game yet. Please add ships for both players and type \"start\"");
                    }
                    _gameStarted = true;
                    _currentPlayerId = 1;
                    return "Game started. Player 1 begin attacking";
                case GameCommands.Attack:
                    if (!_gameStarted)
                    {
                        throw new BattleshipValidationException("Cannot attack just yet. Please finish adding ships for both players and type \"start\"");
                    }
                    return ValidateResult(_gameManager.Attack(_currentPlayerId, args.Skip(1).ToArray()));
                case GameCommands.Restart:
                    _gameStarted = false;
                    _gameManager.StartGame();
                    _currentPlayerId = 1;
                    return "Game restarted, please add ships";
                case GameCommands.Status:
                    var status = _gameManager.GetStatus();
                    return $"Remaining ships - Player1: {status.player1Ships}, Player2: {status.player2Ships}";
                default:
                    return "Invalid command..";
            }
        }

        private int SwitchPlayer()
        {
            _currentPlayerId = 3 - _currentPlayerId;
            return _currentPlayerId;
        }

        private string ValidateResult(GameResult result)
        {
            switch (result)
            {
                case GameResult.Sink:
                    SwitchPlayer();
                    return $"Great hit, the ship was sunk. Player {_currentPlayerId}'s turn to attack.";
                case GameResult.Hit:
                    SwitchPlayer();
                    return $"Great hit. Player {_currentPlayerId}'s turn to attack.";
                case GameResult.GameOver:
                    return $"Well done Player {_currentPlayerId} won !!! Type \"restart\" to play again";
                default:
                    SwitchPlayer();
                    return $"It was a miss, better luck next time. Player {_currentPlayerId}'s turn to attack.";
            }
        }
    }
}
