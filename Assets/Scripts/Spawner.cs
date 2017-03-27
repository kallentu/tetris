using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {

    public GameObject[] blocks;
    private int[] blockSpace = { 0, 0, 0, 0, 0, 0, 0}; //which blocks have been spawned
    public Text row;
    public static int score = 0;

    //resets the availibility of the blocks that are spawned
    void resetSpaces()
    {
        for (int i = 0; i < blockSpace.Length; i++)
        {
            blockSpace[i] = 0;
        }
    }

    public void spawnNext() {
        //randomizes index to choose which block to pick
        int index = Random.Range(0, blocks.Length);
        while (blockSpace[index] == 1) // guarantees no repeats at least
        {
            for (int i = 0; i < blocks.Length; i++) //goes through every single holder in array to check for available block to spawn
            {
                if (blockSpace[i] == 0)
                {
                    index = i;
                    break;
                }
            }

            if (blockSpace[0] == 1 && //all blocks have been put down
                blockSpace[1] == 1 &&
                blockSpace[2] == 1 &&
                blockSpace[3] == 1 &&
                blockSpace[4] == 1 &&
                blockSpace[5] == 1 &&
                blockSpace[6] == 1)
            {
                resetSpaces();        //so we reset
            }
        }
        
        if (blockSpace[index] == 0) //block is immediately available, change availibility for afterwards
        {
            blockSpace[index] = 1; 
        }


        Instantiate(blocks[index],
                    transform.position, //Spawner's location
                    Quaternion.identity);   //default rotation

    }
	// Use this for initialization
	void Start () {
        row.text = "Rows : " + score;
        spawnNext();
	}
	
	// Update is called once per frame
	void Update () {
        row.text = "Rows : " + score;
    }
}
