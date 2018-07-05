using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushItem : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Friend")) // item <-> player
        {
            for (int i = 0; i < newItem.itemList.Count; i++)
            {
                if (newItem.itemList[i].equal_Stone(this.gameObject))
                {
                    if (newItem.itemList[i].stone.name.Equals("bomb(Clone)"))
                    {
                        Debug.Log("Bomb");
                        newMeteo.period -= 0.05f;

                        for(int k = 0; k < newMeteo.meteoList.Count/2; k++)
                        {
                            Destroy(newMeteo.meteoList[k].stone);
                            newMeteo.meteoList[k].stone = null;
                            newMeteo.meteoList.RemoveAt(k);
                        }

                    }
                    else if (newItem.itemList[i].stone.name.Equals("crown(Clone)"))
                    {
                        Debug.Log("Crown");
                    }
                    else // bullet
                    {
                        Debug.Log("Bullet");
                    }
                    Item_Sound.item.Play();
                    newItem.itemList.RemoveAt(i);
                }
                Destroy(this.gameObject);
            }
        }
    }
    
}
