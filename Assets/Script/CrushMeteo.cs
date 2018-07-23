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
        if(other.gameObject.tag.Equals("Friend")) // meteo <-> player
        {
            //Crush();
        }
    }

    public void Crush()
    {
        eff.Boom(gameObject.transform.position);
        meteoManager.RemoveMeteo(gameObject);

    }
}
