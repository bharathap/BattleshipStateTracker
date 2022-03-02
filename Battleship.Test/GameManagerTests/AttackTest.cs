using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship.Application;
using Battleship.Core.Exceptions;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Battleship.Test.GameManagerTests
{
    public class AttackTest : BaseTest
    {
        [Fact]
        public void Attack_WithParams_AttackWithoutShips_Miss()
        {
            var command = "attack 1 1";
            var result = GameManager.Attack(1, command.Split(null).Skip(1).ToArray());

            result.Should().Be(GameResult.Miss);
        }

        [Fact]
        public void Attack_WithParams_AttackWithShips_Hit()
        {
            var command = "addship 1 1 1 5 h";
            var result = GameManager.AddShip(command.Split(null).Skip(1).ToArray());
            result.Should().Be(true);

            command = "attack 1 1";
            var hitResult = GameManager.Attack(2, command.Split(null).Skip(1).ToArray());

            hitResult.Should().Be(GameResult.Hit);
        }

        [Fact]
        public void Attack_WithParams_AttackWithShips_Until_Sink()
        {
            var command = "addship 1 1 1 5 h";
            var result = GameManager.AddShip(command.Split(null).Skip(1).ToArray());
            result.Should().Be(true);

            command = "addship 1 1 2 5 h";
            result = GameManager.AddShip(command.Split(null).Skip(1).ToArray());
            result.Should().Be(true);

            command = "attack 1 1";
            var hitResult = GameManager.Attack(2, command.Split(null).Skip(1).ToArray());
            command = "attack 2 1";
            hitResult = GameManager.Attack(2, command.Split(null).Skip(1).ToArray());
            command = "attack 3 1";
            hitResult = GameManager.Attack(2, command.Split(null).Skip(1).ToArray());
            command = "attack 4 1";
            hitResult = GameManager.Attack(2, command.Split(null).Skip(1).ToArray());
            command = "attack 5 1";
            hitResult = GameManager.Attack(2, command.Split(null).Skip(1).ToArray());

            hitResult.Should().Be(GameResult.Sink);
        }

        [Fact]
        public void Attack_WithParams_AttackWithShips_Until_GameOver()
        {
            var command = "addship 1 1 1 5 h";
            var result = GameManager.AddShip(command.Split(null).Skip(1).ToArray());
            result.Should().Be(true);

            command = "attack 1 1";
            var hitResult = GameManager.Attack(2, command.Split(null).Skip(1).ToArray());
            command = "attack 2 1";
            hitResult = GameManager.Attack(2, command.Split(null).Skip(1).ToArray());
            command = "attack 3 1";
            hitResult = GameManager.Attack(2, command.Split(null).Skip(1).ToArray());
            command = "attack 4 1";
            hitResult = GameManager.Attack(2, command.Split(null).Skip(1).ToArray());
            command = "attack 5 1";
            hitResult = GameManager.Attack(2, command.Split(null).Skip(1).ToArray());

            hitResult.Should().Be(GameResult.GameOver);
        }

        [Fact]
        public void Attack_WithParams_AttackWithShips_InvalidCoordinates_MustFail()
        {
            var command = "addship 1 1 1 5 h";
            var result = GameManager.AddShip(command.Split(null).Skip(1).ToArray());
            result.Should().Be(true);

            command = "attack 11 25";
            var hitResult = Assert.Throws<BattleshipValidationException>(() => GameManager.Attack(2, command.Split(null).Skip(1).ToArray()));
            
            hitResult.Message.Should().Be("Given coordinates are outside the playing board");
        }

        [Fact]
        public void Attack_WithParams_AttackWithShips_BoundaryCoordinates_MustFail()
        {
            var command = "addship 1 1 10 10 h";
            var result = GameManager.AddShip(command.Split(null).Skip(1).ToArray());
            result.Should().Be(true);

            command = "attack 11 11";
            var errorResult = Assert.Throws<BattleshipValidationException>(() => GameManager.Attack(2, command.Split(null).Skip(1).ToArray()));

            errorResult.Message.Should().Be("Given coordinates are outside the playing board");

            command = "attack 10 10";
            var hitResult = GameManager.Attack(2, command.Split(null).Skip(1).ToArray());
            hitResult.Should().Be(GameResult.Hit);
        }
    }
}
