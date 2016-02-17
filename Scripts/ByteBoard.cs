using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gomoku
{
    public class ByteBoard
    {
        #region properties
        public Byte[] _byteBoard { get; set; }
        #endregion

        public ByteBoard()
        {
            _byteBoard = new Byte[91];
        }

        public ByteBoard(UInt32 sizeX, UInt32 sizeY)
        {
            UInt32 size = (sizeX * sizeY) / 4 + 1;
            _byteBoard = new Byte[size];
            for (UInt32 i = 0; i < size; i++)
                _byteBoard[i] = 0;
        }

        public ByteBoard(ByteBoard cpy)
        {
            _byteBoard = new Byte[cpy._byteBoard.Length];
            for (int i = 0; i < cpy._byteBoard.Length; i++)
                _byteBoard[i] = cpy._byteBoard[i];
        }

        public byte GetValue(Position pos)
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

        public bool SetValue(Position pos, int value)
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
    }
}
