using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsManager : MonoBehaviour {

    [Header("private 인데 일단")]
    // For pooling
    public List<GameObject> MovingFriends; // 꼬리 -> 머리 순서
    public List<GameObject> FloatingFriends;
    public List<GameObject> InActiveFriends;
    public LinkedList<Vector2> changeDirectionPoint; // 메모리 파편화 가능성? 나중에 자료구조를 직접 만들든가 해야

    [Space(10)]
    public float MovingSpeedAtStart;
    public float DistanceBetweenFriends;
    [Space(10)]
    public GameObject[] FriendsPrefab;

    private void Awake()
    {
        MovingFriends = new List<GameObject>();
        FloatingFriends = new List<GameObject>();
        InActiveFriends = new List<GameObject>();
        changeDirectionPoint = new LinkedList<Vector2>();

        Transform friendPoolHolder = GameObject.Find("FriendPoolHolder").transform;
        if (friendPoolHolder == null)
            friendPoolHolder = (new GameObject("FriendPoolHolder")).transform;

        // Set start Charater
        GameObject f = GameObject.Find("StartFriend");
        f.transform.parent = friendPoolHolder;
        FriendMover fm = f.GetComponent<FriendMover>();
        fm.isMoving = true;
        fm.Speed = MovingSpeedAtStart;
        fm.CircleCenter = (Vector2)transform.position + Vector2.right * fm.Radius;

        MovingFriends.Add(f);

        // Pre-make objects to pool
        int NumberOfFriends = FriendsPrefab.Length;
        for (int i = 50 / NumberOfFriends; 0 < i; i--)
        {
            for(int j = NumberOfFriends-1; 0 <= j; j--)
            {
                GameObject friend = Instantiate(FriendsPrefab[j]);
                friend.transform.parent = friendPoolHolder;
                friend.SetActive(false);
                InActiveFriends.Add(friend);
            }
        }

        InvokeRepeating("SpawnFriends", 1.5f, 5);
    }

    private void Update()
    {
        //////////////////
        // Get input
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began))
#elif UNITY_WEBGL
        if (Input.GetMouseButtonDown(0))
            TextLog.Print("-Click at frame " + Time.frameCount);
        if ((Input.touchCount == 1) && (Input.GetTouch(0).phase == TouchPhase.Began))
#endif
        {
            TextLog.Print("-Touch at frame " + Time.frameCount);
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
            MovingFriends[i].GetComponent<FriendMover>().MoveCircular();
        }

        //////////////////////
        // Remove the all-passed changeDirectionPoint
        var dest = MovingFriends[0].GetComponent<FriendMover>().destination;
        while (dest != changeDirectionPoint.First)
            changeDirectionPoint.RemoveFirst();

    }

    void SpawnFriends()
    {
        if (InActiveFriends.Count <= 0)
            return;

        float x = Random.Range(-5.5f, 5.5f);
        float y = Random.Range(-9.5f, 9.5f);

        int cha = InActiveFriends.Count - 3;// Random.Range(0, InActiveFriends.Count-1);
        GameObject newFriend = InActiveFriends[cha];

        InActiveFriends.RemoveAt(cha);

        newFriend.SetActive(true);
        newFriend.transform.position = new Vector3(x, y);
        FloatingFriends.Add(newFriend);
    }

    public void AttachFriend(GameObject newFriend)
    {
        // FloatingFriends 에서 찾고, 제거하고
        FloatingFriends.Remove(newFriend);

        // MovingFriends에 넣고
        FriendMover head = MovingFriends[MovingFriends.Count - 1].GetComponent<FriendMover>();

        float thetaAhead = DistanceBetweenFriends / head.Radius;
        if(head.IsClockwise)
        {
            thetaAhead *= -1.0f;
        }

        Vector2 centerToCurrent = (Vector2)head.transform.position - head.CircleCenter;
        Vector2 centerToTarget = new Vector2(
            Mathf.Cos(thetaAhead) * centerToCurrent.x - Mathf.Sin(thetaAhead) * centerToCurrent.y,
            Mathf.Sin(thetaAhead) * centerToCurrent.x + Mathf.Cos(thetaAhead) * centerToCurrent.y
        );
        newFriend.transform.position = head.CircleCenter + centerToTarget;
        newFriend.GetComponent<FriendMover>().Speed = head.Speed;
        newFriend.GetComponent<FriendMover>().isMoving = true;
        newFriend.GetComponent<FriendMover>().CircleCenter = head.CircleCenter;
        newFriend.GetComponent<FriendMover>().IsClockwise = head.IsClockwise;
        MovingFriends.Add(newFriend);
    }

    public void DetachFriend(GameObject oldFriend)
    {
        int idx = MovingFriends.IndexOf(oldFriend);
        for(int i=idx; 0 <= i; --i)
        {
            MovingFriends[i].SetActive(false);
            InActiveFriends.Add(MovingFriends[i]);
        }
        MovingFriends.RemoveRange(0, idx + 1);
    }
}