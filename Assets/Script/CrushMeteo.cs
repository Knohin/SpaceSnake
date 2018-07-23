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
        //if(other.gameObject.tag.Equals("Friend")) // meteo <-> player
        //{
        //    if (other.GetComponent<FriendMover>().isMoving)
        //    {
        //        other.GetComponent<FriendCollider>().Die();
        //        Crush();
        //    }
        //}
    }

    public void Crush()
    {
        MainObject meteo = newMeteo.FindMeteo(gameObject);
        if (meteo == null)
            TextLog.Print("a??????????");

        ScoringManager.meteo_Score += 100;
        /// health 줄이고
        meteo.health--;

        /// 0 이면
        if (meteo.health < 1)
        {
            eff.Boom(gameObject.transform.position);
            meteoManager.RemoveMeteo(gameObject);
        }
    }

    IEnumerator meteo_Effect(Vector3 pos)
    {
        eff.transform.position = new Vector3(pos.x, pos.y, 0);
        eff.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        eff.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
