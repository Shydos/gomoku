using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gomoku
{
    public class Referee
    {
        #region properties
        public Board _board { get; set; }
        public Player _player1 { get; set; }
        public Player _player2 { get; set; }
        public int _winner { get; set; }
        public Player _playerPlaying { get; set; }
        public bool _boardChanged { get; set; }
        public Position _lastPos { get; set; }
        private Position invalidPos;
        #endregion

        #region constructors
        public Referee(bool isRuleBreakable, bool isRuleDoubleThree, bool AI)
        {
            _board = new Board(this, isRuleBreakable, isRuleDoubleThree);
            _player1 = new HumanPlayer(this, 1);
            if (AI)
                _player2 = new AIPlayer(this, 2);
            else
                _player2 = new HumanPlayer(this, 2);
            _winner = 0;
            _playerPlaying = _player1;
            invalidPos = new Position(-1, -1);
            _lastPos = new Position(-1, -1);
            _boardChanged = false;
        }
        #endregion

        #region methods
        public void Start()
        {
        }

        public void Update()
        {
            if (_boardChanged && _playerPlaying is HumanPlayer)
            {
                _board.CheckPlays(1);
            }
            _lastPos = _playerPlaying.AskedPos();
            if (_lastPos != invalidPos)
            {
                if (_boardChanged = _board.putTocken(_playerPlaying, _lastPos))
                {
                    _playerPlaying = System.Object.ReferenceEquals(_playerPlaying, _player1) ? _player2 : _player1;
                }
                _lastPos = invalidPos;
            }
        }
        #endregion
    }
}
