using System;

namespace B20_Ex02
{
    public class Player
    {
        private string m_PlayerName;
        private ushort m_PlayerScore = 0;

        public Player(string i_Name)
        {
            m_PlayerName = i_Name;
        }

        public string GetPlayerName
        {
            get
            {
                return m_PlayerName;
            }

            set
            {
                m_PlayerName = value;
            }
        }

        public ushort GetPlayerScore
        {
            get
            {
                return m_PlayerScore;
            }

            set
            {
                m_PlayerScore = value;
            }
        }

        internal void IncrementScore()
        {
            m_PlayerScore++;
        }
    }
}
