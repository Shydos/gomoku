using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gomoku
{
    class AIPlayer : Player
    {
        public AI _ai { get; set; }

        #region constructor
        public AIPlayer(Referee myReferee, int numPlayer) : base(myReferee, numPlayer)
        {
            _ai = new AI(_referee._board);
        }
        #endregion

        #region methods
        public override Position AskedPos()
        {
            return _ai.GetPosToPLay();
        }
        #endregion
    }
}