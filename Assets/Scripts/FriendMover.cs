using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendMover : MonoBehaviour
{
    [HideInInspector] public float Speed;

    [HideInInspector] public LinkedListNode<Vector2> destination = null;
    [HideInInspector] public bool isMoving = false;

    private bool isClockwise = true;
    public bool IsClockwise { get { return isClockwise; } set { isClockwise = value; } }
    private const float radius = 0.5f;
    public float Radius { get { return radius; } }
    private Vector2 circleCenter;
    public Vector2 CircleCenter { get { return circleCenter; } set { circleCenter = value; } }

    private ViewportManager viewport;

    // 4방향 직진용 변수들
    //public Vector3 dirVector;
    //private int direction; // 0,1,2,3 각각 상,우,하,좌
    //public int Direction
    //{
    //    set
    //    {
    //        direction = value;
    //        if (direction > 3) direction = 0;
    //        if (direction < 0) direction = 3;
    //        switch (direction)
    //        {
    //            case 0: dirVector = Vector3.up; break;
    //            case 1: dirVector = Vector3.right; break;
    //            case 2: dirVector = Vector3.down; break;
    //            case 3: dirVector = Vector3.left; break;
    //            default: dirVector = Vector3.zero; break;
    //        }
    //    }
    //    get { return direction; }
    //}

    private void Awake()
    {
        viewport = GameObject.Find("GameManager").GetComponent<ViewportManager>();
    }

    private void OnEnable()
    {
        //Direction = 0;
        ///////
        circleCenter = (Vector2)transform.position + Vector2.right * radius;
    }

    private void OnDisable()
    {
        destination = null;
    }

    public void ChangeDirection()
    {
        //Direction++;
        /////////
        isClockwise = !isClockwise;
        circleCenter += 2.0f * ((Vector2)transform.position - circleCenter);
    }

    /*public void MoveStraight()
    {
        float movement = Speed * Time.deltaTime; // 한 프레임동안 이동할 거리

        while (movement > 0.0f)
        {
            if (destination == null)
            {
                transform.position += dirVector * movement;
                return;
            }

            float distance = Vector2.Distance(transform.position, destination.Value);
            transform.position = Vector3.MoveTowards(transform.position, destination.Value, movement);
            movement -= distance;

            if (movement > 0.0f)
            {
                // 방향 전환
                ChangeDirection();
                // 다음 목표점 설정
                destination = destination.Next;
            }
        }
    }
    */

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

        /*
        if (transform.position.x < viewport.Left)
            transform.position += Vector3.right * viewport.Width;
        else if (transform.position.x > viewport.Right)
            transform.position += Vector3.left * viewport.Width;

        if (transform.position.y > viewport.Up)
            transform.position += Vector3.down * viewport.Height;
        else if (transform.position.y < viewport.Down)
            transform.position += Vector3.up * viewport.Height;
        */

        return;
    }
    
}