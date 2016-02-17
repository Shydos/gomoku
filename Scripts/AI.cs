using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Gomoku
{
    class AI
    {
        #region properties
        Board _board;
        ByteBoard _bBoard;
        public struct ScorePos
        {
            public Position pos;
            public int score;
            public ScorePos(Position pos, int score)
            {
                this.pos.x = pos.x;
                this.pos.y = pos.y;
                this.score = score;
            }
        }
        private const int MAX_DEPTH = 2;
        private const int WIN_SCORE = 100000;
        private const int LOOSE_SCORE = -100000;

        Dictionary<int, int> patternMap;
        private const int DEFAULT = 100;
        private const int AL4 = 8;
        private const int AL3 = 4;
        private const int AL2 = 2;
        private const int AL1 = 1;
        #endregion

        #region constructors
        public AI(Board board)
        {
            _board = board;
            _bBoard = new ByteBoard();
            patternMap = new Dictionary<int, int>();
            patternMap.Add(1, AL1);
            patternMap.Add(2, AL2);
            patternMap.Add(3, AL3);
            patternMap.Add(4, AL4);
            patternMap.Add(5, DEFAULT);
        }
        #endregion

        #region methodes
        private int GetAlignLenght(Position firstPos, int xAxis, int yAxis, int valueToCheck, ByteBoard board)
        {
            int lenght = 1;
            bool gotLenght = false;
            while (!gotLenght)
            {
                if ((firstPos.x - xAxis >= 0 && firstPos.x - xAxis < 19) && (firstPos.y - yAxis >= 0 && firstPos.y - yAxis < 19))
                {
                    firstPos.x -= xAxis;
                    firstPos.y -= yAxis;
                    if (board.GetValue(firstPos) == valueToCheck)
                    {
                        lenght++;
                    }
                    else
                    {
                        gotLenght = true;
                    }
                }
                else
                {
                    gotLenght = true;
                }
            }
            return lenght;
        }

        private int getAnnoyanceScore(ByteBoard board, int numPlayer, Position pos)
        {
            int alLenght;
            int annScore = 0;
            int valToCheck = (numPlayer == 2 ? 1 : 2);

            for (int xAxis = -1; xAxis <= 1; xAxis++)
            {
                for (int yAxis = -1; yAxis <= 1; yAxis++)
                {
                    if (xAxis != 0 || yAxis != 0)
                    {
                        if ((pos.x - xAxis >= 0 && pos.x - xAxis < 19) && (pos.y - yAxis >= 0 && pos.y - yAxis < 19))
                        {
                            if (board.GetValue(new Position(pos.x - xAxis, pos.y - yAxis)) == valToCheck)
                            {
                                alLenght = GetAlignLenght(pos, xAxis, yAxis, valToCheck, board);
                                if (alLenght > 5)
                                    alLenght = 5;
                                annScore += patternMap[alLenght];
                            }                            
                        }
                    }
                }
            }
            return (annScore);
        }
            
        private int Eval(ByteBoard board, int numPlayer)
        {
            Position pos;
            int score = 0;

            for (int x = 0; x < 19; x++)
            {
                for (int y = 0; y < 19; y++)
                {
                    pos.x = x;
                    pos.y = y;
                    if (board.GetValue(pos) == numPlayer)
                    {
                        score += getAnnoyanceScore(board, numPlayer, pos);
                    }
                }
            }
            return score;
        }

        private ScorePos Minimax(ByteBoard node, int depth, int min, int max)
        {
            int childScore;
            ScorePos scorePos = new ScorePos();
            ByteBoard newNode;

            // Cas spcécifique (sortie de récursivité)
            if (depth == MAX_DEPTH)
            {
                scorePos.score = Eval(node, (depth % 2 == 0 ? 2 : 1));
                return (scorePos);
            }
            // Cas général
            // Node is Max (AI turn)
            if (depth % 2 == 0)
            {
                scorePos.score = min;
                for (int y = 0; y < 19; y++)
                {
                    for (int x = 0; x < 19; x++)
                    {
                        if (node.GetValue(new Position(x, y)) == 0)
                        {
                            newNode = new ByteBoard(node);
                            newNode.SetValue(new Position(x, y), 2);
                            childScore = Minimax(newNode, depth + 1, scorePos.score, max).score;
                            if (childScore > scorePos.score)
                            {
                                scorePos.score = childScore;
                                scorePos.pos.x = x;
                                scorePos.pos.y = y;
                            }
                            if (scorePos.score > max)
                            {
                                scorePos.score = max;
                                return (scorePos);
                            }
                        }
                    }
                }
                return (scorePos);
            }
            // Node is Min (opponent turn)
            else
            {
                scorePos.score = max;
                for (int y = 0; y < 19; y++)
                {
                    for (int x = 0; x < 19; x++)
                    {
                        if (node.GetValue(new Position(x, y)) == 0)
                        {
                            newNode = new ByteBoard(node);
                            newNode.SetValue(new Position(x, y), 1);
                            childScore = Minimax(newNode, depth + 1, min, scorePos.score).score;
                            if (childScore < scorePos.score)
                            {
                                scorePos.score = childScore;
                                scorePos.pos.x = x;
                                scorePos.pos.y = y;
                            }
                            if (scorePos.score < min)
                            {
                                scorePos.score = min;
                                return (scorePos);
                            }
                        }
                    }
                }
                return (scorePos);
            }
        }

        public Position GetPosToPLay()
        {
            for (int i = 0; i < 91; i++)
                _bBoard._byteBoard[i] = _board._byteBoard[i];
            return Minimax(_bBoard, 0, LOOSE_SCORE, WIN_SCORE).pos;
        }
        #endregion
    }
}