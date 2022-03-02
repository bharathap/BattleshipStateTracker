using System;
using Battleship.Application;
using Battleship.Core.Exceptions;
using BattleshipChallenge.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace BattleshipChallenge
{
    internal class Program
    {
        private static IHost _host;
        private static ILogger<Program> _logger;

        static void Main(string[] args)
        {
            _host = Startup();
            _logger = _host.Services.GetRequiredService<ILogger<Program>>();
            PlayGame();
        }

        static IHost Startup()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IGameManager, GameManager>()
                        .AddTransient<ICommandController, CommandController>()
                        .AddLogging(configure => configure.AddConsole());
                }).Build();
        }

        private static void PlayGame()
        {
            bool continueGame = true;

            using var serviceScope = _host.Services.CreateScope();
            var provider = serviceScope.ServiceProvider;
            var controller = provider.GetRequiredService<ICommandController>();

            Console.WriteLine("Welcome to the Battleship Challenge!");
            Console.WriteLine(); 
            Console.WriteLine("For help type help or h, to exit the program type exit or e");

            while (continueGame)
            {
                try
                {
                    var input = Console.ReadLine();

                    switch (input.ToLower())
                    {
                        case "help":
                        case "h":
                            DisplayCommands();
                            break;

                        case "exit":
                        case "e":
                            Console.WriteLine("Are you sure you want to exit (y/n)?");
                            continueGame = Console.ReadLine().EqualsIgnoreCase("y");
                            continueGame = false;
                            break;
                        default:
                            var result = controller.ExecuteAction(input);
                            Console.WriteLine(result);
                            break;
                    }
                }
                catch (BattleshipValidationException bex)
                {
                    _logger.LogWarning(bex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An unknown error occurred. Please retry. {ex.Message}");
                }
            }
        }

        private static void DisplayCommands()
        {
            Console.WriteLine();
            Console.WriteLine(@"addship [playerID (1 or 2)] [x] [y] [length] [orientation]");
            Console.WriteLine(@"Adds a Ship at the given x, y coordinates in the given orientation and length".Indent(1));
            Console.WriteLine(@"orientation: v for vertical  h for horizontal".Indent(2));
            Console.WriteLine();
            Console.WriteLine(@"start");
            Console.WriteLine(@"Call to start the game once all ships are added to the board".Indent(1));
        
            Console.WriteLine();
            Console.WriteLine("attack [x] [y]");
            Console.WriteLine("Attacks the coordinates [x] [y].".Indent(1));
            Console.WriteLine();
            Console.WriteLine("status");
            Console.WriteLine("Displays the number of remaining ships per player".Indent(1));
            Console.WriteLine();
            Console.WriteLine("reset");
            Console.WriteLine("Restarts the game".Indent(1));
            Console.WriteLine();
            Console.WriteLine("exit");
            Console.WriteLine("Exits the game".Indent(1));
            Console.WriteLine();
            Console.WriteLine("For help type -help or -h, to exit the program type -exit or -e");
        }
    }
}
