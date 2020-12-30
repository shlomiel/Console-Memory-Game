using System;
using System.Collections.Generic;

namespace B20_Ex02
{

    // $G$ DSN-001 (-5) Game flow implementation does not belong in this class.

    public class UserInterface
    {
        private const int k_MiliSecondsTosleep = 2000;
        readonly Dictionary<int, char> m_ToCharDict = new Dictionary<int, char>();
        private GameBoard m_TheBoard;
        private Player m_Player1;
        private Player m_Player2;
        private AIPlayer m_ComputerPlayer;
        private bool m_IsGameOver;

        private bool GetIsGameOver()
        {
            if (m_Player2 != null)
            {
                m_IsGameOver = (m_Player1.GetPlayerScore + m_Player2.GetPlayerScore) >= (m_TheBoard.GetBoardSize / 2);
            }
            else
            {
                m_IsGameOver = (m_Player1.GetPlayerScore + m_ComputerPlayer.GetCompPlayer.GetPlayerScore) >= (m_TheBoard.GetBoardSize / 2);
            }

            return m_IsGameOver;
        }

        public void InitializeCharIntDictionary()
        {
            char letter = 'A';
            m_ToCharDict.Clear();
            for (int i = 0; i < 'Z' - 'A'; i++)
            {
                m_ToCharDict.Add(i, letter);
                letter++;
            }
        }

        public Dictionary<int, char> GetDictionary
        {
            get
            {
                return m_ToCharDict;
            }
        }

        private void printBoard()
        {
            char charToPrint = 'A';
            System.Console.Write(" ");
            for (int i = 0; i < m_TheBoard.GetBoardWidth; i++)
            {
                System.Console.Write("   " + charToPrint);
                charToPrint++;
            }



            // $G$ NTT-999 (-5) Should use Environment.NewLine rather than \n.

            for (int lineNumber = 0; lineNumber < m_TheBoard.GetBoardLength; lineNumber++)
            {
                System.Console.Write("\n  ");
                for (int j = 0; j <= m_TheBoard.GetBoardWidth + 1; j++)
                {
                    System.Console.Write("===");
                }

                System.Console.WriteLine();
                System.Console.Write(lineNumber + 1 + " |");
                for (int j = 0; j < m_TheBoard.GetBoardWidth; j++)
                {
                    if (GameBoard.s_SquareStatusBoard[lineNumber, j] == true)
                    {
                        GetDictionary.TryGetValue(m_TheBoard.GetBoard[lineNumber, j], out charToPrint);
                        System.Console.Write(" " + charToPrint + " |");
                    }
                    else
                    {
                        System.Console.Write("   |");
                    }
                }
            }

            System.Console.Write("\n  ");
            for (int j = 0; j <= m_TheBoard.GetBoardWidth + 1; j++)
            {
                System.Console.Write("===");
            }

            System.Console.WriteLine(string.Empty);
        }

        // $G$ NTT-999 (-5) Should use Environment.NewLine rather than \n.
        public void SetUpGame()
        {
            if (m_Player1 == null)
            {
                int selectionBuffer;
                System.Console.WriteLine("Hello and welcome!\nPlease enter player name:");

                m_Player1 = new Player(System.Console.ReadLine());
                getSelectionAndCheckIfValid("You wish to:\n1)play with a friend\n2)the computer:", out selectionBuffer);
                if (selectionBuffer == 1)
                {
                    System.Console.WriteLine("Please enter second player name:");
                    m_Player2 = new Player(System.Console.ReadLine());
                    while (m_Player1.GetPlayerName == m_Player2.GetPlayerName)
                    {
                        System.Console.WriteLine("Player name already taken, please choose diffrent name:");
                        m_Player2 = new Player(System.Console.ReadLine());
                    }
                }
            }
            else
            {
                m_Player1.GetPlayerScore = 0;
            }

            int boardWidth, boardLength;
            checkIfValidBoardSize(out boardLength, out boardWidth);
            m_TheBoard = new GameBoard(boardLength, boardWidth);
            if (m_Player2 == null)
            {
                m_ComputerPlayer = new AIPlayer(m_TheBoard.GetBoardSize);
            }
            else
            {
                m_Player2.GetPlayerScore = 0;
            }

            m_TheBoard.RandomiseCoupleMatrix();
            InitializeCharIntDictionary();
            startGame();
        }

