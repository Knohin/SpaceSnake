using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBullet : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "meteo")
        {
            collision.GetComponent<CrushMeteo>().Crush();
            gameObject.SetActive(false);
        }
    }
}
