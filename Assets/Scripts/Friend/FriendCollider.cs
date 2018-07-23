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
        if (collision.tag == "Friend")
        {
            if (friendMover.indexFromHead == 0) // 내가 머리인 경우
            {
                FriendMover cfm = collision.GetComponent<FriendMover>();
                if (4 < cfm.indexFromHead)
                {
                    TextLog.Print("다이");
                    collision.GetComponent<FriendCollider>().Die();
                }
                else if (cfm.isMoving == false)
                {
                    friendsManager.AttachFriend(collision.gameObject);
                }
            }
        }
        else if (collision.tag == "meteo") // 이부분은 MeteoObject에 CruchMeteo 에서 처리한다
        {
            if (friendMover.isMoving)
            {
                //TextLog.Print("OnTriggerEnter() in FriendCollider");
                collision.GetComponent<CrushMeteo>().Crush();
                if (FriendsManager.b_Crown)
                    return;
                else
                    Die();
            }
        }
    }

    public void Die()
    {
        ///한마리만 남았을떄
        if (friendsManager.MovingFriends.Count == 1)
        {
            for (int i = 0; i < newMeteo.meteoList.Count; i++)
            {
                newMeteo.meteoList[i].moveSpeed = 0.0f;
            }

            Time.timeScale = 0;
            friendsManager.score_Panel.SetActive(true);
        }
        ///두마리 이상
        else
        {
            /// 맞은애가 머리일떄
            if (friendMover.indexFromHead == 0) // 내가 머리인 경우
                friendsManager.DetachFriend(friendsManager.MovingFriends[friendsManager.MovingFriends.Count-2]);
            else
                friendsManager.DetachFriend(gameObject);
        }
        //gameObject.SetActive(false); // DetachFriend 에 포함되있음
    }
}
