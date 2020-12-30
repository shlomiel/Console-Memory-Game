namespace B20_Ex02
{
    public class GameBoard
    {
        private int[,] m_TheBoardMatrix;
        public static bool[,] s_SquareStatusBoard; // True is exposed , false is unexposed
        private int m_BoardSize;
        private int m_BoardLen;
        private int m_BoardWidth;

        public GameBoard(int i_boardLength, int i_boardWidth)
        {
            m_TheBoardMatrix = new int[i_boardLength, i_boardWidth];
            s_SquareStatusBoard = new bool[i_boardLength, i_boardWidth];
            m_BoardSize = i_boardLength * i_boardWidth;
            m_BoardLen = i_boardLength;
            m_BoardWidth = i_boardWidth;
        }

        public int GetBoardSize
        {
            get
            {
                return m_BoardSize;
            }

            set
            {
                m_BoardSize = value;
            }
        }

        public int GetBoardLength
        {
            get
            {
                return m_BoardLen;
            }

            set
            {
                m_BoardLen = value;
            }
        }

        public int GetBoardWidth
        {
            get
            {
                return m_BoardWidth;
            }

            set
            {
                m_BoardWidth = value;
            }
        }

        public int[,] GetBoard
        {
            get
            {
                return m_TheBoardMatrix;
            }

            set
            {
                m_TheBoardMatrix = value;
            }
        }


        // $G$ NTT-007 (-10) There's no need to re-instantiate the Random instance each time it is used.
        public void RandomiseCoupleMatrix()
        {
            System.Random rand = new System.Random();
            int numberOfCouples = m_BoardSize / 2;
            for (int i = 0; i < numberOfCouples; i++)
            {
                int row = rand.Next(m_BoardLen);
                int col = rand.Next(m_BoardWidth);
                while (m_TheBoardMatrix[row, col] != 0)
                {
                    row = rand.Next(m_BoardLen);
                    col = rand.Next(m_BoardWidth);
                }

                m_TheBoardMatrix[row, col] = i + 1;
                while (m_TheBoardMatrix[row, col] != 0)
                {
                    row = rand.Next(m_BoardLen);
                    col = rand.Next(m_BoardWidth);
                }

                m_TheBoardMatrix[row, col] = i + 1;
            }
        }
    }
}
