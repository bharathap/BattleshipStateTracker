using System.Drawing;
using System.Linq;
using Battleship.Application;
using Battleship.Core.Exceptions;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Battleship.Test.CommandControllerTests
{
    public class CommandControllerTest : BaseTest
    {
        [Fact]
        public void AddShip_WithParams_MustPass()
        {
            var command = "addship 1 1 1 5 h";
            GameManager.AddShip(Arg.Any<string[]>()).Returns(r => true);
            var result = CommanController.ExecuteAction(command);
            result.Should().Be("Ship successfully added");
        }

        [Fact]
        public void StartGame_WithNoShipsAdded_MustFail()
        {
            var command = "start";
            GameManager.GetStatus().Returns(r => (0, 0));
            
            var result = Assert.Throws<BattleshipValidationException>(() => CommanController.ExecuteAction("start"));
            result.Message.Should().Be("Cannot start the game yet. Please add ships for both players and type \"start\"");
        }

        [Fact]
        public void AddShip_WithParams_WhenGameIsStarted_MustFail()
        {
            var command = "addship 1 1 1 5 h";

            GameManager.GetStatus().Returns(r => (1, 1));
            GameManager.AddShip(Arg.Any<string[]>()).Returns(r => true);

            var result = CommanController.ExecuteAction("start");
            result.Should().Be("Game started. Player 1 begin attacking");

            var addResult = Assert.Throws<BattleshipValidationException>(() => CommanController.ExecuteAction(command));
            addResult.Message.Should().Be("Cannot add ships, the game has already started");
        }

        [Fact]
        public void Attack_WhenGameIsNotStarted_MustFail()
        {
            var command = "attack 1 1";
            
            var result = Assert.Throws<BattleshipValidationException>(() => CommanController.ExecuteAction(command));
            result.Message.Should().Be("Cannot attack just yet. Please finish adding ships for both players and type \"start\"");
        }

        [Fact]
        public void Attack_WhenGameIsStarted_MustPass()
        {
            var command = "attack 1 1";

            GameManager.GetStatus().Returns(r => (1, 1));
            GameManager.Attack(Arg.Any<int>(), Arg.Any<string []>()).Returns(r => GameResult.Hit);
            var result = CommanController.ExecuteAction("start");

            result = CommanController.ExecuteAction(command);
            result.Should().StartWith("Great hit");
        }
    }
}