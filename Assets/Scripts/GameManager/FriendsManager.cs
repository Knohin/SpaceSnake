using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsManager : MonoBehaviour {

    [Header("private 인데 일단")]
    // For pooling
    public List<FriendMover> MovingFriends; // 꼬리 -> 머리 순서
    public List<FriendMover> FloatingFriends;
    public List<FriendMover> InActiveFriends;
    public LinkedList<Vector2> changeDirectionPoint; // 메모리 파편화 가능성? 나중에 자료구조를 직접 만들든가 해야

    [Space(10)]
    public float MovingSpeedAtStart;
    public float DistanceBetweenFriends;
    [Space(10)]
    public GameObject[] FriendsPrefab;

    public GameObject Crown;
    public GameObject score_Panel;
    public static bool b_Crown;
    public static bool b_Bullet;

    bool isTouching = false;

    private void Awake()
    {
        MovingFriends = new List<FriendMover>();
        FloatingFriends = new List<FriendMover>();
        InActiveFriends = new List<FriendMover>();
        changeDirectionPoint = new LinkedList<Vector2>();

        Transform friendPoolHolder = GameObject.Find("FriendPoolHolder").transform;
        if (friendPoolHolder == null)
            friendPoolHolder = (new GameObject("FriendPoolHolder")).transform;

        // Set start Charater
        GameObject f = GameObject.Find("StartFriend");
        f.transform.parent = friendPoolHolder;
        FriendMover fm = f.GetComponent<FriendMover>();
        //fm.isMoving = true;
        fm.state = FriendMover.eState.Moving;
        fm.indexFromHead = 0;
        fm.Speed = MovingSpeedAtStart;
        fm.CircleCenter = (Vector2)transform.position + Vector2.right * fm.Radius;
        
        MovingFriends.Add(fm);

        // Pre-make objects to pool
        int NumberOfFriends = FriendsPrefab.Length;
        for (int i = 50 / NumberOfFriends; 0 < i; i--)
        {
            for(int j = NumberOfFriends-1; 0 <= j; j--)
            {
                GameObject friend = Instantiate(FriendsPrefab[j]);
                friend.transform.parent = friendPoolHolder;
                friend.SetActive(false);
                InActiveFriends.Add(friend.GetComponent<FriendMover>());
            }
        }

        //Crown
        Crown.SetActive(false);

        InvokeRepeating("SpawnFriends", 1.5f, 5.0f);
    }
    private void OnDisable()
    {
        if (b_Crown == true)
        {
            b_Crown = false;
            Crown.SetActive(false);
        }
    }

    private void Update()
    {
        //////////////////
        // Get input
        if (0 < Input.touchCount)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                TextLog.Print("-Touch at frame " + Time.frameCount);
                if (!isTouching)
                    ChangeDirection();

                isTouching = true;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isTouching = false;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            ChangeDirection();
        }

        UpdateFriends();
    }
    void UpdateFriends()
    {
        ////////////////////
        // Check friend's state
        for (int i = MovingFriends.Count - 1; i >= 0; i--)
        {
            if(MovingFriends[i].state == FriendMover.eState.Dead)
            {
                TextLog.Print("Dead:)l");
                MovingFriends.RemoveAt(i);
                InActiveFriends.Add(MovingFriends[i]);
            }
        }

        //////////////////////
        // Remove the all-passed changeDirectionPoint
        var dest = MovingFriends[0].destination;
        while (dest != changeDirectionPoint.First)
            changeDirectionPoint.RemoveFirst();

        if (b_Crown)
            StartCoroutine("Crown_Effect");

        if (b_Bullet)
            StartCoroutine("Bullet_Effect");
    }
    void SpawnFriends()
    {
        if (InActiveFriends.Count <= 0)
            return;

        float x = Random.Range(-5.5f, 5.5f);
        float y = Random.Range(-9.5f, 9.5f);

        int cha = Random.Range(0, InActiveFriends.Count - 1); //InActiveFriends.Count - 3;
        FriendMover newFriend = InActiveFriends[cha];

        InActiveFriends.RemoveAt(cha);

        newFriend.gameObject.SetActive(true);
        newFriend.transform.position = new Vector3(x, y);
        newFriend.state = FriendMover.eState.Floating;
        FloatingFriends.Add(newFriend);
    }

    void ChangeDirection()
    {
        // Change direction of a Head of Friends
        MovingFriends[MovingFriends.Count - 1].ChangeDirection();

        // Add current position to changeDirectionPoint
        if (MovingFriends.Count > 1)
        {
            changeDirectionPoint.AddLast(MovingFriends[MovingFriends.Count - 1].transform.position);
        }

        // Set destination of the Tails
        for (int i = MovingFriends.Count - 2; i >= 0; i--)
        {
            if (MovingFriends[i].destination != null)
                break;
            MovingFriends[i].destination = changeDirectionPoint.Last;
        }
    }

    public void AttachFriend(FriendMover newFriend)
    {
        // Scoring
        ScoringManager.save_Score += 100;

        // FloatingFriends 에서 찾고, 제거하고
        FloatingFriends.Remove(newFriend);

        // MovingFriends에 넣고
        FriendMover head = MovingFriends[MovingFriends.Count - 1];

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
        newFriend.transform.rotation = Quaternion.identity;
        FriendMover newFriendMover = newFriend.GetComponent<FriendMover>();
        newFriendMover.Speed = head.Speed;
        newFriendMover.state = FriendMover.eState.Moving;
        newFriendMover.indexFromHead = 0;
        newFriendMover.CircleCenter = head.CircleCenter;
        newFriendMover.IsClockwise = head.IsClockwise;
        MovingFriends.Add(newFriendMover);

        for(int i = MovingFriends.Count-2; 0 <= i; --i)
        {
            MovingFriends[i].indexFromHead++;
        }
    }

    public void AttackedAt(FriendMover attackedFriend)
    {
        // 한마리만 남았을떄
        if (MovingFriends.Count == 1)
        {
            GameObject.Find("FriendPoolHolder").SetActive(false);
            score_Panel.SetActive(true);
            GameObject.Find("GameManager").SetActive(false);
        }
        // 두마리 이상
        else
        {
            int idx = -1;
            if (attackedFriend.indexFromHead == 0) // 내가 머리인 경우
                idx = MovingFriends.Count - 2;
            else
                idx = MovingFriends.IndexOf(attackedFriend);

            for (int i = idx; 0 <= i; --i)
            {
                MovingFriends[i].Die();
                //StartCoroutine(WaitForDying(MovingFriends[i]));
            }
            for (int i = idx+1; i < MovingFriends.Count; ++i)
                MovingFriends[i].Flicker(3.0f);
            //MovingFriends.RemoveRange(0, idx + 1);

        }
    }

    IEnumerator Crown_Effect()
    {
        Vector3 nv = MovingFriends[MovingFriends.Count - 1].transform.position + new Vector3(-0.1f, 1.15f, 0f);

        Crown.transform.position = nv;

        if (!Crown.activeSelf)
        {
            TextLog.Print("Crown");
            Crown.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            b_Crown = false;
            Crown.SetActive(false);
        }
    }

    IEnumerator Bullet_Effect()
    {
        // 총알 주기 or 파워
        yield return new WaitForSeconds(3.0f);
        b_Bullet = false;
    }
}