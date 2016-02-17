using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.IO;

enum gameState
{
    START = 0,
    RUN = 1,
    END = 2
}

public class Pair<T, U>
{
    public Pair()
    {
    }

    public Pair(T first, U second)
    {
        this.First = first;
        this.Second = second;
    }

    public T First { get; set; }
    public U Second { get; set; }
};

public class Manager : MonoBehaviour
{
    public Gomoku.Referee   referee;
    public Gomoku.Board     board;
    public int              game_status;
    public bool             player_human;
    public int              score_j1;
    public int              score_j2;
    public bool             AI_on { get; set; }
    public bool             _isRuleBreakable { get; set; }
    public bool             _isRuleDoubleThree { get; set; }

    public GameObject       startMenu;
    public GameObject       mainUI;
    public GameObject       gameoverMenu;
    public GameObject       Tile;
    public GameObject       Wins_J1;
    public GameObject       Wins_J2;
    public GameObject       Wins_IA;
    public GameObject       giban;
    public Text             BreakText;
    public Text             ThreeText;

    /*
        private int taken_pawns_human;
        private int taken_pawns_ia;
        private float decal_x;
        private float decal_y;
        */
    // Use this for initialization
    void Start () {
        giban.SetActive(false);
        game_status = 0;
        score_j1 = 0;
        score_j2 = 0;
        player_human = true;
        AI_on = false;
        _isRuleBreakable = true;
        _isRuleDoubleThree = true;
        ThreeText.text = "TA MERE".ToString();
    }
	

	// Update is called once per frame
	void Update ()
    {
        if (game_status == 0)
        {
            if (_isRuleBreakable && BreakText.text != "Break ON")
                BreakText.text = "Break ON";
            else if (!_isRuleBreakable && BreakText.text != "Break OFF")
                BreakText.text = "Break OFF";
            if (_isRuleDoubleThree && ThreeText.text != "Double Three ON")
                ThreeText.text = "Double Three ON";
            else if (!_isRuleDoubleThree && ThreeText.text != "Double Three OFF")
                ThreeText.text = "Double Three OFF";
        }
        else if (game_status == 1)
        {
            if (AI_on && !player_human)
            {referee.Update();}
            if (referee._winner > 0)
                this.GameOver();
        }
    }

    public void StartGame(bool AI)
    {
        giban.SetActive(true);
        AI_on = AI;
        startMenu.SetActive(false);
        mainUI.SetActive(true);
        referee = new Gomoku.Referee(_isRuleBreakable, _isRuleDoubleThree, AI_on);
        board = referee._board;
        game_status = 1;
    }

    void GameOver()
    {
        game_status = 2;
        mainUI.SetActive(false);
        gameoverMenu.SetActive(true);
        if (referee._winner == 1)
            Wins_J1.SetActive(true);
        else if (referee._winner == 2 && player_human)
            Wins_J2.SetActive(true);
        else
            Wins_IA.SetActive(true);
    }

    public void Restart()
    {SceneManager.LoadScene("Giban");}

    public void Quit()
    { Application.Quit(); }

    public void Pass()
    {player_human = !player_human;}

    public void ButtonBreak()
    { _isRuleBreakable = !_isRuleBreakable; }

    public void ButtonThree()
    { _isRuleDoubleThree = !_isRuleDoubleThree; }

    public void takePawn(bool human)
    {
        if (human)
        { score_j2 += 1; }
        else
        { score_j1 += 1; }
    }
}
