﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsManager : MonoBehaviour {

    [HideInInspector] public List<GameObject> MovingFriends;
    [HideInInspector] public List<GameObject> FloatingFriends;
    public LinkedList<Vector2> changeDirectionPoint; // 메모리 파편화 가능성? 나중에 자료구조를 직접 만들든가 해야
    

    private void Awake()
    {
        MovingFriends = new List<GameObject>();
        FloatingFriends = new List<GameObject>();
        changeDirectionPoint = new LinkedList<Vector2>();

        GameObject f = GameObject.Find("StartFriend");
        f.GetComponent<FriendMover>().isMoving = true;
        MovingFriends.Add(f);
    }

    private void Update()
    {
        //////////////////
        // Get input
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) )
        {
            TextLog.Print("------------------Touch");
            // Change direction of a Head of Friends
            MovingFriends[MovingFriends.Count - 1].GetComponent<FriendMover>().ChangeDirection();

            // Add current position to changeDirectionPoint
            if (MovingFriends.Count > 1)
            {
                changeDirectionPoint.AddLast(MovingFriends[MovingFriends.Count - 1].transform.position);
            }

            // Set destination of the Tails
            for (int i = MovingFriends.Count - 2; i >= 0; i--)
            {
                if (MovingFriends[i].GetComponent<FriendMover>().destination != null)
                    break;
                MovingFriends[i].GetComponent<FriendMover>().destination = changeDirectionPoint.Last;
            }
        }
        
        ////////////////////
        // Move Friends
        for (int i = MovingFriends.Count - 1; i >= 0; i--)
        {
            MovingFriends[i].GetComponent<FriendMover>().Move();
        }

        //////////////////////
        //Remove all passed changeDirectionPoint for memory
        var dest = MovingFriends[0].GetComponent<FriendMover>().destination;
        while (dest != changeDirectionPoint.First)
                changeDirectionPoint.RemoveFirst();
    }

    public void AddFriend(GameObject newFriend)
    {
        // FloatingFriends 에서 찾고, 제거하고

        // MovingFriends에 넣고
        FriendMover head = MovingFriends[MovingFriends.Count - 1].GetComponent<FriendMover>();
        newFriend.transform.position = head.transform.position + head.dirVector * 0.45f;
        newFriend.GetComponent<FriendMover>().isMoving = true;
        newFriend.GetComponent<FriendMover>().Direction = head.Direction;
        MovingFriends.Add(newFriend);
    }
}
