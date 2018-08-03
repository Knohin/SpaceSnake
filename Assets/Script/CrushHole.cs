using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushHole : MonoBehaviour {

    public static Vector3 hole_Pos;

	void Start () {
        hole_Pos = Vector3.one;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag.Equals("meteo"))
        {
            for(int i = 0; i < newMeteo.meteoList.Count; i++)
            {
                if (newMeteo.meteoList[i].equal_Stone(other.gameObject))
                {
                    newMeteo.meteoList[i].b_Blackhole = true;
                    hole_Pos = this.gameObject.transform.position;
                }
            }
        }
    }
}
