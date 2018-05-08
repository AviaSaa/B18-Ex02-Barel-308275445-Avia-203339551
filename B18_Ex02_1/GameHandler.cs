using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_1
{
    class GameHandler
    {
        private Board m_Board;
        private UserInterface m_UserInterface;
        private Player m_PlayerOne; // can be  person only
        private Player m_PlayerTwo; // can be computer or a person
        private int m_BoardSize;

        public GameHandler()
        {
            m_Board = new Board();
            m_UserInterface = new UserInterface();
            m_UserInterface.GameEntry();
            string playerOneName = m_UserInterface.ReadPlayerNameFromUser();

            m_BoardSize = m_UserInterface.ReadBoardSizeFromUser();
            m_Board.CreateBoard(m_BoardSize);
            m_PlayerOne = new Player(playerOneName, ePlayerID.PlayerOne, m_Board);
            if (m_UserInterface.AgainstComputer())
            {
                m_PlayerTwo = new Player("Computer", ePlayerID.Computer, m_Board);
            }
            else
            {
                m_PlayerTwo = new Player(m_UserInterface.ReadPlayerNameFromUser(), ePlayerID.PlayerTwo, m_Board);
            }

            m_PlayerOne.Rival = m_PlayerTwo;
            m_PlayerTwo.Rival = m_PlayerOne;
/*            m_PlayerOne.BoardSize = m_BoardSize;
            m_PlayerTwo.BoardSize = m_BoardSize;*/
            StartGame();
        }

        internal void StartGame()
        {
            bool gameContinue = true;
            bool endOfRound = false;

            
            m_Board.printBoardToConsol();

            while (gameContinue)
            {
                setNewRound();
                endOfRound = false;
                while (!endOfRound)
                {
                    //player one turn
                    m_PlayerOne.Move();
                    m_Board.printBoardToConsol(m_PlayerOne,m_PlayerTwo);

                    // check for valid moves 
                    if (!checkEndOfRound())
                    {
                        // player 2 turn
                        m_PlayerTwo.Move();
                        m_Board.printBoardToConsol(m_PlayerTwo, m_PlayerOne);
                        endOfRound = checkEndOfRound();
                    }
                    else
                    {
                        endOfRound = true;
                    }
                }

                //End of Round screen
                if (!m_UserInterface.EndOFGame(m_PlayerOne, m_PlayerTwo))
                {
                    gameContinue = false;
                }
            }
        }

        private bool playerWantToQuit(Player i_Player)
        {
            bool wantToQuit = i_Player.LastMove.Equals("Q");

            return wantToQuit;
        }

        private void setNewRound()
        {
            m_Board.SetPlayersOnBoard();
            m_Board.printBoardToConsol();
            m_PlayerOne.ResetNumberOfInstrumentsPerRound(m_BoardSize);
            m_PlayerTwo.ResetNumberOfInstrumentsPerRound(m_BoardSize);
            m_PlayerOne.LastMove = "";
            m_PlayerTwo.LastMove = "";
            Console.Write(m_PlayerOne.Name + "'s turn: ");
        }

        private void updateScores(Player i_WinnerOfRound)
        {
            if (i_WinnerOfRound == m_PlayerOne)
            {
                m_PlayerOne.Score += m_PlayerOne.NumberOfInstrumentsPerRound - m_PlayerTwo.NumberOfInstrumentsPerRound;
            }
            else
            {
                m_PlayerTwo.Score += m_PlayerTwo.NumberOfInstrumentsPerRound - m_PlayerOne.NumberOfInstrumentsPerRound;
            }
        }

        private bool checkEndOfRound()
        {
            bool endRound = false;

            if (!m_PlayerOne.CheckForValidMoves() && !m_PlayerTwo.CheckForValidMoves())
            {
                endRound = true;
            }
            else if (m_PlayerOne.NumberOfInstrumentsPerRound <= 0 || !m_PlayerOne.CheckForValidMoves() || playerWantToQuit(m_PlayerOne))
            {
                endRound = true;
                updateScores(m_PlayerTwo);
            }
            else if (m_PlayerTwo.NumberOfInstrumentsPerRound <= 0 || !m_PlayerTwo.CheckForValidMoves() || playerWantToQuit(m_PlayerTwo))
            {
                endRound = true;
                updateScores(m_PlayerOne);
            }

            return endRound;
        }
    }
}
