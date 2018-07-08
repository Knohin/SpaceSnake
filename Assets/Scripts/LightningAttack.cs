using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class LightningAttack : MonoBehaviour {

    // Prefabs to be assigned in Editor
    public GameObject BoltPrefab;
    public float AttackDelayTime;
    public float AttackRange;
    public float Thickness;
    public int NumberOfBoltLine;

    // For pooling
    List<GameObject> activeBoltsObj;
    List<GameObject> inactiveBoltsObj;
    int maxBolts = 20;

    // For finding the nearest meteo
    List<GameObject> nearbyMeteors;

    void Start ()
    {
        activeBoltsObj = new List<GameObject>();
        inactiveBoltsObj = new List<GameObject>();
        nearbyMeteors = new List<GameObject>();

        Transform lightningPoolHolder = transform.Find("LightningPoolHolder");

        GetComponent<CircleCollider2D>().radius = AttackRange;

        for (int i = 0; i < maxBolts; i++)
        {
            GameObject bolt = (GameObject)Instantiate(BoltPrefab);
            
            bolt.transform.parent = lightningPoolHolder;

            // Initialize our lightning with a preset number of max segments
            bolt.GetComponent<LightningBolt>().Initialize(10);
            bolt.SetActive(false);

            // Store in our inactive list
            inactiveBoltsObj.Add(bolt);
        }

        StartCoroutine("AttackNearestWithBolt");
    }
	
	void Update ()
    {
        GameObject boltObj;
        LightningBolt boltComponent;

        // Loop through active lines (backwards because we'll be removing from the list)
        for (int i = activeBoltsObj.Count - 1; i >= 0; i--)
        {
            boltObj = activeBoltsObj[i];
            boltComponent = boltObj.GetComponent<LightningBolt>();

            // If the bolt has faded out
            if (boltComponent.IsComplete)
            {
                // Deactive the segments it contains
                boltComponent.DeactivateSegments();

                boltObj.SetActive(false);
                activeBoltsObj.RemoveAt(i);
                inactiveBoltsObj.Add(boltObj);
            }
        }

        // Update and draw active bolts
        for (int i = 0; i < activeBoltsObj.Count; i++)
        {
            activeBoltsObj[i].GetComponent<LightningBolt>().UpdateBolt();
            activeBoltsObj[i].GetComponent<LightningBolt>().Draw();
        }
        
	}

    IEnumerator AttackNearestWithBolt()
    {
        while (true)
        {
            // Create a (pooled) bolt to nearest meteo
            if (nearbyMeteors.Count > 0)
            {
                float nearestSqr = Mathf.Infinity;
                GameObject nearestMeteo = null;
                // Find nearby meteo
                foreach (GameObject meteo in nearbyMeteors)
                {
                    float distanceSqr = (meteo.transform.position - transform.position).sqrMagnitude;
                    if (distanceSqr < nearestSqr)
                    {
                        nearestSqr = distanceSqr;
                        nearestMeteo = meteo;
                    }
                }

                for (int i = 0; i < NumberOfBoltLine; i++)
                    CreatePooledBolt(transform.position, nearestMeteo.transform.position, Color.white, Thickness);

                yield return new WaitForSeconds(AttackDelayTime);
            }
            yield return null;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("meteo"))
            nearbyMeteors.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("meteo"))
            nearbyMeteors.Remove(collision.gameObject);
    }


    void CreatePooledBolt(Vector2 source, Vector2 dest, Color color, float thickness)
    {
        //if there is an inactive bolt to pull from the pool
        if (inactiveBoltsObj.Count > 0)
        {
            //pull the GameObject
            GameObject boltObj = inactiveBoltsObj[inactiveBoltsObj.Count - 1];

            //move it to the active list
            boltObj.SetActive(true);
            activeBoltsObj.Add(boltObj);
            inactiveBoltsObj.RemoveAt(inactiveBoltsObj.Count - 1);

            //activate the bolt using the given position data
            LightningBolt lb = boltObj.GetComponent<LightningBolt>();
            lb.ActivateBolt(source, dest, color, thickness);
            lb.Draw();
        }
    }
}
