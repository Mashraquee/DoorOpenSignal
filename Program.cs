using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoorOpenSigna
{
    internal class Program
    {
        enum SystemState
        {
            IDLE,
            RUNNING,
            MAINTENANCE
        }
        private static SystemState _currentState = SystemState.IDLE;
        private static CancellationTokenSource _gameCancellation;
        private static Task _gameTask;
        static void Main(string[] args)
        {
            Console.WriteLine("System started.");
            StartGame();

            while (true)
            {
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                HandleCommand(input.Trim().ToLower());
            }
        }

        static void StartGame()
        {
            _currentState = SystemState.RUNNING;
            _gameCancellation = new CancellationTokenSource();
            _gameTask = Task.Run(() => GameLoop(_gameCancellation.Token));

            Console.WriteLine("Game started.");
        }

        static void GameLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    Console.WriteLine("Game running...");
                    Thread.Sleep(1000);
                }
            }
            catch (OperationCanceledException ex)
            {
                // Expected during cancellation
                throw ex;
            }
            finally
            {
                Console.WriteLine("Game stopped cleanly.");
            }
        }

        static void HandleCommand(string command)
        {
            if (command == "signal door_open")
            {
                HandleDoorOpenSignal();
            }
            else
            {
                Console.WriteLine("Unknown command.");
            }
        }

        static void HandleDoorOpenSignal()
        {
            if (_currentState == SystemState.MAINTENANCE)
                return;

            // Transition to maintenance
            _currentState = SystemState.MAINTENANCE;

            Console.WriteLine("Door opened – entering maintenance mode.");

            // Stop game loop cleanly
            if (_gameCancellation != null && !_gameCancellation.IsCancellationRequested)
            {
                _gameCancellation.Cancel();
                _gameTask?.Wait();
            }
        }
    }
}


