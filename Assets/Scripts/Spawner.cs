using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject[] blocks;


    public void spawnNext() {
        //randomizes index to choose which block to pick
        int index = Random.Range(0, blocks.Length);
        int last = -1;                                                   // start out of bounds
        last = (index == last) ? Random.Range(0, blocks.Length) : index; // guarantees no repeats at least

        Instantiate(blocks[last],
                    transform.position, //Spawner's location
                    Quaternion.identity);   //default rotation

    }
	// Use this for initialization
	void Start () {
        spawnNext();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
