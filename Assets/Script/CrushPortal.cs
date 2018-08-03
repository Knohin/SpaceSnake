using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushPortal : MonoBehaviour {

    public GameObject eff;
    
	void Start () {
        eff.gameObject.SetActive(false);
	}
	

    IEnumerator light_Effect(Vector3 pos)
    {
        eff.gameObject.SetActive(true);
        eff.transform.position = new Vector3(pos.x, pos.y, 0);
        yield return new WaitForSeconds(0.03f);
        eff.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag.Equals("meteo"))
        {
            for(int i = 0; i < newMeteo.meteoList.Count; i++)
            {
                if (newMeteo.meteoList[i].equal_Stone(other.gameObject))
                {
                    float x = Random.Range(-5.0f, 5.0f);
                    float y = Random.Range(-10f, 10f);
                    StartCoroutine("light_Effect", this.gameObject.transform.position);

                    newMeteo.meteoList[i].ballPos = new Vector3(x, y, 0);
                }
            }
        }
    }
}
