using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship.Application;
using Battleship.Core.Entities;
using Battleship.Test.Mocks;
using BattleshipChallenge;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Battleship.Test.CommandControllerTests
{
    public abstract class BaseTest
    {
        protected readonly ILogger<CommandController> MockLogger;
        protected readonly ILogger<GameManager> MockGameManagerLogger;
        protected readonly IGameManager GameManager;
        protected readonly ICommandController CommanController;

        protected BaseTest()
        {
            MockLogger = Substitute.For<MockLogger<CommandController>>();
            MockGameManagerLogger = Substitute.For<MockLogger<GameManager>>();
            GameManager = Substitute.For<IGameManager>();
            CommanController = new CommandController(MockLogger, GameManager);
        }
    }
}
