using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {

    public bool human;
    public bool isFinal;
    private int last_score;
    public GameObject gm;
    private Manager manager;
    private Text text;

	// Use this for initialization
	void Start () {
        manager = gm.GetComponent<Manager>();
        text = gameObject.GetComponent<Text>();
        text.text = 0.ToString();
        last_score = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!isFinal && manager.game_status == 1)
        {
            if (human && last_score != manager.score_j1)
            {
                text.text =  manager.score_j1.ToString();
                last_score = manager.score_j1;
            }
            if (!human && last_score != manager.score_j2)
            {
                text.text = manager.score_j2.ToString();
                last_score = manager.score_j2;
            }
        }
        if (isFinal && manager.game_status == 2)
        {
            if (human && last_score != manager.score_j1)
            {
                text.text = manager.score_j1.ToString();
                last_score = manager.score_j1;
            }
            if (!human && last_score != manager.score_j2)
            {
                text.text = manager.score_j2.ToString();
                last_score = manager.score_j2;
            }
        }
    }
}
