using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_1
{
    class ArtificialIntelligence
    {
        private class ComputerValidOptionNode
        {
            private int m_RowIndex;
            private int m_ColIndex;
            private eOptionalMoves m_Direction;

            public ComputerValidOptionNode(int i_Row, int i_Col, eOptionalMoves i_Direction)
            {
                m_RowIndex = i_Row;
                m_ColIndex = i_Col;
                m_Direction = i_Direction;
            }

            public int RowIndex
            {
                get { return m_RowIndex; }
                set { m_RowIndex = value; }
            }

            public int ColIndex
            {
                get { return m_ColIndex; }
                set { m_ColIndex = value; }
            }

            public eOptionalMoves Direction
            {
                get { return m_Direction; }
                set { m_Direction = value; }
            }
        }

        private Player m_AIPlayer;
        private Board m_Board;
        private MovementValidation m_MovementValidation;

        public ArtificialIntelligence(Player i_Player,Board i_Board)
        {
            m_AIPlayer = i_Player;
            m_Board = i_Board;
            m_MovementValidation = new MovementValidation(m_AIPlayer, m_Board);
        }

        internal void RegularComputerMove()
        {
            int[] soldierToBeKingIndex = new int[2];
            int[] maxEatGainIndex = new int[2];
            int maxGain = 0;
            int currentGain;
            eOptionalMoves maxGainDirection = eOptionalMoves.InvalidMove;
            eOptionalMoves kingDirection = eOptionalMoves.InvalidMove;

            for (int i = 0 ; i < m_Board.Size; i++)
            {
                for (int j = 0 ; j < m_Board.Size; j++)
                {
                    if (m_MovementValidation.IsCorrectPlayerIsInCell(m_Board[i, j]))
                    {
                        foreach (eOptionalMoves direction in Enum.GetValues(typeof(eOptionalMoves)))
                        {
                            if (m_MovementValidation.ValidMoveByType(i, j, direction) && m_MovementValidation.ValidInstrumentDirection(m_Board[i, j], direction))
                            {
                                currentGain = computerEatCounterByMovement(i, j, 0, direction);
                                if (currentGain > maxGain)
                                {
                                    maxGain = currentGain;
                                    maxEatGainIndex[0] = i;
                                    maxEatGainIndex[1] = j;
                                    maxGainDirection = direction;
                                }

                                if (computerMoveToBeKingByDirection(i, j, direction))
                                {
                                    soldierToBeKingIndex[0] = i;
                                    soldierToBeKingIndex[1] = j;
                                    kingDirection = direction;
                                }
                            }
                        }
                    }
                }
            }

            if (maxGainDirection != eOptionalMoves.InvalidMove && maxGain >= 1)
            {
                computerMakeMove(maxEatGainIndex[0], maxEatGainIndex[1], maxGainDirection);
            }
            else if (kingDirection != eOptionalMoves.InvalidMove)
            {
                computerMakeMove(soldierToBeKingIndex[0], soldierToBeKingIndex[1], kingDirection);
            }
            else
            {
                computerRandomMove();
            }
        }

        internal void ContinuesComputerMove(int i_Row, int i_Col)
        {
            eOptionalMoves[] eatDirectionOptions =
            {
                eOptionalMoves.EatDownLeft, eOptionalMoves.EatDownRight, eOptionalMoves.EatUpLeft,
                eOptionalMoves.EatUpRight
            };

            eOptionalMoves direction = eOptionalMoves.InvalidMove;

            foreach (eOptionalMoves currentDirection in eatDirectionOptions)
            {
                if (m_MovementValidation.ValidMoveByType(i_Row, i_Col, currentDirection) && m_MovementValidation.ValidInstrumentDirection(m_Board[i_Row, i_Col], currentDirection))
                {
                    direction = currentDirection;
                }
            }

            computerMakeMove(i_Row, i_Col, direction);
        }

        private void computerRandomMove()
        {
            Random random = new Random();
            LinkedList<ComputerValidOptionNode> validOptionList = computerRandomMoveOptionList();
            LinkedListNode<ComputerValidOptionNode> listIterator = validOptionList.First;
            ComputerValidOptionNode chosenNode = null;
            int index = random.Next(0, validOptionList.Count);

            while (chosenNode == null)
            {
                listIterator = validOptionList.First;
                for (int i = 0; i < index; i++)
                {
                    listIterator = listIterator.Next;
                }

                index = random.Next(0, validOptionList.Count);
                chosenNode = listIterator.Value;
            }
            
            chosenNode = listIterator.Value;
            computerMakeMove(chosenNode.RowIndex, chosenNode.ColIndex, chosenNode.Direction);
        }

        private void computerMakeMove(int i_Row, int i_Col, eOptionalMoves i_Direction)
        {
            m_AIPlayer.LastMove = computerMoveToString(i_Row, i_Col, i_Direction);
            switch (i_Direction)
            {
                case eOptionalMoves.MoveDownRight:
                    m_AIPlayer.MakeTheMove(i_Row, i_Col, i_Row + 1, i_Col + 1);
                    break;
                case eOptionalMoves.MoveDownLeft:
                    m_AIPlayer.MakeTheMove(i_Row, i_Col, i_Row + 1, i_Col - 1);
                    break;
                case eOptionalMoves.EatDownRight:
                    m_AIPlayer.MakeTheMove(i_Row, i_Col, i_Row + 2, i_Col + 2);
                    break;
                case eOptionalMoves.EatDownLeft:
                    m_AIPlayer.MakeTheMove(i_Row, i_Col, i_Row + 2, i_Col - 2);
                    break;
                case eOptionalMoves.MoveUpRight:
                    m_AIPlayer.MakeTheMove(i_Row, i_Col, i_Row - 1, i_Col + 1);
                    break;
                case eOptionalMoves.MoveUpLeft:
                    m_AIPlayer.MakeTheMove(i_Row, i_Col, i_Row - 1, i_Col - 1);
                    break;
                case eOptionalMoves.EatUpRight:
                    m_AIPlayer.MakeTheMove(i_Row, i_Col, i_Row - 2, i_Col + 2);
                    break;
                case eOptionalMoves.EatUpLeft:
                    m_AIPlayer.MakeTheMove(i_Row, i_Col, i_Row - 2, i_Col - 2);
                    break;
            }
        }

        private LinkedList<ComputerValidOptionNode> computerRandomMoveOptionList()
        {
            eOptionalMoves direction = eOptionalMoves.InvalidMove;
            LinkedList<ComputerValidOptionNode> validOptionList = new LinkedList<ComputerValidOptionNode>();
            ComputerValidOptionNode currentNode;

            for (int i = 0; i < m_Board.Size; i++)
            {
                for (int j = 0; j < m_Board.Size; j++)
                {
                    foreach (eOptionalMoves currentDirection in Enum.GetValues(typeof(eOptionalMoves)))
                    {
                        if (m_MovementValidation.IsCorrectPlayerIsInCell(m_Board[i, j]))
                        {
                            if (m_MovementValidation.ValidMoveByType(i, j, currentDirection) && m_MovementValidation.ValidInstrumentDirection(m_Board[i, j], currentDirection))
                            {
                                currentNode = new ComputerValidOptionNode(i, j, currentDirection);
                                validOptionList.AddFirst(currentNode);
                            }
                        }
                    }
                }
            }

            return validOptionList;
        }

        private int computerEatCounterByMovement(int i_Row, int i_Col, int i_EatCounter, eOptionalMoves io_MoveType)
        {
            int answer;
            if (!m_MovementValidation.ValidMoveByType(i_Row, i_Col, io_MoveType))
            {
                answer = i_EatCounter;
            }
            else
            {
                switch (io_MoveType)
                {
                    case (eOptionalMoves.EatDownRight):
                        answer = computerEatCounterByMovement(i_Row + 2, i_Col + 2, i_EatCounter + 1, io_MoveType);
                        break;
                    case (eOptionalMoves.EatDownLeft):
                        answer = computerEatCounterByMovement(i_Row + 2, i_Col - 2, i_EatCounter + 1, io_MoveType);
                        break;
                    case (eOptionalMoves.EatUpRight):
                        answer = computerEatCounterByMovement(i_Row - 2, i_Col + 2, i_EatCounter + 1, io_MoveType);
                        break;
                    case (eOptionalMoves.EatUpLeft):
                        answer = computerEatCounterByMovement(i_Row - 2, i_Col - 2, i_EatCounter + 1, io_MoveType);
                        break;
                    default:
                        answer = i_EatCounter;
                        break;
                }
            }

            return answer;
        }

        private bool computerMoveToBeKingByDirection(int i_Row, int i_Col, eOptionalMoves io_MoveDirection)
        {
            bool isToBeKing = false;

            if (io_MoveDirection != eOptionalMoves.MoveDownLeft || io_MoveDirection != eOptionalMoves.MoveDownRight)
            {
                isToBeKing = false;
            }

            else if ((i_Row == m_Board.Size - 2) && (m_Board[i_Row, i_Col] == eInstrumentType.PlayerTwoSoldier))
            {
                if (m_MovementValidation.ValidInstrumentDirection(eInstrumentType.PlayerTwoSoldier, io_MoveDirection))
                {
                    isToBeKing = true;
                }
            }

            return isToBeKing;
        }

        private string computerMoveToString(int i_Row, int i_Col, eOptionalMoves io_MoveDirection)
        {
            string computerMove = null;
            char rowFrom = (char)(i_Row + 97);
            char colFrom = (char)(i_Col + 65);
            char colChaerDest;
            char rowCharDest;
            int rowTo = 0;
            int colTo = 0;

            switch (io_MoveDirection)
            {
                case (eOptionalMoves.MoveDownRight):
                    rowTo = rowFrom + 1;
                    colTo = colFrom + 1;
                    break;
                case (eOptionalMoves.MoveDownLeft):
                    rowTo = rowFrom + 1;
                    colTo = colFrom - 1;
                    break;
                case (eOptionalMoves.MoveUpRight):
                    rowTo = rowFrom - 1;
                    colTo = colFrom + 1;
                    break;
                case (eOptionalMoves.MoveUpLeft):
                    rowTo = rowFrom - 1;
                    colTo = colFrom - 1;
                    break;
                case (eOptionalMoves.EatDownRight):
                    rowTo = rowFrom + 2;
                    colTo = colFrom + 2;
                    break;
                case (eOptionalMoves.EatDownLeft):
                    rowTo = rowFrom + 2;
                    colTo = colFrom - 2;
                    break;
                case (eOptionalMoves.EatUpRight):
                    rowTo = rowFrom - 2;
                    colTo = colFrom + 2;
                    break;
                case (eOptionalMoves.EatUpLeft):
                    rowTo = rowFrom - 2;
                    colTo = colFrom - 2;
                    break;
                default:
                    computerMove = "Invalid computer move";
                    break;
            }

            if (computerMove == null)
            {
                rowCharDest = (char)(rowTo);
                colChaerDest = (char)(colTo);
                computerMove = string.Format("{0}{1}>{2}{3}",
                    colFrom,
                    rowFrom,
                    colChaerDest,
                    rowCharDest
                    );
            }

            return computerMove;
        }
    }
}
