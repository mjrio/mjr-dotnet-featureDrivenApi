using System;
using System.Linq;
using Poc.Domain.GameModule;

namespace Poc.SnakeConsole
{


    class Program
    {
        static void Main(string[] args)
        {
            var store = Bootstrapper.Start().Result;
            SnakeEgine snakeEgine = new SnakeEgine(store);

            while (true)
            {

                Console.WriteLine("New Game:<Enter>");
                Console.WriteLine("Quit: <Esc>");
                Console.WriteLine("History: 1-2-3");
                ConsoleKeyInfo consoleKey = Console.ReadKey();
                Console.Clear();
                bool isReplaying=false;
                
                Snake snake = null;

                if (consoleKey.Key == ConsoleKey.NumPad1)
                {
                    isReplaying = true;
                    snake = store.GetByIdAsync<Snake>("Snake1", 0).Result;
                }
                if (consoleKey.Key == ConsoleKey.NumPad2)
                {
                    isReplaying = true;
                    snake = store.GetByIdAsync<Snake>("Snake2", 0).Result;
                }
                if (consoleKey.Key == ConsoleKey.NumPad3)
                {
                    isReplaying = true;
                    snake = store.GetByIdAsync<Snake>("Snake3", 0).Result;
                }
                if (consoleKey.Key == ConsoleKey.Enter)
                {
                    var games = store.ListStreams("Snake", 5).Result;
                    snake = Snake.Start(games.Count() + 1);
                }
                if (consoleKey.Key == ConsoleKey.Escape)
                {
                    return;
                }

                snakeEgine.Start();
                snakeEgine.Play(snake, isReplaying).Wait();
            }
        }
    }
}