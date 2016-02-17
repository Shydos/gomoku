using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gomoku
{
    class Program
    {
        public Referee referee;

        public void Start()
        {
            bool isRuleBreakable;
            bool isRuleDoubleThree;

            isRuleBreakable = true;
            isRuleDoubleThree = true;
            referee = new Referee(isRuleBreakable, isRuleDoubleThree, true);
            referee.Start();

            #region
            // TEST
            /*
            referee._board.SetInterValue(new Position(0, 0), 1);
            referee._board.SetByteValue(new Position(0, 0), 1);
            referee._board.SetInterValue(new Position(1, 1), 2);
            referee._board.SetByteValue(new Position(1, 1), 2);
            referee._board.SetInterValue(new Position(2, 2), 2);
            referee._board.SetByteValue(new Position(2, 2), 2);

            referee._board.SetInterValue(new Position(6, 1), 1);
            referee._board.SetByteValue(new Position(6, 1), 1);
            referee._board.SetInterValue(new Position(7, 1), 1);
            referee._board.SetByteValue(new Position(7, 1), 1);
            referee._board.SetInterValue(new Position(8, 1), 1);
            referee._board.SetByteValue(new Position(8, 1), 1);
            referee._board.SetInterValue(new Position(9, 1), 1);
            referee._board.SetByteValue(new Position(9, 1), 1);
            referee._board.SetInterValue(new Position(11, 1), 1);
            referee._board.SetByteValue(new Position(11, 1), 1);
            referee._board.SetInterValue(new Position(12, 1), 1);
            referee._board.SetByteValue(new Position(12, 1), 1);
            referee._board.SetInterValue(new Position(8, 0), 2);
            referee._board.SetByteValue(new Position(8, 0), 2);
            referee._board.SetInterValue(new Position(8, 2), 1);
            referee._board.SetByteValue(new Position(8, 2), 1);

            referee._board.SetInterValue(new Position(1, 4), 1);
            referee._board.SetByteValue(new Position(1, 4), 1);
            referee._board.SetInterValue(new Position(2, 4), 1);
            referee._board.SetByteValue(new Position(2, 4), 1);
            referee._board.SetInterValue(new Position(3, 4), 1);
            referee._board.SetByteValue(new Position(3, 4), 1);
            referee._board.SetInterValue(new Position(1, 6), 1);
            referee._board.SetByteValue(new Position(1, 6), 1);
            referee._board.SetInterValue(new Position(2, 6), 1);
            referee._board.SetByteValue(new Position(2, 6), 1);
            referee._board.SetInterValue(new Position(4, 6), 1);
            referee._board.SetByteValue(new Position(4, 6), 1);

            referee._board.SetInterValue(new Position(6, 7), 2);
            referee._board.SetByteValue(new Position(6, 7), 2);
            referee._board.SetInterValue(new Position(6, 10), 2);
            referee._board.SetByteValue(new Position(6, 10), 2);
            referee._board.SetInterValue(new Position(9, 7), 2);
            referee._board.SetByteValue(new Position(9, 7), 2);
            referee._board.SetInterValue(new Position(12, 7), 2);
            referee._board.SetByteValue(new Position(12, 7), 2);
            referee._board.SetInterValue(new Position(12, 10), 2);
            referee._board.SetByteValue(new Position(12, 10), 2);
            referee._board.SetInterValue(new Position(7, 8), 1);
            referee._board.SetByteValue(new Position(7, 8), 1);
            referee._board.SetInterValue(new Position(7, 10), 1);
            referee._board.SetByteValue(new Position(7, 10), 1);
            referee._board.SetInterValue(new Position(8, 9), 1);
            referee._board.SetByteValue(new Position(8, 9), 1);
            referee._board.SetInterValue(new Position(8, 10), 1);
            referee._board.SetByteValue(new Position(8, 10), 1);
            referee._board.SetInterValue(new Position(9, 8), 1);
            referee._board.SetByteValue(new Position(9, 8), 1);
            referee._board.SetInterValue(new Position(9, 9), 1);
            referee._board.SetByteValue(new Position(9, 9), 1);
            referee._board.SetInterValue(new Position(10, 9), 1);
            referee._board.SetByteValue(new Position(10, 9), 1);
            referee._board.SetInterValue(new Position(10, 10), 1);
            referee._board.SetByteValue(new Position(10, 10), 1);
            referee._board.SetInterValue(new Position(11, 8), 1);
            referee._board.SetByteValue(new Position(11, 8), 1);
            referee._board.SetInterValue(new Position(11, 10), 1);
            referee._board.SetByteValue(new Position(11, 10), 1);
            */
            #endregion

            displayGrid(referee);
            while (true)
            {
                Debug.Log("Player " + referee._playerPlaying._numPlayer + " (" + referee._playerPlaying._nbToken + "/10 tokens)");
                if (referee._playerPlaying is HumanPlayer)
                {
                    Debug.Log("Enter coordonee with the format : \"x y\"");
                    String coord = System.Console.ReadLine();
                    char[] delimiters = { ' ' };
                    string[] words = coord.Split(delimiters);
                    Position newPos = new Position(Int32.Parse(words[0]), Int32.Parse(words[1]));
                    referee._lastPos = newPos;
                }
                referee.Update();
                if (referee._boardChanged)
                {
                    displayGrid(referee);
                    CheckVictory(referee);
                }
            }
        }

        public void Update()
        {
            Debug.Log("Player " + referee._playerPlaying._numPlayer + " (" + referee._playerPlaying._nbToken + "/10 tokens)");
            if (referee._playerPlaying is HumanPlayer)
            {
                Debug.Log("Enter coordonee with the format : \"x y\"");
                String coord = System.Console.ReadLine();
                char[] delimiters = { ' ' };
                string[] words = coord.Split(delimiters);
                Position newPos = new Position(Int32.Parse(words[0]), Int32.Parse(words[1]));
                referee._lastPos = newPos;
            }
            referee.Update();
            if (referee._boardChanged)
            {
                displayGrid(referee);
                CheckVictory(referee);
            }
        }

        public static void CheckVictory(Referee referee)
        {
            if (referee._winner != 0)
            {

            }
        }

        public static void displayGrid(Referee referee)
        {
            for (int y = 0; y < 19; y++)
            {
                for (int x = 0; x < 19; x++)
                {
                    System.Console.Write(referee._board.GetInterValue(new Position(x, y))._value + " ");
                }
            }
        }
    }
}
