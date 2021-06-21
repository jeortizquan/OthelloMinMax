using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinMax_Algorithm
{
    class Position
    {
        #region " Attributes "
        public byte x { set; get; }
        public byte y { set; get; }
        public byte Black { set; get; }
        public byte White { set; get; }
        #endregion

        #region " Constructor's "
        public Position()
        {
        }
        public Position(byte _x, byte _y)
        {
            x = _x;
            y = _y;
        }
        public Position(byte _x, byte _y, byte _Black, byte _White)
        {
            x = _x;
            y = _y;
            Black = _Black;
            White = _White;
        }
        #endregion
    }
}
