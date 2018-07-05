using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushMeteo : MonoBehaviour {

    public GameObject eff;

	// Use this for initialization
	void Start () {
        eff.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}
    
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag.Equals("meteo")) // meteo <-> meteo
        {
            //Debug.Log("Meteo");

            for(int i = 0; i < newMeteo.meteoList.Count; i++)
            {

                if (newMeteo.meteoList[i].equal_Stone(other.gameObject))
                {
                    //newMeteo.meteoList[i].moveValue.x *= -1;
                    //newMeteo.meteoList[i].moveValue.y *= -1;
                    //newMeteo.meteoList[i].moveSpeed += 0.005f;
                    //newMeteo.meteoList.RemoveAt(i);
                }
                else if (newMeteo.meteoList[i].equal_Stone(this.gameObject))
                {
                    //newMeteo.meteoList[i].moveValue.x *= -1;
                    //newMeteo.meteoList[i].moveValue.y *= -1;
                    //newMeteo.meteoList[i].moveSpeed += 0.005f;
                    //newMeteo.meteoList.RemoveAt(i);
                }
            }

            //Destroy(other.gameObject);
            //Destroy(this.gameObject);


        }
        else if(other.gameObject.tag.Equals("Friend")) // meteo <-> player
        {
            //Debug.Log("Player");
            for(int i = 0; i< newMeteo.meteoList.Count; i++)
            {
                if(newMeteo.meteoList[i].equal_Stone(this.gameObject))
                {
                    newMeteo.meteoList.RemoveAt(i);
                    StartCoroutine("meteo_Effect", this.gameObject.transform.position);
                    break;
                }
            }
        }
       
    }

    IEnumerator meteo_Effect(Vector3 pos)
    {
        eff.gameObject.SetActive(true);
        eff.transform.position = new Vector3(pos.x, pos.y, 0);
        yield return new WaitForSeconds(0.1f);
        eff.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    /*
    void OnCollisionEnter2D(Collision2D other) // editphotosforfree.com/photoapps/remove-background-from-image-online
    {
        
    }
    */
}
