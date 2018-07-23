using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendMover : MonoBehaviour
{
    [HideInInspector] public float Speed;

    [HideInInspector] public LinkedListNode<Vector2> destination = null;
    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public int indexFromHead = -1; // 이거 넣으면 사실상 위에 있는 변수(isMoving)는 필요가 없음

    private bool isClockwise = true;
    public bool IsClockwise { get { return isClockwise; } set { isClockwise = value; } }
    private const float radius = 1.0f;
    public float Radius { get { return radius; } }
    private Vector2 circleCenter;
    public Vector2 CircleCenter { get { return circleCenter; } set { circleCenter = value; } }

    //private ViewportManager viewport;
    
    private void Awake()
    {
        //viewport = GameObject.Find("GameManager").GetComponent<ViewportManager>();
    }

    private void OnEnable()
    {
        //circleCenter = (Vector2)transform.position + Vector2.right * radius;
    }

    private void OnDisable()
    {
        destination = null;
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