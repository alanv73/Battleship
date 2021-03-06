﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/********************************************
 * ---====<<<< BATTLESHIP >>>>====----
 * Alan Van Art
 * 10 x 10 game board
 * ship occupies a single space on the board
 * 
 * input X and Y coordinates (0-9) to guess 
 * the location of the ship that is randomly
 * placed on the board. Gameplay continues
 * until the location is correctly guessed.
 ********************************************/

namespace BattleshipGame
{
    struct ShipPoint
    {
        public int spX;
        public int spY;
    }

    class Battleship
    {
        private static int boardsize = 10;
        private string[,] gameBoard = new string[boardsize, boardsize];
        private readonly Random rnd = new Random();
        private ShipPoint shipPos = new ShipPoint();

        public bool Sunk { get; private set; } = false;
        public bool Dupe { get; private set; } = false;
        public int Boardsize { get => boardsize; }

        //class constructor
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

        //prints current state of board
        public void PrintBoard()
        {
            PrintTitle();
            Console.WriteLine();
            for (int y = 0; y < boardsize; y++)
            {
                int lpad = (34 - ((boardsize * 2) - 1)) / 2;
                lpad = (lpad >= 0) ? lpad : 0;
                Console.Write(new string(' ', lpad));
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

        //picks random location for a ship
        public void LaunchShip()
        {
            shipPos.spX = rnd.Next(boardsize);
            shipPos.spY = rnd.Next(boardsize);
        }

        //returns ship location as text and X & Y output variables
        public string GetShipPos(out int xPos, out int yPos)
        {
            xPos = shipPos.spX;
            yPos = shipPos.spY;
            return "( " + shipPos.spX + ", " + shipPos.spY + " )";
        }

        //use this for guessing position
        //calls ShotMiss/ShotHit to update - 
        //the board with hit/miss, also updates sunk/dupe var 
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

        //prints game title
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

        //updates board in case of miss - checks for duplicate guesses
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

        //updates board in case of hit
        private void ShotHit(int shotx, int shoty)
        {
            gameBoard[shotx, shoty] = "X";
        }

    } //end battleship class

    class Program
    {
        static void Main(string[] args)
        {
            //init variables and game object
            bool shipSunk = false;
            int guesses = 1;
            bool goodguess = true;
            int guessX = 0, guessY = 0;
            Battleship myBS = new Battleship();
            string shipLocation = myBS.GetShipPos(out int shipPosX, out int shipPosY);

            //main game loop
            while (!shipSunk)
            {
                Console.Clear();
                //Console.WriteLine(shipLocation);//uncomment to see ship location on screen
                myBS.PrintBoard();

                //check to see if the last guess was a duplicate of previous guesses
                if (myBS.Dupe)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" \nYou already guessed that spot, guess again.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

                do//get X coordinate
                {
                    Console.Write("\nGuess #{0} | Enter X Coordinate: ", guesses);
                    try
                    {
                        guessX = Convert.ToInt32(Console.ReadLine());
                        if (guessX < 0 || guessX > myBS.Boardsize - 1)
                            throw new ArgumentOutOfRangeException();
                        else
                            goodguess = true;
                    }
                    catch //in case something other than an integer was entered
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\a\nValid values are 0 through {0}\n", myBS.Boardsize - 1);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        goodguess = false;
                    }
                } while (!goodguess);

                do//get Y coordinate
                {
                    Console.Write("\nGuess #{0} | Enter Y Coordinate: ", guesses);
                    try
                    {
                        guessY = Convert.ToInt32(Console.ReadLine());
                        if (guessY < 0 || guessY > myBS.Boardsize - 1)
                            throw new ArgumentOutOfRangeException();
                        else
                            goodguess = true;
                    }
                    catch//in case something other than an integer was entered
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\a\nValid values are 0 through {0}\n", myBS.Boardsize - 1);
                        goodguess = false;
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                } while (!goodguess);

                shipSunk = myBS.ShootAtShip(guessX, guessY);

                //increment guess count if guess was not a duplicate and the ship wasn't sunk
                if (!myBS.Dupe && !myBS.Sunk)
                    guesses++;
            }

            //game was won
            Console.Clear();
            //Console.WriteLine(shipLocation);//uncomment to see ship location on screen
            myBS.PrintBoard();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\nCongratulations! ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\nYou sank my BATTLESHIP in {0} guesses!", guesses);
            Console.ReadKey();


        }
    }
}
