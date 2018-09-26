using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CrushMagnet : MonoBehaviour
{

    public GameObject Black_Hole;
    public static bool active_Hole;

    private Renderer rd;
    private Collider2D co; 

    // Use this for initialization
    void Start()
    {
        rd = GetComponent<SpriteRenderer>();
        co = GetComponent<CircleCollider2D>();

        Black_Hole.SetActive(false);
        active_Hole = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Black_Hole.gameObject.activeSelf)
        {
            Black_Hole.gameObject.transform.Rotate(0, 0, -3.0f);
        }
    }
    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Friend")) // item <-> player
        {
            if (other.gameObject.GetComponent<FriendMover>().state == FriendMover.eState.Moving)
            {
                for (int i = 0; i < newItem.itemList.Count; i++)
                {
                    if (newItem.itemList[i].equal_Stone(this.gameObject))
                    {
                        Item_Sound.item.Play();
                        newItem.itemList.RemoveAt(i);
                        yield return StartCoroutine("SpawnHole");

                        break;
                    }
                }
            }

            
        }

    }

    IEnumerator SpawnHole()
    {
        rd.enabled = false;
        co.enabled = false;

        float x = Random.Range(-5.0f, 5.0f);
        float y = Random.Range(-9.0f, 9.0f);

        Black_Hole.transform.position = new Vector3(x, y, 0);

        Black_Hole.SetActive(true);
        active_Hole = true;

        yield return new WaitForSeconds(3.5f);
        Black_Hole.SetActive(false);
        active_Hole = false;

        Destroy(this.gameObject);

    }
}
