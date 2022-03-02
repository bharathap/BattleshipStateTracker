using FluentAssertions;
using System;
using System.Linq;
using Battleship.Core.Exceptions;
using Xunit;

namespace Battleship.Test.GameManagerTests
{
    public class AddShipTest : BaseTest
    {
        [Fact]
        public void AddShip_WithParams_AddValidShipToEmptyBoard_MustAddShips()
        {
            var command = "addship 1 1 1 5 h";
            var result = GameManager.AddShip(command.Split(null).Skip(1).ToArray());

            result.Should().Be(true);
            var status = GameManager.GetStatus();

            status.Should().NotBeNull();
            status.player1Ships.Should().Be(1);
            status.player2Ships.Should().Be(0);
        }

        [Fact]
        public void AddShip_WithParams_AddValidMultipleShips_ToPlayer1__MustAddShips()
        {
            var command = "addship 1 1 1 5 h";
            var result = GameManager.AddShip(command.Split(null).Skip(1).ToArray());
            result.Should().Be(true);

            command = "addship 1 1 2 5 v";
            result = GameManager.AddShip(command.Split(null).Skip(1).ToArray());
            result.Should().Be(true);

            var status = GameManager.GetStatus();

            status.Should().NotBeNull();
            status.player1Ships.Should().Be(2);
            status.player2Ships.Should().Be(0);
        }

        [Fact]
        public void AddShip_WithParams_AddValidMultipleShips_ToPlayer2__MustAddShips()
        {
            var command = "addship 2 1 1 5 h";
            var result = GameManager.AddShip(command.Split(null).Skip(1).ToArray());
            result.Should().Be(true);

            command = "addship 2 1 2 5 v";
            result = GameManager.AddShip(command.Split(null).Skip(1).ToArray());
            result.Should().Be(true);

            var status = GameManager.GetStatus();

            status.Should().NotBeNull();
            status.player1Ships.Should().Be(0);
            status.player2Ships.Should().Be(2);
        }

        [Fact]
        public void AddShip_WithParams_AddValidMultipleShips_WithOverlapCoordinates_ForDifferentPlayes__MustAddShips()
        {
            var command = "addship 1 1 1 5 h";
            var result = GameManager.AddShip(command.Split(null).Skip(1).ToArray());
            result.Should().Be(true);

            command = "addship 2 1 2 5 v";
            result = GameManager.AddShip(command.Split(null).Skip(1).ToArray());
            result.Should().Be(true);

            var status = GameManager.GetStatus();

            status.Should().NotBeNull();
            status.player1Ships.Should().Be(1);
            status.player2Ships.Should().Be(1);
        }

        [Fact]
        public void AddShip_WithParams_InvalidPlayerId__MustThrowError()
        {
            var command = "addship 5 1 1 5 h";

            var ex = Assert.Throws<BattleshipValidationException>(() =>
                GameManager.AddShip(command.Split(null).Skip(1).ToArray()));
            ex.Message.Should().Be("Invalid parameters");
        }

        [Fact]
        public void AddShip_WithParams_InvalidCoordinates__MustThrowError()
        {
            var command = "addship 1 11 1 5 h";

            var ex = Assert.Throws<BattleshipValidationException>(() =>
                GameManager.AddShip(command.Split(null).Skip(1).ToArray()));
            ex.Message.Should().Be("Given coordinates are outside the playing board");
        }

        [Fact]
        public void AddShip_WithParams_MissingAndAdditionalArgs__MustThrowError()
        {
            var command = "addship 1 11 1 5";

            var ex = Assert.Throws<BattleshipValidationException>(() =>
                GameManager.AddShip(command.Split(null).Skip(1).ToArray()));
            ex.Message.Should().Be("Invalid parameters");

            command = "addship 1 11 1 5 h additional";

            ex = Assert.Throws<BattleshipValidationException>(() =>
                GameManager.AddShip(command.Split(null).Skip(1).ToArray()));
            ex.Message.Should().Be("Invalid parameters");
        }

        [Fact]
        public void AddShip_WithParams_InvalidArgumentDataType__MustThrowError()
        {
            var command = "addship 1 h 1 5 h";

            var ex = Assert.Throws<BattleshipValidationException>(() =>
                GameManager.AddShip(command.Split(null).Skip(1).ToArray()));
            ex.Message.Should().Be("Invalid parameters");

            command = "addship 1 1 1 l h additional";

            ex = Assert.Throws<BattleshipValidationException>(() =>
                GameManager.AddShip(command.Split(null).Skip(1).ToArray()));
            ex.Message.Should().Be("Invalid parameters");
        }
    }
}
