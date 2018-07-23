using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newHole : MonoBehaviour {

    public GameObject Portal;
    //public GameObject Hole;

    //public static bool active_Hole;


    // Use this for initialization
    void Start () {

        Portal.SetActive(false);
        //Hole.gameObject.SetActive(false);
        //active_Hole = false;

        StartCoroutine("SpawnPortal");
        //StartCoroutine("SpawnHole");

    }
	
	// Update is called once per frame
	void Update () {
        /*
        if (Hole.gameObject.activeSelf)
        {
            Hole.gameObject.transform.Rotate(0, 0, 3.0f);
            active_Hole = Hole.gameObject.activeSelf;
        }
        else
        {
            active_Hole = Hole.gameObject.activeSelf;
        }
        */

        if (Portal.gameObject.activeSelf)
        {
            Portal.transform.Rotate(0, 0, 3.0f);
        }

    }

    IEnumerator SpawnPortal()
    {
        yield return new WaitForSeconds(10.0f);

        while (true)
        {
            float x = Random.Range(-5.0f, 5.0f);
            float y = Random.Range(-9.0f, 9.0f);

            Portal.transform.position = new Vector3(x, y, 0);

            Portal.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            Portal.SetActive(false);

            yield return new WaitForSeconds(10.0f);
        }

    }
    /*
    IEnumerator SpawnHole()
    {
        yield return new WaitForSeconds(10.0f);

        while (true)
        {
            float x = Random.Range(-5.0f, 5.0f);
            float y = Random.Range(-9.0f, 9.0f);

            Hole.gameObject.transform.position = new Vector3(x, y, 0);

            Hole.gameObject.SetActive(true);

            yield return new WaitForSeconds(5.0f);

            Hole.gameObject.SetActive(false);

            yield return new WaitForSeconds(5.0f);

        }

    }
    */
}
