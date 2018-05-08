using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_1
{
    class MovementValidation
    {
        private Player m_Player;
        private Board m_Board;
        private UserInterface m_UserInterface;
        
        public MovementValidation(Player i_Player, Board i_Board)
        {
            m_Player = i_Player;
            m_Board = i_Board;
            m_UserInterface = new UserInterface();
        }

        internal string ValidPlayerMove(bool i_IsContinuesMove)
        {
            string playerMove = m_UserInterface.ReadMoveFromUser(m_Board.Size, m_Player);
            bool quit = playerMove.Equals("Q");
            bool movePassedValidationTests;

            if (!quit)
            {
                movePassedValidationTests = IsValidMoveAsPlayer(playerMove);
            }
            else
            {
                i_IsContinuesMove = false;
                movePassedValidationTests = true;
            }

            while (!movePassedValidationTests || i_IsContinuesMove)
            {
                if (i_IsContinuesMove && movePassedValidationTests)
                {
                    if (m_Player.LastMove[3] != playerMove[0] || m_Player.LastMove[4] != playerMove[1])
                    {
                        Console.WriteLine("Invalid move, you must continue with the same player");
                    }
                    else
                    {
                        i_IsContinuesMove = false;
                    }
                }

                if (!movePassedValidationTests || i_IsContinuesMove)
                {
                    playerMove = m_UserInterface.ReadMoveFromUser(m_Board.Size, m_Player);
                    quit = playerMove.Equals("Q");
                    if (!quit)
                    {
                        movePassedValidationTests = IsValidMoveAsPlayer(playerMove);
                    }
                    else 
                    {
                        movePassedValidationTests = true;
                        i_IsContinuesMove = false;
                    } 
                }
            }

            return playerMove;
        }

        internal bool IsValidMoveAsPlayer(string i_Move)
        {
            bool isValidMoveResult = true;
            //convert the move into integer
            int colMoveFrom = CharToLocationAtBoard(i_Move[0]);
            int rowMoveFrom = CharToLocationAtBoard(i_Move[1]);
            int colMoveTo = CharToLocationAtBoard(i_Move[3]);
            int rowMoveTo = CharToLocationAtBoard(i_Move[4]);
            //check what is the spcific instrument type in board
            eInstrumentType instrumentTypeFrom = m_Board[rowMoveFrom, colMoveFrom];
            //catalog move type 
            eOptionalMoves requiredMove = ConvertToEnumMove(rowMoveFrom, rowMoveTo, colMoveFrom, colMoveTo);

            if (requiredMove == eOptionalMoves.InvalidMove)
            {
                isValidMoveResult = false;
                Console.Write("Invalid move. Please try again: ");
            }
            else if (!ValidMoveByType(rowMoveFrom, colMoveFrom, requiredMove))
            {
                isValidMoveResult = false;
                Console.Write("Invalid move. Please try again: ");
            }
            else if (!IsCorrectPlayerIsInCell(instrumentTypeFrom))
            {
                isValidMoveResult = false;
                Console.Write("Invalid move, it is not your instrument. Please try again: ");
            }
            else if (!ValidInstrumentDirection(instrumentTypeFrom, requiredMove))
            {
                isValidMoveResult = false;
                Console.Write("Invalid move, a soldier can go only forward. Please try again: ");
            }
            else if (CheckIfCanEat())
            {
                if (requiredMove != eOptionalMoves.EatDownLeft && requiredMove != eOptionalMoves.EatDownRight &&
                    requiredMove != eOptionalMoves.EatUpLeft && requiredMove != eOptionalMoves.EatUpRight)
                {
                    isValidMoveResult = false;
                    Console.Write("Invalid move, you should make a capture. Please try again: ");

                }
            }

            return isValidMoveResult;
        }

        internal eOptionalMoves ConvertToEnumMove(int i_RowFrom, int i_RowTo, int i_ColFrom, int i_ColTo)
        {
            eOptionalMoves requiredMove = eOptionalMoves.InvalidMove;

            if (i_ColFrom == i_ColTo + 2 && i_RowFrom == i_RowTo + 2)
            {
                requiredMove = eOptionalMoves.EatUpLeft;
            }
            else if (i_ColFrom == i_ColTo + 1 && i_RowFrom == i_RowTo + 1)
            {
                requiredMove = eOptionalMoves.MoveUpLeft;
            }
            else if (i_ColFrom == i_ColTo - 2 && i_RowFrom == i_RowTo + 2)
            {
                requiredMove = eOptionalMoves.EatUpRight;
            }
            else if (i_ColFrom == i_ColTo - 1 && i_RowFrom == i_RowTo + 1)
            {
                requiredMove = eOptionalMoves.MoveUpRight;
            }
            else if (i_ColFrom == i_ColTo + 2 && i_RowFrom == i_RowTo - 2)
            {
                requiredMove = eOptionalMoves.EatDownLeft;
            }
            else if (i_ColFrom == i_ColTo + 1 && i_RowFrom == i_RowTo - 1)
            {
                requiredMove = eOptionalMoves.MoveDownLeft;
            }
            else if (i_ColFrom == i_ColTo - 2 && i_RowFrom == i_RowTo - 2)
            {
                requiredMove = eOptionalMoves.EatDownRight;
            }
            else if (i_ColFrom == i_ColTo - 1 && i_RowFrom == i_RowTo - 1)
            {
                requiredMove = eOptionalMoves.MoveDownRight;
            }

            return requiredMove;
        }

        internal bool IsCorrectPlayerIsInCell(eInstrumentType i_InstrumentTypeInCell)
        {
            bool isCorrectPlayerInCell = false;

            if (m_Player.PlayerID == ePlayerID.PlayerOne)
            {
                if (i_InstrumentTypeInCell == eInstrumentType.PlayerOneSoldier ||
                    i_InstrumentTypeInCell == eInstrumentType.PlayerOneKing)
                {
                    isCorrectPlayerInCell = true;
                }
            }
            else
            {
                if (i_InstrumentTypeInCell == eInstrumentType.PlayerTwoSoldier ||
                    i_InstrumentTypeInCell == eInstrumentType.PlayerTwoKing)
                {
                    isCorrectPlayerInCell = true;
                }
            }

            return isCorrectPlayerInCell;
        }

        internal bool ValidInstrumentDirection(eInstrumentType instrumentTypeFrom, eOptionalMoves requiredMove)
        {
            bool isValidMove = true;

            //soldier1 can move only up
            if (instrumentTypeFrom == eInstrumentType.PlayerOneSoldier)
            {
                if (requiredMove != eOptionalMoves.EatUpLeft && requiredMove != eOptionalMoves.EatUpRight &&
                    requiredMove != eOptionalMoves.MoveUpLeft && requiredMove != eOptionalMoves.MoveUpRight)
                {
                    isValidMove = false;
                }
            }
            //soldier2 can move only down
            else if (instrumentTypeFrom == eInstrumentType.PlayerTwoSoldier)
            {
                if (requiredMove != eOptionalMoves.EatDownLeft && requiredMove != eOptionalMoves.EatDownRight &&
                    requiredMove != eOptionalMoves.MoveDownLeft && requiredMove != eOptionalMoves.MoveDownRight)
                {
                    isValidMove = false;
                }
            }

            return isValidMove;
        }

        internal bool CheckForValidMovesForPlayerOne()
        {
            bool hasValidMove = false;

            for (int i = 0; (i < m_Board.Size) && (!hasValidMove); i++)
            {
                for (int j = 0; (j < m_Board.Size) && (!hasValidMove); j++)
                {
                    hasValidMove = ChekForValidMovesForPlayerOneAtIndexes(i, j);
                }
            }

            return hasValidMove;
        }

        internal bool ChekForValidMovesForPlayerOneAtIndexes(int i_Row, int i_Col)
        {
            bool hasValidMove = false;

            switch (m_Board[i_Row, i_Col])
            {
                case (eInstrumentType.PlayerOneSoldier):
                    hasValidMove = MoveUpRight(i_Row, i_Col) || MoveUpLeft(i_Row, i_Col) ||
                                   EatUpRight(i_Row, i_Col) || EatUpLeft(i_Row, i_Col);
                    break;
                case (eInstrumentType.PlayerOneKing):
                    hasValidMove = MoveDownRight(i_Row, i_Col) || MoveDownLeft(i_Row, i_Col) ||
                                   EatDownRight(i_Row, i_Col) || EatDownLeft(i_Row, i_Col) || MoveUpRight(i_Row, i_Col) ||
                                   MoveUpLeft(i_Row, i_Col) || EatUpRight(i_Row, i_Col) || EatUpLeft(i_Row, i_Col);
                    break;
                default:
                    break;
            }

            return hasValidMove;
        }

        internal bool ChekForValidEatMovesForPlayerOneAtIndexes(int i_Row, int i_Col)
        {
            bool hasValidMove = false;

            switch (m_Board[i_Row, i_Col])
            {
                case (eInstrumentType.PlayerOneSoldier):
                    hasValidMove = EatUpRight(i_Row, i_Col) || EatUpLeft(i_Row, i_Col);
                    break;
                case (eInstrumentType.PlayerOneKing):
                    hasValidMove = EatDownRight(i_Row, i_Col) || EatDownLeft(i_Row, i_Col) ||
                                   EatUpRight(i_Row, i_Col) || EatUpLeft(i_Row, i_Col);
                    break;
                default:
                    break;
            }

            return hasValidMove;
        }

        internal bool CheckForValidMovesForPlayerTwo()
        {
            bool hasValidMove = false;

            for (int i = 0; i < (m_Board.Size) && (!hasValidMove); i++)
            {
                for (int j = 0; (j < m_Board.Size) && (!hasValidMove); j++)
                {
                    hasValidMove = CheckForValidMovesForPlayerTwoAtIndexes(i, j);
                }
            }

            return hasValidMove;
        }

        internal bool CheckForValidMovesForPlayerTwoAtIndexes(int i_Row, int i_Col)
        {
            bool hasValidMove = false;

            switch (m_Board[i_Row, i_Col])
            {
                case (eInstrumentType.PlayerTwoSoldier):
                    hasValidMove = MoveDownRight(i_Row, i_Col) || MoveDownLeft(i_Row, i_Col) ||
                                   EatDownRight(i_Row, i_Col) || EatDownLeft(i_Row, i_Col);
                    break;
                case (eInstrumentType.PlayerTwoKing):
                    hasValidMove = MoveDownRight(i_Row, i_Col) || MoveDownLeft(i_Row, i_Col) ||
                                   EatDownRight(i_Row, i_Col) || EatDownLeft(i_Row, i_Col) || MoveUpRight(i_Row, i_Col) ||
                                   MoveUpLeft(i_Row, i_Col) || EatUpRight(i_Row, i_Col) || EatUpLeft(i_Row, i_Col);
                    break;
                default:
                    break;
            }

            return hasValidMove;
        }

        internal bool ChekForValidEatMovesForPlayerTwoAtIndexes(int i_Row, int i_Col)
        {
            bool hasValidMove = false;

            switch (m_Board[i_Row, i_Col])
            {
                case (eInstrumentType.PlayerTwoSoldier):
                    hasValidMove = EatDownRight(i_Row, i_Col) || EatDownLeft(i_Row, i_Col);
                    break;
                case (eInstrumentType.PlayerTwoKing):
                    hasValidMove =
                                   EatDownRight(i_Row, i_Col) || EatDownLeft(i_Row, i_Col) ||
                                    EatUpRight(i_Row, i_Col) || EatUpLeft(i_Row, i_Col);
                    break;
                default:
                    break;
            }

            return hasValidMove;
        }

        internal bool CheckIfCanEat()
        {
            bool canEat = false;

            for (int i = 0; i < m_Board.Size; i++)
            {
                for (int j = 0; j < m_Board.Size; j++)
                {
                    if (m_Player.PlayerID == ePlayerID.PlayerOne)
                    {
                        canEat = canEat || ChekForValidEatMovesForPlayerOneAtIndexes(i, j);
                    }
                    else if (m_Player.PlayerID == ePlayerID.PlayerTwo)
                    {
                        canEat = canEat || ChekForValidEatMovesForPlayerTwoAtIndexes(i, j);
                    }

                }
            }

            return canEat;
        }

        internal bool CheckIfCanEatWithSpecificIndex(int i_Row, int i_Col)
        {
            bool canEat = false;

            if (m_Player.PlayerID == ePlayerID.PlayerOne)
            {
                canEat = ChekForValidEatMovesForPlayerOneAtIndexes(i_Row, i_Col);
            }
            else
            {
                canEat = ChekForValidEatMovesForPlayerTwoAtIndexes(i_Row, i_Col);
            }

            return canEat;
        }

        internal bool ValidMoveByType(int i_Row, int i_Col, eOptionalMoves io_MoveType)
        {
            bool validByMoveType;

            switch (io_MoveType)
            {
                case (eOptionalMoves.MoveDownRight):
                    validByMoveType = MoveDownRight(i_Row, i_Col);
                    break;
                case (eOptionalMoves.MoveDownLeft):
                    validByMoveType = MoveDownLeft(i_Row, i_Col);
                    break;
                case (eOptionalMoves.MoveUpRight):
                    validByMoveType = MoveUpRight(i_Row, i_Col);
                    break;
                case (eOptionalMoves.MoveUpLeft):
                    validByMoveType = MoveUpLeft(i_Row, i_Col);
                    break;
                case (eOptionalMoves.EatDownRight):
                    validByMoveType = EatDownRight(i_Row, i_Col);
                    break;
                case (eOptionalMoves.EatDownLeft):
                    validByMoveType = EatDownLeft(i_Row, i_Col);
                    break;
                case (eOptionalMoves.EatUpRight):
                    validByMoveType = EatUpRight(i_Row, i_Col);
                    break;
                case (eOptionalMoves.EatUpLeft):
                    validByMoveType = EatUpLeft(i_Row, i_Col);
                    break;
                default:
                    validByMoveType = false;
                    break;
            }

            return validByMoveType;
        }

        internal bool MoveDownRight(int i_Row, int i_Col)
        {
            bool validMove = true;

            if ((i_Row + 1) >= m_Board.Size || (i_Col + 1) >= m_Board.Size)
            {
                validMove = false;
            }
            else if (m_Board[i_Row + 1, i_Col + 1] != eInstrumentType.Space)
            {
                validMove = false;
            }

            return validMove;
        }

        internal bool MoveDownLeft(int i_Row, int i_Col)
        {
            bool validMove = true;

            if ((i_Row + 1) >= m_Board.Size || (i_Col - 1) < 0)
            {
                validMove = false;
            }
            else if (m_Board[i_Row + 1, i_Col - 1] != eInstrumentType.Space)
            {
                validMove = false;
            }

            return validMove;
        }

        internal bool EatDownRight(int i_Row, int i_Col)
        {
            bool validMove = true;
            int victimRow = i_Row + 1;
            int victimCol = i_Col + 1;

            if ((i_Row + 2) >= m_Board.Size || (i_Col + 2) >= m_Board.Size)
            {
                validMove = false;
            }
            else if (m_Board[i_Row + 2, i_Col + 2] != eInstrumentType.Space)
            {
                validMove = false;
            }
            else if (m_Player.PlayerID == ePlayerID.PlayerOne)
            {
                if (m_Board[victimRow, victimCol] != eInstrumentType.PlayerTwoKing &&
                   m_Board[victimRow, victimCol] != eInstrumentType.PlayerTwoSoldier)
                {
                    validMove = false;
                }
            }
            else // computer or player two
            {
                if (m_Board[victimRow, victimCol] != eInstrumentType.PlayerOneKing &&
                   m_Board[victimRow, victimCol] != eInstrumentType.PlayerOneSoldier)
                {
                    validMove = false;
                }
            }

            return validMove;
        }

        internal bool EatDownLeft(int i_Row, int i_Col)
        {
            bool validMove = true;
            int victimRow = i_Row + 1;
            int victimCol = i_Col - 1;

            if ((i_Row + 2) >= m_Board.Size || (i_Col - 2) < 0)
            {
                validMove = false;
            }
            else if (m_Board[i_Row + 2, i_Col - 2] != eInstrumentType.Space)
            {
                validMove = false;
            }
            else if (m_Player.PlayerID == ePlayerID.PlayerOne)
            {
                if (m_Board[victimRow, victimCol] != eInstrumentType.PlayerTwoKing &&
                   m_Board[victimRow, victimCol] != eInstrumentType.PlayerTwoSoldier)
                {
                    validMove = false;
                }
            }
            else // computer or player two
            {
                if (m_Board[victimRow, victimCol] != eInstrumentType.PlayerOneKing &&
                   m_Board[victimRow, victimCol] != eInstrumentType.PlayerOneSoldier)
                {
                    validMove = false;
                }
            }

            return validMove;
        }

        internal bool MoveUpRight(int i_Row, int i_Col)
        {
            bool validMove = true;

            if ((i_Row - 1) < 0 || (i_Col + 1) >= m_Board.Size)
            {
                validMove = false;
            }

            else if (m_Board[i_Row - 1, i_Col + 1] != eInstrumentType.Space)
            {
                validMove = false;
            }

            return validMove;
        }

        internal bool MoveUpLeft(int i_Row, int i_Col)
        {
            bool validMove = true;

            if ((i_Row - 1) < 0 || (i_Col - 1) < 0)
            {
                validMove = false;
            }

            else if (m_Board[i_Row - 1, i_Col - 1] != eInstrumentType.Space)
            {
                validMove = false;
            }

            return validMove;
        }

        internal bool EatUpRight(int i_Row, int i_Col)
        {
            bool validMove = true;
            int victimRow = i_Row - 1;
            int victimCol = i_Col + 1;

            if ((i_Row - 2) < 0 || (i_Col + 2) >= m_Board.Size)
            {
                validMove = false;
            }
            else if (m_Board[i_Row - 2, i_Col + 2] != eInstrumentType.Space)
            {
                validMove = false;
            }
            else if (m_Player.PlayerID == ePlayerID.PlayerOne)
            {
                if (m_Board[victimRow, victimCol] != eInstrumentType.PlayerTwoKing &&
                   m_Board[victimRow, victimCol] != eInstrumentType.PlayerTwoSoldier)
                {
                    validMove = false;
                }
            }
            else // computer or player two
            {
                if (m_Board[victimRow, victimCol] != eInstrumentType.PlayerOneKing &&
                   m_Board[victimRow, victimCol] != eInstrumentType.PlayerOneSoldier)
                {
                    validMove = false;
                }
            }

            return validMove;
        }

        internal bool EatUpLeft(int i_Row, int i_Col)
        {
            bool validMove = true;
            int victimRow = i_Row - 1;
            int victimCol = i_Col - 1;

            if ((i_Row - 2) < 0 || (i_Col - 2) < 0)
            {
                validMove = false;
            }
            else if (m_Board[i_Row - 2, i_Col - 2] != eInstrumentType.Space)
            {
                validMove = false;
            }
            else if (m_Player.PlayerID == ePlayerID.PlayerOne)
            {
                if (m_Board[victimRow, victimCol] != eInstrumentType.PlayerTwoKing &&
                   m_Board[victimRow, victimCol] != eInstrumentType.PlayerTwoSoldier)
                {
                    validMove = false;
                }
            }
            else // computer or player two
            {
                if (m_Board[victimRow, victimCol] != eInstrumentType.PlayerOneKing &&
                   m_Board[victimRow, victimCol] != eInstrumentType.PlayerOneSoldier)
                {
                    validMove = false;
                }
            }

            return validMove;
        }

        internal int CharToLocationAtBoard(char i_Char)
        {
            int numericValue;
            if (i_Char >= 'a')
            {
                numericValue = i_Char - 97;
            }
            else
            {
                numericValue = i_Char - 65;
            }

            return numericValue;
        }
    }
}
