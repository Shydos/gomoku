using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gomoku
{
    public enum Status {normal, good};
    public class Intersection
    {
        #region properties
        public Position _pos { get; set; }
        public int _value { get; set; }
        public Status _status { get; set; }
        #endregion

        #region constructors
        public Intersection(Position pos)
        {
            _pos = pos;
            _value = 0;
            _status = Status.normal;
        }
        #endregion
    }
}
