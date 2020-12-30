using System.Collections.Generic;
using System;

namespace B20_Ex02
{
    public class AIPlayer
    {
        readonly Player m_Player = new Player("ComputerPlayer");
        public Square[] m_ExposedMemory;
        public Square[] m_SaveNextTurn;
        public bool m_RandNextRound = true;

        public Player GetCompPlayer
        {
            get
            {
                return m_Player;
            }
        }

        public AIPlayer(int i_BoardSize)
        {
            m_ExposedMemory = new Square[(i_BoardSize / 2) + 1];
            m_SaveNextTurn = new Square[2];
        }

        private Square randomlyChooseSquare(int i_BoardLen, int i_BoardWidth)
        {
            System.Random rand = new System.Random();
            int row;
            int col;
            do
            {
                row = rand.Next(i_BoardLen);
                col = rand.Next(i_BoardWidth);
            } 
            while (GameBoard.s_SquareStatusBoard[row, col] == true);

            return new Square(row, col);
        }

        public void SetNextComputerTurn(int o_BoardLen, int o_BoardWidth)
        {
            if (m_RandNextRound == true)
            {
                m_SaveNextTurn[0] = randomlyChooseSquare(o_BoardLen, o_BoardWidth);
                do
                {
                    m_SaveNextTurn[1] = randomlyChooseSquare(o_BoardLen, o_BoardWidth);
                }
                while (m_SaveNextTurn[0] == m_SaveNextTurn[1]);
            }
        }

        public void SavingRevealedCardToMemory(int i_SquareValue, Square i_Choise)
        {
            if (m_ExposedMemory[i_SquareValue].GetVisitedBefore == false)
            { 
                m_ExposedMemory[i_SquareValue] = i_Choise;
                m_ExposedMemory[i_SquareValue].GetVisitedBefore = true;
            }
            else if (m_ExposedMemory[i_SquareValue] != i_Choise)
            {
                m_SaveNextTurn[0] = i_Choise;
                m_SaveNextTurn[1] = m_ExposedMemory[i_SquareValue];
                m_RandNextRound = false;
            }
            else
            { 
                m_RandNextRound = true;
            }
        }
    }
}
