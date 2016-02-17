using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gomoku
{
    class Align
    {
        Position _origine;
        int _length;
        int _xAxe;
        int _yAxe;
        public Align(Position origine, int length, int x, int y)
        {
            _origine = origine;
            _length = length;
            _xAxe = x;
            _yAxe = y;
        }

        public bool contains(Align other)
        {
            bool isEqual = true;

            isEqual &= _xAxe == other._xAxe && _yAxe == other._yAxe || other._length == 1;
            isEqual &= _origine == other._origine;
            return isEqual;
        }
    }
}
