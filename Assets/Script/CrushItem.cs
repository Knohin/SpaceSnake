using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushItem : MonoBehaviour {
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Friend")) // item <-> player
        {
            if (other.gameObject.GetComponent<FriendMover>().state == FriendMover.eState.Moving)
            {
                for (int i = 0; i < newItem.itemList.Count; i++)
                {
                    if (newItem.itemList[i].equal_Stone(this.gameObject))
                    {
                        if (newItem.itemList[i].stone.name.Equals("crown(Clone)"))
                        {
                            //Debug.Log("Crown");
                            // start에 코루틴 키고 bool 로 조정?
                            Item_Effect.b_Crown = true;
                        }
                        else // bullet
                        {
                            //Debug.Log("Bullet");
                            Item_Effect.b_Bullet = true;
                        }
                        Item_Sound.item.Play();
                        newItem.itemList.RemoveAt(i);
                    }
                    Destroy(this.gameObject);
                }
            }
        }


    }
    
}
