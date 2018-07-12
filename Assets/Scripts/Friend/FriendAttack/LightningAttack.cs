using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    newMeteo meteoManager;

    FriendMover friendMover;

    void Awake ()
    {
        activeBoltsObj = new List<GameObject>();
        inactiveBoltsObj = new List<GameObject>();
        
        meteoManager = GameObject.Find("GameManager").GetComponent<newMeteo>();

        friendMover = transform.GetComponentInParent<FriendMover>();

        for (int i = 0; i < maxBolts; i++)
        {
            GameObject bolt = (GameObject)Instantiate(BoltPrefab);
            
            bolt.transform.parent = transform;

            // Initialize our lightning with a preset number of max segments
            bolt.GetComponent<LightningBolt>().Initialize(10);
            bolt.SetActive(false);

            // Store in our inactive list
            inactiveBoltsObj.Add(bolt);
        }
    }
    private void Start()
    {
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
    private void OnDisable()
    {
        GameObject boltObj;
        // Deactivate all Ligthning Bolts
        for (int i = activeBoltsObj.Count - 1; i >= 0; i--)
        {
            boltObj = activeBoltsObj[i];
            boltObj.GetComponent<LightningBolt>().DeactivateSegments();

            boltObj.SetActive(false);
            activeBoltsObj.RemoveAt(i);
            inactiveBoltsObj.Add(boltObj);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

    IEnumerator AttackNearestWithBolt()
    {
        while (true)
        {
            if (friendMover.isMoving)
            {
                float nearestSqr = Mathf.Infinity;
                MainObject nearestMeteo = null;
                // Find nearby meteo
                foreach (MainObject meteo in newMeteo.meteoList)
                {
                    float distanceSqr = (meteo.ballPos - transform.position).sqrMagnitude;
                    if (distanceSqr < nearestSqr)
                    {
                        nearestSqr = distanceSqr;
                        nearestMeteo = meteo;
                    }
                }

                // If the nearest meteo is in the attack range,
                Vector3 AttackPoint = nearestMeteo.stone.transform.position;
                if ((transform.position - AttackPoint).sqrMagnitude <= AttackRange * AttackRange)
                {
                    // Create a (pooled) bolt to nearest meteo
                    for (int i = 0; i < NumberOfBoltLine; i++)
                        CreatePooledBolt(transform.position, AttackPoint, Color.white, Thickness);
                    nearestMeteo.stone.GetComponent<CrushMeteo>().Crush();
                    yield return new WaitForSeconds(AttackDelayTime);
                }
            }
            yield return null;
        }

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
