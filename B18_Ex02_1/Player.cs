using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_1
{
    public enum ePlayerID
    {
        PlayerOne, 
        PlayerTwo,
        Computer
    }

    public enum eOptionalMoves
    {
        MoveDownRight,
        MoveDownLeft,
        MoveUpRight,
        MoveUpLeft,
        EatDownRight,
        EatDownLeft,
        EatUpRight,
        EatUpLeft,
        InvalidMove
    }

    class Player
    {
        private string m_PlayerName;
        private int m_PlayerScore = 0;
        private int m_NumberOfInstrumentsPerRound;
        private ePlayerID m_PlayerID;
        private Board m_Board;
        private Player m_Rival;
        private string m_LastMove;
        private ArtificialIntelligence m_ArtificialIntelligence;
        private MovementValidation m_MovementValidation;

        public Player(string i_Name, ePlayerID i_PlayerID, Board i_Board)
        {
            m_PlayerName = i_Name;
            m_PlayerID = i_PlayerID;
            m_Board = i_Board;
            m_LastMove = "";
            m_MovementValidation = new MovementValidation(this, m_Board);
            if (i_PlayerID == ePlayerID.Computer)
            {
                m_ArtificialIntelligence = new ArtificialIntelligence(this, m_Board);
            }
        }

        public Player Rival
        {
            set { m_Rival = value; }
            get { return m_Rival; }
        }

        public ePlayerID PlayerID
        {
            get { return m_PlayerID; }
        }

        public string LastMove
        {
            get { return m_LastMove; }
            set { m_LastMove = value; }
        }

        public string Name
        {
            get { return m_PlayerName; }
        }

        public int Score
        {
            get { return m_PlayerScore; }
            set { m_PlayerScore = value; }
        }

        public int NumberOfInstrumentsPerRound
        {
            get { return m_NumberOfInstrumentsPerRound; }
            set { m_NumberOfInstrumentsPerRound = value; }
        }

        internal void ResetNumberOfInstrumentsPerRound(int i_BoardSize)
        {
            if (i_BoardSize == 6)
            {
                m_NumberOfInstrumentsPerRound = 6;
            }
            else if (i_BoardSize == 8)
            {
                m_NumberOfInstrumentsPerRound = 12;
            }
            else
            {
                m_NumberOfInstrumentsPerRound = 20;
            }
        }

        internal void Move()
        {
            if (m_PlayerID == ePlayerID.Computer)
            {
                m_ArtificialIntelligence.RegularComputerMove();
            }
            else
            {
                realPlayerMove(false);
            }
        }

        private void realPlayerMove(bool io_IsContinuesMove)
        {
            int colMoveFrom;
            int rowMoveFrom;
            int colMoveTo;
            int rowMoveTo;
            string validPlayerMove = m_MovementValidation.ValidPlayerMove(io_IsContinuesMove);

            if (!validPlayerMove.Equals("Q"))
            {
                m_LastMove = validPlayerMove;
                colMoveFrom = m_MovementValidation.CharToLocationAtBoard(validPlayerMove[0]);
                rowMoveFrom = m_MovementValidation.CharToLocationAtBoard(validPlayerMove[1]);
                colMoveTo = m_MovementValidation.CharToLocationAtBoard(validPlayerMove[3]);
                rowMoveTo = m_MovementValidation.CharToLocationAtBoard(validPlayerMove[4]);
                MakeTheMove(rowMoveFrom, colMoveFrom, rowMoveTo, colMoveTo);
            }
        }

        internal void MakeTheMove(int i_RowFrom, int i_ColFrom, int i_RowTo, int i_ColTo)
        {
            int rowVictim = (i_RowFrom + i_RowTo) / 2;
            int colVictim = (i_ColFrom + i_ColTo) / 2;

            m_Board[i_RowTo, i_ColTo] = m_Board[i_RowFrom, i_ColFrom];
            m_Board[i_RowFrom, i_ColFrom] = eInstrumentType.Space;
            soldierToBeKing(i_RowTo, i_ColTo);
            if (i_RowFrom == i_RowTo + 2 || i_RowTo == i_RowFrom + 2) // eat rival instrument
            {
                updateNumberOfInstrumentsPerRound(rowVictim, colVictim);
                m_Board[rowVictim, colVictim] = eInstrumentType.Space;
                if (m_MovementValidation.CheckIfCanEatWithSpecificIndex(i_RowTo, i_ColTo)) // can eat again with same instrument
                {
                    m_Board.printBoardToConsol(this, this);
                    switch (m_PlayerID)
                    {
                        case ePlayerID.Computer:
                            m_ArtificialIntelligence.ContinuesComputerMove(i_RowTo, i_ColTo);
                            break;
                        default:
                            realPlayerMove(true);
                            break;
                    }
                }
            }
        }

        internal bool CheckForValidMoves()
        {
            bool hasValidMove;
            if (m_PlayerID == ePlayerID.PlayerOne)
            {
                hasValidMove = m_MovementValidation.CheckForValidMovesForPlayerOne();
            }
            else
            {
                hasValidMove = m_MovementValidation.CheckForValidMovesForPlayerTwo();
            }

            return hasValidMove;
        }

        private void updateNumberOfInstrumentsPerRound(int i_RowVictim, int i_ColVictim)
        {
            if (m_PlayerID == ePlayerID.PlayerOne)
            {
                if (m_Board[i_RowVictim, i_ColVictim] == eInstrumentType.PlayerTwoSoldier)
                {
                    m_Rival.NumberOfInstrumentsPerRound -= 1;
                }
                else
                {
                    m_Rival.NumberOfInstrumentsPerRound -= 4;
                }
            }
            else
            {
                if (m_Board[i_RowVictim, i_ColVictim] == eInstrumentType.PlayerOneSoldier)
                {
                    m_Rival.NumberOfInstrumentsPerRound -= 1;
                }
                else
                {
                    m_Rival.NumberOfInstrumentsPerRound -= 4;
                }
            }
        }

        private void soldierToBeKing(int i_Row, int i_Col) // gets the location after the movement !!! 
        {
            if ((i_Row == 0) && (m_Board[i_Row, i_Col] == eInstrumentType.PlayerOneSoldier) )
            {
                m_Board[i_Row, i_Col] = eInstrumentType.PlayerOneKing;
                m_NumberOfInstrumentsPerRound += 3;
            }
            else if ((i_Row == m_Board.Size - 1) && (m_Board[i_Row, i_Col] == eInstrumentType.PlayerTwoSoldier))
            {
                m_Board[i_Row, i_Col] = eInstrumentType.PlayerTwoKing;
                m_NumberOfInstrumentsPerRound += 3;
            }
        }

    }
}
