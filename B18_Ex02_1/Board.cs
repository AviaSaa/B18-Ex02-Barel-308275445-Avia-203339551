using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_1
{
    public enum eInstrumentType
    {
        Space,
        PlayerOneSoldier,
        PlayerOneKing,
        PlayerTwoSoldier,
        PlayerTwoKing
    }

    class Board
    {
        private int m_Size;
        private eInstrumentType[,] m_BoardMatrix;

        internal void CreateBoard(int i_BoardSize)
        {
            m_Size = i_BoardSize;
            m_BoardMatrix = new eInstrumentType[m_Size, m_Size];
        }

        public int Size
        {
            get { return m_Size; }
            set { m_Size = value; }
        }

        public eInstrumentType this[int i_Row, int i_Col]
        {
            get { return m_BoardMatrix[i_Row, i_Col]; }
            set { m_BoardMatrix[i_Row, i_Col] = value; }
        }

        public void SetPlayersOnBoard()
        {
            //upper part (O) = Player Two
            for (int i = 0; i < (m_Size / 2) - 1; i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    if (isEven(i) && isEven(j))
                    {
                        m_BoardMatrix[i, j ] = eInstrumentType.Space;
                    }
                    else if(isEven(i) && !isEven(j))
                    {
                        m_BoardMatrix[i, j] = eInstrumentType.PlayerTwoSoldier;
                    }
                    else if (!isEven(i) && isEven(j))
                    {
                        m_BoardMatrix[i, j] = eInstrumentType.PlayerTwoSoldier;
                    }
                    else
                    {
                        m_BoardMatrix[i, j] = eInstrumentType.Space;
                    }
                }
            }

            //two rows spaces
            for (int i = (m_Size / 2) - 1; i < ((m_Size / 2) + 1); i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    m_BoardMatrix[i, j] = eInstrumentType.Space;
                }
            }

            //buttom part (X) = Player One
            for (int i = ((m_Size / 2) + 1) ; i < m_Size ; i++)
            {
                for (int j = 0; j < m_Size ; j++)
                {
                    if (isEven(i) && isEven(j))
                    {
                        m_BoardMatrix[i, j] = eInstrumentType.Space;
                    }
                    else if (isEven(i) && !isEven(j))
                    {
                        m_BoardMatrix[i, j] = eInstrumentType.PlayerOneSoldier;
                    }
                    else if (!isEven(i) && isEven(j))
                    {
                        m_BoardMatrix[i, j] = eInstrumentType.PlayerOneSoldier;
                    }
                    else
                    {
                        m_BoardMatrix[i, j] = eInstrumentType.Space;
                    }
                }
            }
        }

        private static bool isEven(int i_number)
        {
            bool isEvenNumber = true;

            if (i_number % 2 != 0)
            {
                isEvenNumber = false;
            }

            return isEvenNumber;
        }

        public void printBoardToConsol()
        {
            char rowsSign = 'a';
            char colsSign = 'A';
          
            Ex02.ConsoleUtils.Screen.Clear();

            //print first row A B C ...
            for (int i = 0; i < m_Size; i++)
            {
                Console.Write("   " + colsSign);
                colsSign++;
            }
            Console.Write(Environment.NewLine);
            Console.Write(" "); //for indentetion
            for (int k = 0; k < (m_Size * 4) + 1; k++)
            {
                Console.Write("=");
            }
            Console.Write(Environment.NewLine);


            //print the rest of the matrix
            for (int i = 0; i < m_Size; i++)
            {
                Console.Write("" + rowsSign + "|");
                for (int j = 0; j < m_Size; j++)
                {
                    switch (m_BoardMatrix[i,j])
                    {
                        case (eInstrumentType.PlayerOneSoldier):
                            Console.Write(" X |");
                            break;
                        case (eInstrumentType.PlayerOneKing):
                            Console.Write(" K |");
                            break;
                        case (eInstrumentType.PlayerTwoSoldier):
                            Console.Write(" O |");
                            break;
                        case (eInstrumentType.PlayerTwoKing):
                            Console.Write(" U |");
                            break;
                        case (eInstrumentType.Space):
                            Console.Write("   |");
                            break;
                        default:
                            Console.Write("   |");
                            break;
                    }
                }
                Console.Write(Environment.NewLine);
                Console.Write(" "); //for indentetion
                rowsSign++;
                for (int k = 0; k < (m_Size * 4) + 1; k++)
                {
                    Console.Write("=");
                }
                Console.Write(Environment.NewLine);
            }
        }

        public void printBoardToConsol(Player i_LastPlayerTurn, Player i_NextPlayerTurn)
        {
            string message;
            printBoardToConsol();
            message = string.Format(
                @"{0}'s move was ({1}): {2}
{3}'s Turn ({4}): ",
                i_LastPlayerTurn.Name,
                (i_LastPlayerTurn.PlayerID == ePlayerID.PlayerOne) ? "X" : "O",
                i_LastPlayerTurn.LastMove,
                i_NextPlayerTurn.Name,
                (i_NextPlayerTurn.PlayerID == ePlayerID.PlayerOne) ? "X" : "O"
            );
            Console.Write(message);
        }

    }
}
