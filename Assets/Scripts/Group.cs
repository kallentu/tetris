using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour {

    //Time since last gravity tick
    float lastFall = 0;


    //verifies each child block's position
    bool isValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = Grid.roundVec2(child.position);

            //not inside border?
            if (!Grid.insideBorder(v))
                return false;

            //block in grid cell (and not part of same group)?
            if (Grid.grid[(int)v.x, (int)v.y] != null &&
                Grid.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }

    //removes all old block positions from the grid and adds the new ones
    void updateGrid()
    {
        //removes old children
        for (int y = 0; y < Grid.h; ++y)
            for (int x = 0; x < Grid.w; ++x)
                if (Grid.grid[x, y] != null)
                    if (Grid.grid[x, y].parent == transform) // if the is a part of group
                        Grid.grid[x, y] = null;

        //add new children
        foreach (Transform child in transform)
        {
            Vector2 v = Grid.roundVec2(child.position);
            Grid.grid[(int)v.x, (int)v.y] = child;
        }
    }

    //int findBotRow ()
    //{
    //    foreach (Transform child in transform)
    //    {
    //        Vector2 v = Grid.roundVec2(child.position);

    //        for (int i = 0; i > (int)v.y; i++) //start at the the child, find if no space
    //        {
    //            foreach (Transform c in transform)
    //            {
    //                Vector2 p = Grid.roundVec2(c.position);
    //                if (Grid.grid[(int)p.x, i] == null)
    //                {
    //                    return i;
    //                }
    //            }

    //        }
    //    }

    //    return 0;

    //}


	// Use this for initialization
	void Start () {
		//Default pos not valid? Immediate game over.
        if (!isValidGridPos())
        {
            Debug.Log("Game Over.");
            Destroy(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
        //move left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //modify position
            transform.position += new Vector3(-1, 0, 0);

            //see if valid
            if (isValidGridPos())
                //update grid if valid
                updateGrid();
            else
                //invalid, revert
                transform.position += new Vector3(1, 0, 0);
        }
        //move right
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //modify position
            transform.position += new Vector3(1, 0, 0);

            //see if valid
            if (isValidGridPos())
                //update grid if valid
                updateGrid();
            else
                //invalid, revert
                transform.position += new Vector3(-1, 0, 0);
        }
        //rotate
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);

            //check validity
            if (isValidGridPos())
                //update grid if valid
                updateGrid();
            else
                //invalid, revert
                transform.Rotate(0, 0, 90);
        }
        // Move Downwards and Fall
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            // Modify position
            // transform.position += new Vector3(0, -1, 0);
            

            // See if valid
            if (isValidGridPos())
            {
                // transform.position += Vector3.down * 10 * Time.deltaTime;
                // It's valid. Update grid.
                updateGrid();
            }
            else {
                // It's not valid. revert.
                // transform.position += Vector3.up * 10 * Time.deltaTime;

                while (!isValidGridPos())
                {
                    transform.position += Vector3.up * (float)0.5 * Time.deltaTime;

                }

                // Clear filled horizontal lines
                Grid.deleteFullRows();

                // Spawn next Group
                FindObjectOfType<Spawner>().spawnNext();

                // Disable script
                enabled = false;
            }

            //makes block go down faster
            transform.position += Vector3.down * 5 * Time.deltaTime;

            lastFall = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {

            // See if valid
            if (isValidGridPos())
            {
                // It's valid. Update grid.
                updateGrid();
            }
            else {
                // Clear filled horizontal lines
                Grid.deleteFullRows();

                // Spawn next Group
                FindObjectOfType<Spawner>().spawnNext();

                // Disable script
                enabled = false;
            }

            //makes block hit the lowest possible space
            while (isValidGridPos())
            {
                updateGrid();
                transform.position += Vector3.down * (float)0.5 * Time.deltaTime;
            }

            lastFall = Time.time;
        }
        else if (Time.time - lastFall >= 1)
        {
            transform.position += new Vector3(0, -1, 0);

            // See if valid
            if (isValidGridPos())
            {
                // It's valid. Update grid.
                updateGrid();
            }
            else {
                // It's not valid. revert.
                // transform.position += new Vector3(0, 1, 0);
                while (!isValidGridPos())
                {
                    transform.position += Vector3.up * (float)0.5 * Time.deltaTime;
                }

                // Clear filled horizontal lines
                Grid.deleteFullRows();

                // Spawn next Group
                FindObjectOfType<Spawner>().spawnNext();

                // Disable script
                enabled = false;
            }

            lastFall = Time.time;
        }
        
    }

}