        private void startGame()
        {
            int TurnDecider = 0;
            while (!GetIsGameOver())
            {
                if (TurnDecider % 2 == 0)
                {
                    playRound(m_Player1, ref TurnDecider);
                }
                else if (m_Player2 != null)
                {
                    playRound(m_Player2, ref TurnDecider);
                }
                else
                {
                    m_ComputerPlayer.SetNextComputerTurn(m_TheBoard.GetBoardLength, m_TheBoard.GetBoardWidth);
                    playRound(m_ComputerPlayer.GetCompPlayer, ref TurnDecider);
                }
            }

            if (m_Player2 != null)
            {
                announceWinner(m_Player1, m_Player2);
            }
            else
            {
                announceWinner(m_Player1, m_ComputerPlayer.GetCompPlayer);
            }

        }


        // $G$ CSS-999 (-5) Ref parameters should start with io_PascaleCased
        private void playRound(Player io_PlayerX, ref int i_TurnDecider)
        {
            int squareValue1, squareValue2;
            int firstRowSelected, firstColSelected, secondRowSelected, secondColSelected;
            Ex02.ConsoleUtils.Screen.Clear();
            printBoard();

            if (io_PlayerX.GetPlayerName != "ComputerPlayer")
            {
                squareValue1 = handleSquarePlayerInput(io_PlayerX, out firstRowSelected, out firstColSelected);
                squareValue2 = handleSquarePlayerInput(io_PlayerX, out secondRowSelected, out secondColSelected);
            }
            else
            {
                firstRowSelected = m_ComputerPlayer.m_SaveNextTurn[0].GetRow;
                firstColSelected = m_ComputerPlayer.m_SaveNextTurn[0].GetCol;
                secondRowSelected = m_ComputerPlayer.m_SaveNextTurn[1].GetRow;
                secondColSelected = m_ComputerPlayer.m_SaveNextTurn[1].GetCol;
                squareValue1 = m_TheBoard.GetBoard[firstRowSelected, firstColSelected];
                squareValue2 = m_TheBoard.GetBoard[secondRowSelected, secondColSelected];
                System.Console.WriteLine("{0} , {1}", firstRowSelected + 1, (char)(firstColSelected + 'A'));
                System.Console.WriteLine("{0} , {1}", secondRowSelected + 1, (char)(secondColSelected + 'A'));
                System.Threading.Thread.Sleep(k_MiliSecondsTosleep);
                Ex02.ConsoleUtils.Screen.Clear();
                GameBoard.s_SquareStatusBoard[firstRowSelected, firstColSelected] = true;
                GameBoard.s_SquareStatusBoard[secondRowSelected, secondColSelected] = true;

                printBoard();
            }

            if (squareValue1 == squareValue2)
            {
                System.Console.WriteLine(string.Format("\nNice job! {0} you get a point!", io_PlayerX.GetPlayerName));
                io_PlayerX.IncrementScore();
                System.Threading.Thread.Sleep(k_MiliSecondsTosleep);
                if (m_ComputerPlayer != null)
                {
                    m_ComputerPlayer.m_RandNextRound = io_PlayerX.GetPlayerName == "ComputerPlayer";
                }
            }
            else
            {
                if (m_ComputerPlayer != null)
                {
                    m_ComputerPlayer.SavingRevealedCardToMemory(squareValue1, new Square(firstRowSelected, firstColSelected));
                    m_ComputerPlayer.SavingRevealedCardToMemory(squareValue2, new Square(secondRowSelected, secondColSelected));
                }

                System.Console.WriteLine(string.Format("\nOhhh nice try {0}, maybe next round", io_PlayerX.GetPlayerName));
                GameBoard.s_SquareStatusBoard[firstRowSelected, firstColSelected] = false;
                GameBoard.s_SquareStatusBoard[secondRowSelected, secondColSelected] = false;
                ++i_TurnDecider;
                System.Threading.Thread.Sleep(k_MiliSecondsTosleep);
                Ex02.ConsoleUtils.Screen.Clear();
                printBoard();
            }
        }

        private void announceWinner(Player i_Player1, Player i_Player2)
        {
            const int optionNo = 2;
            int selectionBuffer;
            {
                System.Console.WriteLine(
                "Game is over! {0} has {1} points, {2} has {3} points!",
                 m_Player1.GetPlayerName,
                 i_Player1.GetPlayerScore,
                 i_Player2.GetPlayerName,
                 i_Player2.GetPlayerScore);
                if (m_Player1.GetPlayerScore > i_Player2.GetPlayerScore)
                {
                    System.Console.WriteLine("\nCongratulations {0}, you won!", i_Player1.GetPlayerName);
                }
                else if (i_Player1.GetPlayerScore < i_Player2.GetPlayerScore)
                {
                    System.Console.WriteLine("\nCongratulations {0}, you won!", i_Player2.GetPlayerName);
                }
                else
                {
                    System.Console.WriteLine("\nIt's a ite!");
                }

                getSelectionAndCheckIfValid("Would you like to play another round?\n1)Yes\n2)No\n", out selectionBuffer);

                m_IsGameOver = selectionBuffer == optionNo;
                if (!m_IsGameOver)
                {
                    SetUpGame();
                }
            }
        }

