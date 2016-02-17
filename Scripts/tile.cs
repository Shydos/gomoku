using UnityEngine;
using System.Collections;
using System;

public class tile : MonoBehaviour {

    public GameObject white_stone;
    public GameObject black_stone;
    private Manager manager;
    private GameObject stone;
    private bool stone_set;
    private float x;
    private float z;
    private float y;
    public int x_id = 0;
    public int y_id = 0;
    private int stone_color;
    public Gomoku.Position coord;

	// Use this for initialization
	void Start () {
        manager = GameObject.Find("GameManager").GetComponent<Manager>();
        x = gameObject.transform.position.x;
        z = gameObject.transform.position.z;
        y = gameObject.transform.position.y;
        coord = new Gomoku.Position(x_id, y_id);
        stone_set = false;
        stone_color = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (manager.game_status > 0)
        {
            if (stone_set && manager.board._interBoard[x_id, y_id]._value == 0)
            {
                if (stone_color == 1)
                { manager.takePawn(true); }
                else if (stone_color == 2)
                { manager.takePawn(false); }
                Destroy(stone);
                stone_color = 0;
                stone_set = false;
            }
            else if (!stone_set)
            {
                if (manager.board._interBoard[x_id, y_id]._value == 1)
                {
                    stone = Instantiate(white_stone, new Vector3(x, 0.200f, z - 0.2f), Quaternion.identity) as GameObject;
                    manager.Pass();
                    stone.transform.parent = gameObject.transform;
                    stone_color = 1;
                    stone_set = true;
                }
                else if (manager.board._interBoard[x_id, y_id]._value == 2)
                {
                    stone = Instantiate(black_stone, new Vector3(x, 0.200f, z - 0.2f), Quaternion.identity) as GameObject;
                    manager.Pass();
                    stone.transform.parent = gameObject.transform;
                    stone_color = 2;
                    stone_set = true;
                }
            }
        }
	}

    void OnTouchDown()
    {
        if (manager.game_status == 1)
        {
            if (!stone_set)
            {
                manager.referee._lastPos = coord;
                manager.referee.Update();
            }
        }
    }

    void OnTouchUp ()
    {
    
    }

    void OnTouchStay ()
    {

    }

    public static implicit operator GameObject(tile v)
    {
        throw new NotImplementedException();
    }
}
