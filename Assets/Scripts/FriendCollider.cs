using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendCollider : MonoBehaviour {

    private FriendsManager friendsManager;
    private FriendMover friendMover;

    private void Awake()
    {
        friendsManager = GameObject.Find("GameManager").GetComponent<FriendsManager>();
        friendMover = GetComponent<FriendMover>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Meteoroid")
        {
            Debug.Log("OnTriggerEnter() in FriendCollider");
            TextLog.Print("OnTriggerEnter() in FriendCollider");
        }
        else if (collision.tag == "Friend")
        {
            TextLog.Print("OnTriggerEnter() in FriendCollider with tag");
            if (friendMover.isMoving)
            {
                //TextLog.Print("디즴");
            }
            else
            {
                friendsManager.AttachFriend(gameObject);
            }
        }
    }
}
