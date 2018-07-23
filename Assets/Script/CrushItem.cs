using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushItem : MonoBehaviour {

    //private FriendsManager friendsManager;
    
    // Use this for initialization
    void Start () {

        //friendsManager = GameObject.Find("GameManager").GetComponent<FriendsManager>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Friend")) // item <-> player
        {
            if (other.gameObject.GetComponent<FriendMover>().isMoving)
            {
                for (int i = 0; i < newItem.itemList.Count; i++)
                {
                    if (newItem.itemList[i].equal_Stone(this.gameObject))
                    {
                        if (newItem.itemList[i].stone.name.Equals("crown(Clone)"))
                        {
                            //Debug.Log("Crown");
                            // start에 코루틴 키고 bool 로 조정?
                            FriendsManager.b_Crown = true;
                        }
                        else // bullet
                        {
                            //Debug.Log("Bullet");
                            FriendsManager.b_Bullet = true;
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
