using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship.Application;
using Battleship.Test.Mocks;

namespace Battleship.Test.GameManagerTests
{
    public abstract class BaseTest
    {
        protected readonly ILogger<GameManager> GameManagerLogger;
        protected readonly IGameManager GameManager;

        protected BaseTest()
        {
            GameManagerLogger = Substitute.For<MockLogger<GameManager>>();
            GameManager = new GameManager(GameManagerLogger, 10, 10);
        }
    }
}
