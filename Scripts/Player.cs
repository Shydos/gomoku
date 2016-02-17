using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gomoku
{
    public abstract class Player
    {
        #region properties  
        protected Referee _referee;
        public int _numPlayer { get; set; }
        public int _nbToken { get; set; }
        #endregion

        #region constructors
        public Player(Referee referee, int numPlayer)
        {
            _referee = referee;
            _numPlayer = numPlayer;
            _nbToken = 0;
        }
        #endregion
        
        #region methods
        public abstract Position AskedPos();
        #endregion
    }
}
