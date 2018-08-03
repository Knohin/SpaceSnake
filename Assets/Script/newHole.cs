using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newHole : MonoBehaviour {

    public GameObject Portal;
    
    void Start () {

        Portal.SetActive(false);

        StartCoroutine("SpawnPortal");
    }
	
	void Update () {

        if (Portal.gameObject.activeSelf)
        {
            Portal.transform.Rotate(0, 0, 3.0f);
        }
    }

    IEnumerator SpawnPortal()
    {
        yield return new WaitForSeconds(30.0f);

        while (true)
        {
            float x = Random.Range(-5.0f, 5.0f);
            float y = Random.Range(-9.0f, 9.0f);

            Portal.transform.position = new Vector3(x, y, 0);

            Portal.SetActive(true);
            yield return new WaitForSeconds(5.0f);
            Portal.SetActive(false);

            yield return new WaitForSeconds(15.0f);
        }

    }
}
