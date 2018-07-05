using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Sound : MonoBehaviour {

    public static AudioSource item;

	// Use this for initialization
	void Start () {
        item = GetComponent<AudioSource>();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
