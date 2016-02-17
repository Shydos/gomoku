using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Gomoku
{
    public struct Position
    {
        public int x;
        public int y;
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Position(Position cpy)
        {
            this.x = cpy.x;
            this.y = cpy.y;
        }
        public static bool operator ==(Position p1, Position p2)
        {
            return (p1.x == p2.x && p1.y == p2.y);
        }
        public static bool operator !=(Position p1, Position p2)
        {
            return (p1.x != p2.x && p1.y != p2.y);
        }
    }

    public enum Orientation
    {
        Horizontal,
        Vertical,
        UpDiagonal,
        DownDiagonal
    }

    public struct BreakableAlign
    {
        public Position pos;
        public int valueToCheck;
        public String hashCode;
        public BreakableAlign(Position pos, int valueToCheck)
        {
            this.pos = pos;
            this.valueToCheck = valueToCheck;
            hashCode = pos.x + ":" + pos.y + ":" + valueToCheck;
        }        
    }

    public class Board
    {
        #region properties  
        private Referee _referee;
        public byte[] _byteBoard { get; set; }
        public Intersection[,] _interBoard { get; set; }
        private List<BreakableAlign> breakableAlignList;
        private readonly bool _isRuleBreakable;
        private readonly bool _isRuleDoubleThree;
        #endregion

        #region constructors
        public Board(Referee referee, bool isRuleBreakable, bool isRuleDoubleThree)
        {
            _referee = referee;
            _isRuleBreakable = isRuleBreakable;
            _isRuleDoubleThree = isRuleDoubleThree;
            breakableAlignList = new List<BreakableAlign>();
            _interBoard = new Intersection[19, 19];
            _byteBoard = new byte[91];
            for (int i = 0; i < 91; i++)
            {
                _byteBoard[i] = 0;
            }
            for (int x = 0; x < 19; x++)
            {
                for (int y = 0; y < 19; y++)
                {
                    _interBoard[x, y] = new Intersection(new Position(x, y));
                }
            }
        }
        #endregion

        #region methods
        private Position GetFirstOfLine(Position pos, int xAxis, int yAxis, int valueToCheck)
        {
            Position newPos = new Position(pos.x, pos.y);
            bool gotPos = false;
            while (!gotPos)
            {
                if ((newPos.x + xAxis >= 0 && newPos.x + xAxis < 19) && (newPos.y + yAxis >= 0 && newPos.y + yAxis < 19))
                {
                    newPos.x += xAxis;
                    newPos.y += yAxis;
                    if (GetInterValue(newPos)._value != valueToCheck)
                    {
                        newPos.x -= xAxis;
                        newPos.y -= yAxis;
                        gotPos = true;
                    }
                }
                else
                {
                    gotPos = true;
                }
            }
            return newPos;
        }

        private int GetAlignLenght(Position firstPos, int xAxis, int yAxis, int valueToCheck)
        {
            int lenght = 1;
            bool gotLenght = false;
            while (!gotLenght)
            {
                if ((firstPos.x - xAxis >= 0 && firstPos.x - xAxis < 19) && (firstPos.y - yAxis >= 0 && firstPos.y - yAxis < 19))
                {
                    firstPos.x -= xAxis;
                    firstPos.y -= yAxis;
                    if (GetInterValue(firstPos)._value == valueToCheck)
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

        private bool CheckBreakPoint(Position pos, int valueToCheck, bool isRefresh)
        {
            Position leftPos;
            Position rightPos;
            Position farRightPos;
            bool unique;

            for (int xAxis = -1; xAxis <= 1; xAxis++)
            {
                for (int yAxis = -1; yAxis <= 1; yAxis++)
                {
                    if (xAxis != 0 || yAxis != 0)
                    {
                        if (((pos.x + xAxis >= 0 && pos.x + xAxis < 19) && (pos.y + yAxis >= 0 && pos.y + yAxis < 19)) &&
                            ((pos.x - xAxis >= 0 && pos.x - xAxis < 19) && (pos.y - yAxis >= 0 && pos.y - yAxis < 19)) &&
                            ((pos.x - 2 * xAxis >= 0 && pos.x - 2 * xAxis < 19) && (pos.y - 2 * yAxis >= 0 && pos.y - 2 * yAxis < 19)))
                        {
                            leftPos = new Position(pos.x + xAxis, pos.y + yAxis);
                            rightPos = new Position(pos.x - xAxis, pos.y - yAxis);
                            farRightPos = new Position(pos.x - 2 * xAxis, pos.y - 2 * yAxis);
                            if ((GetByteValue(leftPos) == (valueToCheck == 1 ? 2 : 1)) &&
                                (GetByteValue(rightPos) == (valueToCheck)) &&
                                (GetByteValue(farRightPos) == 0))
                            {
                                if (!isRefresh)
                                {
                                    unique = true;
                                    foreach (var align in breakableAlignList)
                                    {
                                        if (align.hashCode == (pos.x + ":" + pos.y + ":" + valueToCheck))
                                        {
                                            unique = false;
                                        }
                                    }
                                    if (unique)
                                    {
                                        breakableAlignList.Add(new BreakableAlign(pos, valueToCheck));
                                    }
                                }
                                return true;
                            }
                            else if ((GetByteValue(leftPos) == 0) &&
                                (GetByteValue(rightPos) == (valueToCheck) &&
                                (GetByteValue(farRightPos) == (valueToCheck == 1 ? 2 : 1))))
                            {
                                if (!isRefresh)
                                {
                                    unique = true;
                                    foreach (var align in breakableAlignList)
                                    {
                                        if (align.hashCode == (pos.x + ":" + pos.y + ":" + valueToCheck))
                                        {
                                            unique = false;
                                        }
                                    }
                                    if (unique)
                                    {
                                        breakableAlignList.Add(new BreakableAlign(pos, valueToCheck));
                                    }
                                }
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Recursive function to check if 5 ubreakable token are aligned
        /// </summary>
        /// <param name="x">x pos</param>
        /// <param name="y">y pos</param>
        /// <param name="xAxis">x axis (from calling pos)</param>
        /// <param name="yAxis">Y axis (from calling pos)</param>
        /// <param name="iteLeft">number of token left to check</param>
        /// <param name="valueToCheck">value to check</param>
        /// <returns>true if good token found</returns>
        private bool CheckUnbreakableAlign(Position pos, int xAxis, int yAxis, int iteLeft, int valueToCheck, bool isRefresh)
        {
            if (iteLeft > 0)
            {
                if ((pos.x >= 0 && pos.x < 19) && (pos.y >= 0 && pos.y < 19))
                {
                    if (GetInterValue(pos)._value == valueToCheck)
                    {
                        if (CheckBreakPoint(pos, valueToCheck, isRefresh))
                        {
                            return false;
                        }
                        else
                        {
                            pos.x -= xAxis;
                            pos.y -= yAxis;
                            return CheckUnbreakableAlign(pos, xAxis, yAxis, iteLeft - 1, valueToCheck, isRefresh);
                        }
                    }
                }
                return false;
            }
            return true;
        }

        public bool RuleFiveBreakable(Player playerplaying, Position pos)
        {
            System.Console.WriteLine("Rule 5 breakable...");
            Position firstOfLine;
            int lenght;

            for (int xAxis = -1; xAxis <= 1; xAxis++)
            {
                for (int yAxis = -1; yAxis <= 1; yAxis++)
                {
                    if (xAxis != 0 || yAxis != 0)
                    {
                        firstOfLine = GetFirstOfLine(pos, xAxis, yAxis, playerplaying._numPlayer);
                        lenght = GetAlignLenght(firstOfLine, xAxis, yAxis, playerplaying._numPlayer);
                        for (int z = 0; z <= (lenght - 5); z++)
                        {
                            if (CheckUnbreakableAlign(firstOfLine, xAxis, yAxis, 5, playerplaying._numPlayer, false))
                            {
                                _referee._winner = playerplaying._numPlayer;
                                return true;
                            }
                            firstOfLine.x -= xAxis;
                            firstOfLine.y -= yAxis;
                        }
                    }
                }
            }
            return false;
        }

        public bool RefreshBreakable()
        {
            System.Console.WriteLine("Refresh 5 breakable...");
            Position firstOfLine;
            int lenght;

            foreach (var align in breakableAlignList)
            {
                for (int xAxis = -1; xAxis <= 1; xAxis++)
                {
                    for (int yAxis = -1; yAxis <= 1; yAxis++)
                    {
                        if (xAxis != 0 || yAxis != 0)
                        {
                            firstOfLine = GetFirstOfLine(align.pos, xAxis, yAxis, align.valueToCheck);
                            lenght = GetAlignLenght(firstOfLine, xAxis, yAxis, align.valueToCheck);
                            for (int z = 0; z <= (lenght - 5); z++)
                            {
                                if (CheckUnbreakableAlign(firstOfLine, xAxis, yAxis, 5, align.valueToCheck, true))
                                {
                                    _referee._winner = align.valueToCheck;
                                    return true;
                                }
                                firstOfLine.x -= xAxis;
                                firstOfLine.y -= yAxis;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Recursive function to check if 5 token are aligned
        /// </summary>
        /// <param name="x">x pos</param>
        /// <param name="y">y pos</param>
        /// <param name="xAxis">x axis (from calling pos)</param>
        /// <param name="yAxis">Y axis (from calling pos)</param>
        /// <param name="iteLeft">number of token left to check</param>
        /// <param name="valueToCheck">value to check</param>
        /// <returns>true if good token found</returns>
        private bool CheckAlign(Position pos, int xAxis, int yAxis, int iteLeft, int valueToCheck)
        {
            if (iteLeft > 0)
            {
                if ((pos.x >= 0 && pos.x < 19) && (pos.y >= 0 && pos.y < 19))
                {
                    if (GetInterValue(pos)._value == valueToCheck)
                    {
                        pos.x -= xAxis;
                        pos.y -= yAxis;
                        return CheckAlign(pos, xAxis, yAxis, iteLeft - 1, valueToCheck);
                    }
                }
                return false;
            }
            return true;
        }

        public bool RuleAlignFiveStones(Player playerplaying, Position pos)
        {
            System.Console.WriteLine("Rule 5 align...");
            Position firstOfLine;

            for (int xAxis = -1; xAxis <= 1; xAxis++)
            {
                for (int yAxis = -1; yAxis <= 1; yAxis++)
                {
                    if (xAxis != 0 || yAxis != 0)
                    {
                        firstOfLine = GetFirstOfLine(pos, xAxis, yAxis, playerplaying._numPlayer);
                        if (CheckAlign(firstOfLine, xAxis, yAxis, 5, playerplaying._numPlayer))
                        {
                            _referee._winner = playerplaying._numPlayer;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Recursive function to check if a pair can be taken
        /// </summary>
        /// <param name="x">x pos</param>
        /// <param name="y">y pos</param>
        /// <param name="xAxis">x axis (from calling pos)</param>
        /// <param name="yAxis">Y axis (from calling pos)</param>
        /// <param name="iteLeft">number of token left to check</param>
        /// <param name="valueToCheck">value to check</param>
        /// <returns>true if good token found</returns>
        private bool CheckCapture(Position pos, int xAxis, int yAxis, int iteLeft, int valueToCheck)
        {
            int tmpValueToCheck = valueToCheck;
            if (iteLeft > 0)
            {
                if (iteLeft == 2 || iteLeft == 3)
                {
                    tmpValueToCheck = (tmpValueToCheck == 1) ? 2 : 1;
                }
                pos.x += xAxis;
                pos.y += yAxis;
                if ((pos.x >= 0 && pos.x < 19) && (pos.y >= 0 && pos.y < 19))
                {
                    if (GetInterValue(pos)._value == tmpValueToCheck)
                    {
                        return CheckCapture(pos, xAxis, yAxis, iteLeft - 1, valueToCheck);
                    }
                }
                return false;
            }
            return true;
        }

        public bool RuleCaptureTenStones(Player playerplaying, Position pos)
        {
            System.Console.WriteLine("Rule 10 stones...");
            for (int xAxis = -1; xAxis <= 1; xAxis++)
            {
                for (int yAxis = -1; yAxis <= 1; yAxis++)
                {
                    if (xAxis != 0 || yAxis != 0)
                    {
                        if (CheckCapture(pos, xAxis, yAxis, 3, playerplaying._numPlayer))
                        {
                            Position rmvPos = new Position(pos.x + xAxis, pos.y + yAxis);
                            SetInterValue(rmvPos, 0);
                            SetByteValue(rmvPos, 0);
                            rmvPos.x += xAxis;
                            rmvPos.y += yAxis;
                            SetInterValue(rmvPos, 0);
                            SetByteValue(rmvPos, 0);
                            playerplaying._nbToken += 2;
                        }
                    }
                }
            }
            if (playerplaying._nbToken >= 10)
            {
                _referee._winner = playerplaying._numPlayer;
                return true;
            }
            return false;
        }

        private int checkDoubleThree(Orientation orientation, int checkPos, int i, Position pos, int numPlayer, int iMax)
        {
            int x = pos.x + checkPos + i;
            int y = pos.y + checkPos + i;
            switch (orientation)
            {
                case Orientation.Horizontal:
                    y = pos.y;
                    break;
                case Orientation.Vertical:
                    x = pos.x;
                    break;
                case Orientation.DownDiagonal:
                    break;
                case Orientation.UpDiagonal:
                    y = pos.y - checkPos - i;
                    break;
            }
            if ((x < 0 || x >= 19 || y < 0 || y >= 19) || (i == 0 || i == iMax) && _interBoard[x, y]._value != 0)
                return 42;
            else if (_interBoard[x, y]._value == numPlayer)
                return 1;
            if (iMax == 5)
            {
                if ((i == 2 || i == 3) && _interBoard[x, y]._value != 0 && _interBoard[x, y]._value != numPlayer)
                    return 42;
                if ((i == 1 || i == 4) && _interBoard[x, y]._value != numPlayer)
                    return 42;
            }
            return 0;
        }

        public bool RuleDoubleThree(Player playerplaying, Position pos)
        {
            System.Console.WriteLine("Rule double 3...");
            int checkPos = -3;

            // Attention !!!
            _interBoard[pos.x, pos.y]._value = playerplaying._numPlayer;
            while (checkPos < 0)
            {
                int[] cpt = { 0, 0, 0, 0 };

                for (int i = 0; i < 5; i++)
                {
                    cpt[0] += checkDoubleThree(Orientation.Horizontal, checkPos, i, pos, playerplaying._numPlayer, 4);
                    cpt[1] += checkDoubleThree(Orientation.Vertical, checkPos, i, pos, playerplaying._numPlayer, 4);
                    cpt[2] += checkDoubleThree(Orientation.DownDiagonal, checkPos, i, pos, playerplaying._numPlayer, 4);
                    cpt[3] += checkDoubleThree(Orientation.UpDiagonal, checkPos, i, pos, playerplaying._numPlayer, 4);
                }
                if (cpt[0] == 3 || cpt[1] == 3 || cpt[2] == 3 || cpt[3] == 3)
                {
                    Console.WriteLine("\nTrois libre alligné !\n");
                }
                checkPos++;
            }
            checkPos = -4;
            while (checkPos < 0)
            {
                int[] cpt = { 0, 0, 0, 0 };

                for (int i = 0; i < 6; i++)
                {
                    cpt[0] += checkDoubleThree(Orientation.Horizontal, checkPos, i, pos, playerplaying._numPlayer, 5);
                    cpt[1] += checkDoubleThree(Orientation.Vertical, checkPos, i, pos, playerplaying._numPlayer, 5);
                    cpt[2] += checkDoubleThree(Orientation.DownDiagonal, checkPos, i, pos, playerplaying._numPlayer, 5);
                    cpt[3] += checkDoubleThree(Orientation.UpDiagonal, checkPos, i, pos, playerplaying._numPlayer, 5);
                }
                if (cpt[0] == 3 || cpt[1] == 3 || cpt[2] == 3 || cpt[3] == 3)
                {
                    Console.WriteLine("\nTrois libre espacé !\n");
                }
                checkPos++;
            }
            // Attention !!!
            _interBoard[pos.x, pos.y]._value = 0;
            return true;
        }

        public bool possiblePos(Player playerplaying, Position pos)
        {
            if (pos.x >= 0 && pos.x < 19 && pos.y >= 0 && pos.y < 19)
            {
                if (_interBoard[pos.x, pos.y]._value == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool putTocken(Player playerplaying, Position pos)
        {
            System.Console.WriteLine("Puting token...");
            bool isTokenValid = false;
            if (possiblePos(playerplaying, pos))
            {
                if (_isRuleDoubleThree)
                {
                    if (RuleDoubleThree(playerplaying, pos))
                    {
                        SetInterValue(pos, playerplaying._numPlayer);
                        SetByteValue(pos, playerplaying._numPlayer);
                        isTokenValid = true;
                    }
                }
                else
                {
                    SetInterValue(pos, playerplaying._numPlayer);
                    SetByteValue(pos, playerplaying._numPlayer);
                    isTokenValid = true;
                }
                if (isTokenValid)
                {
                    if (!RuleCaptureTenStones(playerplaying, pos))
                    {
                        if (_isRuleBreakable)
                        {
                            RuleFiveBreakable(playerplaying, pos);
                            RefreshBreakable();
                        }
                        else
                        {
                            RuleAlignFiveStones(playerplaying, pos);
                        }
                    }
                }
            }
            return isTokenValid;
        }

        public bool CheckPlays(int numPlayer)
        {
            System.Console.WriteLine("Checking plays...");
            return true;
        }

        public Intersection GetInterValue(Position pos)
        {
            return _interBoard[pos.x, pos.y];
        }

        public void SetInterValue(Position pos, int value)
        {
            _interBoard[pos.x, pos.y]._value = value;
        }

        public byte GetByteValue(Position pos)
        {
            int x = pos.x;
            int y = pos.y;
            int _recordSize = 2;
            int _sizeMax = 19;
            byte _mask1 = 0x03;
            byte _mask2 = 0xC0;
            byte _mask3 = 0x30;
            byte _mask4 = 0x0C;
            byte rep = 0;
            int calc = 0;
            int first_id = 0;
            int second_id = 0;

            calc = (y * _recordSize * _sizeMax) + ((x + 1) * _recordSize);
            if (y == 0 || y == 18)
            {
                first_id = calc / 8 - 1;
                second_id = calc / 8;
            }
            else
            {
                first_id = calc / 8;
                second_id = calc / 8 + 1;
            }
            switch (calc % 8)
            {
                case 0:
                    rep = (byte)(_byteBoard[first_id] & _mask1);
                    break;
                case 2:
                    rep = (byte)(_byteBoard[second_id] & _mask2);
                    rep = (byte)(rep >> 6);
                    break;
                case 4:
                    rep = (byte)(_byteBoard[second_id] & _mask3);
                    rep = (byte)(rep >> 4);
                    break;
                case 6:
                    rep = (byte)(_byteBoard[second_id] & _mask4);
                    rep = (byte)(rep >> 2);
                    break;
                default:
                    rep = 0xFF;
                    break;
            }
            return rep;
        }

        public bool SetByteValue(Position pos, int value)
        {
            int x = pos.x;
            int y = pos.y;
            int _recordSize = 2;
            int _sizeMax = 19;
            byte maskValue = 0x0;
            int calc = 0;
            int first_id = 0;
            int second_id = 0;

            calc = (y * _recordSize * _sizeMax) + ((x + 1) * _recordSize);
            if (y == 0 || y == 18)
            {
                first_id = calc / 8 - 1;
                second_id = calc / 8;
            }
            else
            {
                first_id = calc / 8;
                second_id = calc / 8 + 1;
            }
            switch (value)
            {
                case 0:
                    maskValue = 0;
                    break;
                case 1:
                    maskValue = 0x01;
                    break;
                case 2:
                    maskValue = 0x02;
                    break;
                default:
                    return false;
            }
            switch (calc % 8)
            {
                case 0:
                    _byteBoard[first_id] = (byte)(_byteBoard[first_id] & 0xFC);
                    _byteBoard[first_id] = (byte)(_byteBoard[first_id] | maskValue);
                    break;
                case 2:
                    maskValue = (byte)(maskValue << 6);
                    _byteBoard[second_id] = (byte)(_byteBoard[second_id] & 0x3F);
                    _byteBoard[second_id] = (byte)(_byteBoard[second_id] | maskValue);
                    break;
                case 4:
                    maskValue = (byte)(maskValue << 4);
                    _byteBoard[second_id] = (byte)(_byteBoard[second_id] & 0xCF);
                    _byteBoard[second_id] = (byte)(_byteBoard[second_id] | maskValue);
                    break;
                case 6:
                    maskValue = (byte)(maskValue << 2);
                    _byteBoard[second_id] = (byte)(_byteBoard[second_id] & 0xF3);
                    _byteBoard[second_id] = (byte)(_byteBoard[second_id] | maskValue);
                    break;
                default:
                    return false;
            }
            return true;
        }

        #endregion
    }
}
