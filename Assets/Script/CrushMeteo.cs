using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushMeteo : MonoBehaviour {

    public Explosion eff;

    static newMeteo meteoManager;

    void Start () {
        meteoManager = GameObject.Find("GameManager").GetComponent<newMeteo>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("meteo")) // meteo <-> meteo
        {
            //for(int i = 0; i < newMeteo.meteoList.Count; i++)
            //{
            //    if (newMeteo.meteoList[i].equal_Stone(other.gameObject))
            //    {
            //        newMeteo.meteoList[i].moveValue.x *= -1;
            //        newMeteo.meteoList[i].moveValue.y *= -1;
            //        newMeteo.meteoList[i].moveSpeed += 0.005f;
            //        newMeteo.meteoList.RemoveAt(i);
            //    }
            //    else if (newMeteo.meteoList[i].equal_Stone(this.gameObject))
            //    {
            //        newMeteo.meteoList[i].moveValue.x *= -1;
            //        newMeteo.meteoList[i].moveValue.y *= -1;
            //        newMeteo.meteoList[i].moveSpeed += 0.005f;
            //        newMeteo.meteoList.RemoveAt(i);
            //    }
            //}
            //Destroy(other.gameObject);
            //Destroy(this.gameObject);
        }
        else if(other.gameObject.tag.Equals("Friend")) // meteo <-> player
        {
            Crush();
        }
    }

    public void Crush()
    {
        eff.Boom(gameObject.transform.position);
        meteoManager.RemoveMeteo(gameObject);

    }
}
