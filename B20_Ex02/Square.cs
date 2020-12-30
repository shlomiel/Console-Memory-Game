using System;
using System.Collections.Generic;

namespace B20_Ex02
{
    public struct Square
    {
        private int m_Row;
        private int m_Col;
        private bool m_VisitedBefore;

        public Square(int i_row, int i_col)
        {
            m_Row = i_row;
            m_Col = i_col;
            m_VisitedBefore = false;
        }

        public bool GetVisitedBefore
        {
            get
            {
                return m_VisitedBefore;
            }

            set
            {
                m_VisitedBefore = value;
            }
        }

        public int GetRow
        {
            get
            {
                return m_Row;
            }

            set
            {
                m_Row = value;
            }
        }

        public int GetCol
        {
            get
            {
                return m_Col;
            }

            set
            {
                m_Col = value;
            }
        }

        public static bool operator !=(Square i_Square1, Square i_Square2)
        {
            return !i_Square1.Equals(i_Square2);
        }

        public static bool operator ==(Square i_Square1, Square i_Square2)
        {
            return i_Square1.Equals(i_Square2);
        }

        public override bool Equals(object i_obj)
        {
            if (!(i_obj is Square))
            {
                return false;
            }

            Square toCompare = (Square)i_obj;
            return (m_Row == toCompare.m_Row && m_Col == toCompare.m_Col);
        }
    }
}