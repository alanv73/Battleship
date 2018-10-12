using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipGame
{
    struct ShipPoint
    {
        public int spX;
        public int spY;
    }

    class Battleship
    {
        private readonly static int boardsize = 10;
        private string[,] gameBoard = new string[boardsize, boardsize];
        private readonly Random rnd = new Random();
        private ShipPoint shipPos = new ShipPoint();

        public bool Sunk { get; private set; } = false;
        public bool Dupe { get; private set; } = false;

        public Battleship()
        {
            for (int y = 0; y < boardsize; y++)
            {
                for (int x = 0; x < boardsize; x++)
                {
                    gameBoard[x,y] = "O";
                }
            }
            LaunchShip();
        }

        public void PrintBoard()
        {
            for (int y = 0; y < boardsize; y++)
            {
                Console.Write(new string(' ', 8));
                for (int x = 0; x < boardsize; x++)
                {
                    if (gameBoard[x, y] == "O")
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else if (gameBoard[x, y] == "*")
                        Console.ForegroundColor = ConsoleColor.Green;
                    else if (gameBoard[x, y] == "X")
                        Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(gameBoard[x, y] + " ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.WriteLine();
            }
        }

        public void LaunchShip()
        {
            shipPos.spX = rnd.Next(boardsize);
            shipPos.spY = rnd.Next(boardsize);
        }

        public string GetShipPos(out int xPos, out int yPos)
        {
            xPos = shipPos.spX;
            yPos = shipPos.spY;
            return "( " + shipPos.spX + ", " + shipPos.spY + " )";
        }

        public bool ShootAtShip(int xcoord, int ycoord)
        {
            if (xcoord == shipPos.spX && ycoord == shipPos.spY)
            {
                Sunk = true;
                ShotHit(xcoord, ycoord);
                return true;
            }
            ShotMiss(xcoord, ycoord);
            return false;
        }

        public void PrintTitle()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\n---");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("====");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("<<<<");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(" BATTLESHIP ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(">>>>");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("====");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("---\n");
        }

        private void ShotMiss(int shotx, int shoty)
        {
            if (gameBoard[shotx, shoty] == "*")
                Dupe = true;
            else
            {
                gameBoard[shotx, shoty] = "*";
                Dupe = false;
            }
        }

        private void ShotHit(int shotx, int shoty)
        {
            gameBoard[shotx, shoty] = "X";
        }

    } //end battleship class

    class Program
    {
        static void Main(string[] args)
        {
            bool shipSunk = false;
            int guesses = 1;
            bool goodguess = true;
            int guessX = 0, guessY = 0;
            Battleship myBS = new Battleship();
            string shipLocation = myBS.GetShipPos(out int shipPosX, out int shipPosY);

            while (!shipSunk)
            {
                Console.Clear();
                Console.WriteLine(shipLocation);
                myBS.PrintTitle();
                Console.WriteLine();
                myBS.PrintBoard();

                do
                {
                    if (myBS.Dupe)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" \nYou already guessed that spot, guess again.");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    Console.Write("\nGuess #{0} | Enter X Coordinate: ", guesses);
                    try
                    {
                       guessX = Convert.ToInt32(Console.ReadLine());
                       goodguess = true;
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\a\nInvalid Input\n");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        goodguess = false;
                    }
                } while (!goodguess);

                do
                {
                    Console.Write("\nGuess #{0} | Enter Y Coordinate: ", guesses);
                    try
                    {
                        guessY = Convert.ToInt32(Console.ReadLine());
                        goodguess = true;
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\a\nInvalid Input\n");
                        goodguess = false;
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                } while (!goodguess);

                shipSunk = myBS.ShootAtShip(guessX, guessY);

                if (!myBS.Dupe && !myBS.Sunk)
                    guesses++;
            }

            Console.Clear();
            Console.WriteLine(shipLocation);
            myBS.PrintTitle();
            Console.WriteLine();
            myBS.PrintBoard();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\nCongratulations! ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\nYou sank my BATTLESHIP in {0} guesses!", guesses);
            Console.ReadKey();


        }
    }
}
