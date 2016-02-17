using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gomoku
{
    class HumanPlayer : Player
    {
        #region constructor
        public HumanPlayer(Referee myReferee, int numPlayer) : base(myReferee, numPlayer)
        {
        }
        #endregion

        #region methods
        public override Position AskedPos()
        {
            return _referee._lastPos;
        }
        #endregion
    }
}