        private int handleSquarePlayerInput(Player i_PlayerX, out int io_RowSelected, out int io_ColSelected)
        {
            System.Console.WriteLine(string.Format("\n{0}, please select square to unveil (enter row and than enter column):", i_PlayerX.GetPlayerName));
            makeSureSquareSelectionIsInRange(out io_RowSelected, out io_ColSelected);
            GameBoard.s_SquareStatusBoard[io_RowSelected, io_ColSelected] = true;
            Ex02.ConsoleUtils.Screen.Clear();
            printBoard();
            return m_TheBoard.GetBoard[io_RowSelected, io_ColSelected];
        }

        // $G$ CSS-999 (-5) Out parameters should start with o_PascaleCased
        // $G$ DSN-002 (-20) No separation between the logical part of the game and the UI.

        private void makeSureSquareSelectionIsInRange(out int i_row, out int i_col)
        {
            string checkIfQuit;
            checkIfQuit = System.Console.ReadLine();
            int.TryParse(checkIfQuit, out i_row);
            if (checkIfQuit == "Q")
            {
                i_col = 0;
                System.Console.WriteLine("You chose to quit the game, see you next time!");
                System.Threading.Thread.Sleep(k_MiliSecondsTosleep);
                Environment.Exit(-1);
            }
            else
            {
                string inputBuffer = System.Console.ReadLine();
                while (inputBuffer.Length != 1)
                {
                    System.Console.WriteLine("Column must be a single charcater, please enter column again:");
                    inputBuffer = System.Console.ReadLine();
                }

                char let = char.Parse(inputBuffer);
                i_col = let - 'A' + 1;

                while (i_row > m_TheBoard.GetBoardLength || i_row < 1 || i_col > m_TheBoard.GetBoardWidth || i_col < 1 || GameBoard.s_SquareStatusBoard[i_row - 1, i_col - 1] == true)
                {
                    if (i_row < 1 || i_row > m_TheBoard.GetBoardLength)
                    {
                        System.Console.WriteLine("Row is not in range, please enter numbers between 1 to {0}", m_TheBoard.GetBoardLength);
                    }
                    else if (i_col > m_TheBoard.GetBoardWidth || i_col < 1)
                    {
                        System.Console.WriteLine("Column is not in range, please enter letters between A to {0}", (char)(m_TheBoard.GetBoardWidth + 'A' - 1));
                    }
                    else
                    {
                        System.Console.WriteLine("Choose a different square, the chosen square already been revealed");
                    }

                    int.TryParse(System.Console.ReadLine(), out i_row);
                    let = char.Parse(System.Console.ReadLine());
                    i_col = let - 'A' + 1;
                }

                i_row--;
                i_col--;
            }
        }

        private void getSelectionAndCheckIfValid(string i_message, out int i_selection)
        {
            System.Console.WriteLine(i_message);
            int.TryParse(System.Console.ReadLine(), out i_selection);
            while (i_selection != 1 && i_selection != 2)
            {
                System.Console.WriteLine("Invalid selection, please try again");
                System.Console.WriteLine(i_message);
                int.TryParse(System.Console.ReadLine(), out i_selection);
            }
        }

        private void checkIfValidBoardSize(out int i_Length, out int i_Width)
        {
            System.Console.WriteLine("Enter board size (Length , Width) sizes must be between 4 and 6:");
            int.TryParse(System.Console.ReadLine(), out i_Length);
            int.TryParse(System.Console.ReadLine(), out i_Width);
            while (i_Width < 4 || i_Width > 6 || i_Length < 4 || i_Length > 6 || (i_Width * i_Length) % 2 != 0)
            {
                if (i_Width < 4 || i_Width > 6 || i_Length < 4 || i_Length > 6)
                {
                    System.Console.WriteLine("Invalid selection,length and width between 4 and 6 only!");
                }
                else
                {
                    System.Console.WriteLine("Invalid selection, number of squares meaning rows*columns must be even\ntry again:");
                }

                System.Console.WriteLine("Enter board size (Length , Width):");
                int.TryParse(System.Console.ReadLine(), out i_Width);
                int.TryParse(System.Console.ReadLine(), out i_Length);
            }
        }
    }
}
