using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour {

    //The grid itself
    public static int w = 10;
    public static int h = 20;

    //use transform instead of gameobject so that we can just check position directly
    public static Transform[,] grid = new Transform[w, h];

    public static int score = 0;

    //makes sure that when we rotate, the vector stays at a rounded number
    public static Vector2 roundVec2(Vector2 v) {
        return new Vector2(Mathf.Round(v.x),
                           Mathf.Round(v.y));
    }

    //make sure the block is inside the borders
    public static bool insideBorder(Vector2 pos) {
        return ((int)pos.x >= 0 &&
                (int)pos.x < w &&
                (int)pos.y >= 0.5);
    }

    //deletes all blocks in a certain row
    //param y   the row we want deleted
    public static void deleteRow(int y) {
        for (int x = 0; x < w; ++x) {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    //when deleted, row above falls down
    public static void decreaseRow(int y) {
        for (int x = 0; x < w; ++x) {
            if (grid[x, y] != null)
            {
                //Move one towards bottom
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                //Updates block position in the game world
                grid[x, y - 1].position += new Vector3(0, -1, 0); //decrease y by 1
            }
        }
    }

    //uses decreaseRow 
    //pushes ALL rows down
    public static void decreaseRowsAbove(int y) {
        //loops through all rows above to push them down
        for (int i = y; i < h; ++i)
            decreaseRow(i);
    }

    //when row is full of blocks
    public static bool isRowFull(int y) {
        for (int x = 0; x < w; ++x)
            if (grid[x, y] == null)
                return false;
        return true;
    }

    //deletes row if full of blocks, moves all downwards
    public static void deleteFullRows() {
        for (int y = 0; y < h; ++y)
        {
            if (isRowFull(y))
            {
                deleteRow(y);
                Spawner.score++;
                decreaseRowsAbove(y+1);
                --y; //decrease because we want to check the row that drops down
            }
        }
    }

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	}
}
