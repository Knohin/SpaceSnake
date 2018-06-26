using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendMover : MonoBehaviour
{

    public float Speed;

    bool isClockWise = false;
    [HideInInspector] public LinkedListNode<Vector2> destination = null;
    [HideInInspector] public bool isMoving = false;

    public Vector3 dirVector;
    private int direction; // 0,1,2,3 각각 상,우,하,좌
    public int Direction
    {
        set
        {
            direction = value;
            if (direction > 3) direction = 0;
            if (direction < 0) direction = 3;
            switch (direction)
            {
                case 0: dirVector = Vector3.up; break;
                case 1: dirVector = Vector3.right; break;
                case 2: dirVector = Vector3.down; break;
                case 3: dirVector = Vector3.left; break;
                default: dirVector = Vector3.zero; break;
            }
        }
        get { return direction; }
    }

    private void OnEnable()
    {
        Direction = 0;
    }

    private void OnDisable()
    {
        destination = null;
    }
    
    public void ChangeDirection()
    {
        // isClockWise = !isClockWise;
        Direction++;
    }

    public void Move()
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

    public void MoveCircular(/**/)
    {
        //
    }
}