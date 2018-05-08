using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace B18_Ex02_1
{
    class UserInterface
    {
        
        internal void GameEntry()
        {
            Console.WriteLine("Welcome to American Checker!");
        }

        internal string ReadPlayerNameFromUser()
        {
            string userName;

            Console.WriteLine("Please enter your name:");
            userName = Console.ReadLine();
            while (!isValidUserName(userName))
            {
                Console.WriteLine("Invalid Name, please enter again");
                userName = Console.ReadLine();
            }

            return userName;
        }

        internal string ReadMoveFromUser(int i_BoardSize, Player i_Player)
        {
            String move = Console.ReadLine();

            while (!isValidMoveFormat(move, i_BoardSize, i_Player))
            {
                if (!isValidMoveFormat(move, i_BoardSize, i_Player))
                {
                    Console.Write("Invalid Move format (COLrow>COLrow). Please try again: ");
                    move = Console.ReadLine();
                }
            }

            return move;
        }

        internal bool EndOFGame(Player i_PlayerOne, Player i_PlayerTwo)
        {
            string message = string.Format(@"{0}'s score: {1} , {2}'s score: {3}",
                i_PlayerOne.Name,
                i_PlayerOne.Score,
                i_PlayerTwo.Name,
                i_PlayerTwo.Score
            );

            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine(message);

            return playAgain();
        }

        internal int ReadBoardSizeFromUser()
        {
            int boardSize;
            boardSize = readLineForBoardSize();
            return boardSize;
        }

        internal bool AgainstComputer()
        {
            bool againstComputer = true;
            string userInput;

            Console.WriteLine("Do you want to play against the computer? y/n");
            userInput = Console.ReadLine();
            while (userInput != "y" && userInput != "n")
            {
                Console.WriteLine("Invalid answer. Please enter 'y' or 'n'");
                userInput = Console.ReadLine();
            }
            if (userInput == "n")
            {
                againstComputer = false;
            }

            return againstComputer;
        }

        private bool playAgain()
        {
            bool playAgain = true;
            string userInput;

            Console.WriteLine("Would you like to play again? y/n");
            userInput = Console.ReadLine();
            while (userInput != "y" && userInput != "n")
            {
                Console.WriteLine("Invalid answer. Please enter 'y' or 'n'");
                userInput = Console.ReadLine();
            }

            if (userInput == "n")
            {
                playAgain = false;
            }

            return playAgain;
        }

        private static bool isValidUserName(string i_Name)
        {
            bool isValid = true;

            if (i_Name.Length > 20)
            {
                isValid = false;
            }
            foreach (char currentChar in i_Name)
            {
                if (currentChar == ' ')
                {
                    isValid = false;
                }
            }

            return isValid;
        }
        
        private static int readLineForBoardSize()
        {
            int boardSize;
            String userInput;
            bool isNumber;

            Console.WriteLine("Please enter the wanted board size (6 or 8 or 10)");
            userInput = Console.ReadLine();
            isNumber = int.TryParse(userInput, out boardSize);
            while (!isNumber || !isValidBoardSIze(boardSize))
            {
                Console.WriteLine("Invalid size. Please select: 6 or 8 or 10");
                userInput = Console.ReadLine();
                isNumber = int.TryParse(userInput, out boardSize);
            }

            return boardSize;
        }

        private static bool isValidBoardSIze(int i_size)
        {
            bool boardValidation = true;

            if ((i_size != 6) && (i_size != 8) && (i_size != 10))
            {
                boardValidation = false;
            }

            return boardValidation;
        }

        private bool isValidMoveFormat(string i_Move, int i_Size, Player i_Player)
        {
            bool moveValidation = true;
            int charLocation = 0;

            if(i_Move.Length != 5)
            {
                moveValidation = false;
            }

            foreach (char currentChar in i_Move)
            {
                if (charLocation == 0 || charLocation == 3)
                {
                    if (currentChar < 'A' || currentChar >= 'A' + i_Size)
                    {
                        moveValidation = false;
                    }
                }
                if (charLocation == 1 || charLocation == 4)
                {
                    if (currentChar < 'a' || currentChar >= 'a' + i_Size)
                    {
                        moveValidation = false;
                    }
                }
                if (charLocation == 2)
                {
                    if (currentChar != '>')
                    {
                        moveValidation = false;
                    }
                }
                charLocation++;
            }

            if (i_Move.Equals("Q") && isPlayerLosing(i_Player))
            {
                moveValidation = true;
                i_Player.LastMove = "Q";
            }

            return moveValidation;
        }

        private bool isPlayerLosing(Player i_Player)
        {
            bool isLosing = true;

            if (i_Player.NumberOfInstrumentsPerRound >= i_Player.Rival.NumberOfInstrumentsPerRound)
            {
                isLosing = false;
            }

            return isLosing;
        }
    }
}
