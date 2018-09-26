using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendMover : MonoBehaviour
{
    [HideInInspector] public float Speed;

    [HideInInspector] public LinkedListNode<Vector2> destination = null;
    [HideInInspector] public int indexFromHead = -1; // 이거 넣으면 사실상 위에 있는 변수(isMoving)는 필요가 없음
    [HideInInspector] public int health = 1;

    public enum eState
    {
        Floating,
        Moving,
        Dying,
        Dead
    }
    public eState state = eState.Dead;

    private bool isClockwise = true;
    public  bool IsClockwise { get { return isClockwise; } set { isClockwise = value; } }
    private const float radius = 1.0f;
    public  float Radius { get { return radius; } }
    private Vector2 circleCenter;
    public  Vector2 CircleCenter { get { return circleCenter; } set { circleCenter = value; } }

    bool isWounded = false;
    const float Pad = 0.5f; // Pad for outline
    FriendsManager friendsManager;
    
    private void Awake()
    {
        friendsManager = GameObject.Find("GameManager").GetComponent<FriendsManager>();
    }
    private void OnEnable()
    {
        if (gameObject.name == "Con 1(Clone)")
            health = 2;
        else
            health = 1;
    }

    private void OnDisable()
    {
        destination = null;
        indexFromHead = -1;
        isWounded = false;
    }

    private void Update()
    {
        switch(state)
        {
            case eState.Floating:
                transform.Rotate(0, 0, -0.5f);
                break;
            case eState.Moving:
                MoveCircular();
                break;
            case eState.Dying:
                break;
            case eState.Dead:
                break;
        }
    }
    public void ChangeDirection()
    {
        isClockwise = !isClockwise;
        circleCenter += 2.0f * ((Vector2)transform.position - circleCenter);
    }

    public void MoveCircular()
    {
        float thetaToMove = Speed / radius * Time.deltaTime; // 한 프레임동안 이동할 각도

        float thetaToDest;
        while (destination != null)
        {
            Vector2 centerToPos = (Vector2)transform.position - circleCenter;
            Vector2 centerToDest = destination.Value - circleCenter;
            thetaToDest = (isClockwise) ? Vector2.Angle(centerToPos, centerToDest) : Vector2.Angle(centerToDest, centerToPos);
            thetaToDest *= Mathf.PI / 180.0f;
            if ( thetaToDest < 0 )
            {
                thetaToDest += 2.0f * Mathf.PI;
            }
            
            if (thetaToMove < thetaToDest)
            {
                break;
            }

            transform.position = destination.Value;
            ChangeDirection();
            thetaToMove -= thetaToDest;

            destination = destination.Next;
        }
        if (isClockwise)
            thetaToMove *= -1.0f;
        Vector2 centerToCurrent = (Vector2)transform.position - circleCenter;
        Vector2 centerToTarget = new Vector2(
            Mathf.Cos(thetaToMove) * centerToCurrent.x - Mathf.Sin(thetaToMove) * centerToCurrent.y,
            Mathf.Sin(thetaToMove) * centerToCurrent.x + Mathf.Cos(thetaToMove) * centerToCurrent.y
        );
        transform.position = circleCenter + centerToTarget;

        // Check if it is out of Screen 
        if (indexFromHead == 0 && !isWounded)
        {
            if (transform.position.x < ViewportManager.Left - Pad
                || transform.position.x > ViewportManager.Right + Pad
                || transform.position.y < ViewportManager.Down - Pad
                || transform.position.y > ViewportManager.Up + Pad)
            {
                friendsManager.AttackedAt(this);
            }
        }

        return;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Friend"
            && indexFromHead == 0)
        {
             FriendMover cfm = collision.GetComponent<FriendMover>();
            if (cfm.state == eState.Floating)
            {
                friendsManager.AttachFriend(cfm);
            }
            else if (6 < cfm.indexFromHead)
            {
                friendsManager.AttackedAt(cfm);
                //collision.GetComponent<FriendMover>().Die();
            }
        }
        else if (collision.tag == "meteo")
        {
            if (state == eState.Moving && !isWounded)
            {
                collision.GetComponent<CrushMeteo>().Crush();
                if (Item_Effect.b_Crown)
                    return;
                else
                    //Die();
                    friendsManager.AttackedAt(this);
            }
        }
    }

    public void Die()
    {
        StartCoroutine(DyingAnimation());
        return;
    }
    IEnumerator DyingAnimation()
    {
        SpriteRenderer thisSprite = GetComponent<SpriteRenderer>();
        Color spriteColor= thisSprite.color;
        Vector3 Gravity = new Vector3(0.0f, -10.0f);

        float randomX = Random.Range(-1f, 1f);
        Vector3 velocity = new Vector3(randomX, 4.0f);
        Quaternion angularVel = Quaternion.Euler(0, 0, randomX*2);

        state = eState.Dying;

        float alpha = 1.0f;
        while(0 < alpha)
        {
            transform.position += velocity * Time.deltaTime;
            velocity += Gravity * Time.deltaTime;

            transform.rotation *= angularVel;

            alpha -= 1.0f * Time.deltaTime;
            spriteColor.a = alpha;
            thisSprite.color = spriteColor;

            yield return null;
        }
        spriteColor.a = 1.0f;
        thisSprite.color = spriteColor;

        state = eState.Dead;

        gameObject.SetActive(false);
        yield return null;
    }

    public void Flicker(float sec)
    {
        StartCoroutine(FlickeringForSeconds(sec));
        return;
    }
    IEnumerator FlickeringForSeconds(float sec)
    {
        float remainTime = sec;
        SpriteRenderer thisSprite = GetComponent<SpriteRenderer>();
        Color spriteColor = thisSprite.color;

        float alpha = 1.0f;
        float delta = -0.1f;

        isWounded = true;
        while (0 < remainTime)
        {
            if (alpha < 0.3f)
            {
                alpha = 0.3f;
                delta *= -1;
            }
            else if (1f < alpha)
            {
                alpha = 1;
                delta *= -1;
            }

            alpha += delta;
            spriteColor.a = alpha;
            thisSprite.color = spriteColor;

            yield return null;
            remainTime -= Time.deltaTime;
        }

        spriteColor.a = 1.0f;
        thisSprite.color = spriteColor;

        isWounded = false;
        yield return null;
    }

    //private void Die2()
    //{
    //    // 한마리만 남았을떄
    //    if (friendsManager.MovingFriends.Count == 1)
    //    {
    //        GameObject.Find("GameManager").SetActive(false);
    //        GameObject.Find("FriendPoolHolder").SetActive(false);
    //        friendsManager.score_Panel.SetActive(true);
    //    }
    //    // 두마리 이상
    //    else
    //    {
    //        if (indexFromHead == 0) // 내가 머리인 경우
    //            friendsManager.DetachFriend(friendsManager.MovingFriends[friendsManager.MovingFriends.Count - 2].gameObject);
    //        else
    //            friendsManager.DetachFriend(gameObject);
    //    }
    //    //gameObject.SetActive(false); // DetachFriend 에 포함되있음
    //}

}