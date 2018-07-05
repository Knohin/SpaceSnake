using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewportManager : MonoBehaviour {

    public float Width;
    public float Height;

    [HideInInspector] public Vector2 upperLeft;
    [HideInInspector] public Vector2 bottomRight;

    [HideInInspector] public float Left;
    [HideInInspector] public float Right;
    [HideInInspector] public float Up;
    [HideInInspector] public float Down;

    private void Awake()
    {
        Left = -Width / 2.0f;
        Right = Width / 2.0f;
        Up = Height / 2.0f;
        Down = -Height / 2.0f;

        upperLeft = new Vector2(Left, Up);
        bottomRight = new Vector2(Right, Down);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 upperLeftCorner = new Vector3(-Width / 2.0f, Height / 2.0f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(upperLeftCorner, upperLeftCorner + Vector3.right * Width);
        Gizmos.DrawLine(upperLeftCorner + Vector3.right * Width, upperLeftCorner + Vector3.right * Width + Vector3.down * Height);
        Gizmos.DrawLine(upperLeftCorner, upperLeftCorner + Vector3.down * Height);
        Gizmos.DrawLine(upperLeftCorner + Vector3.down * Height, upperLeftCorner + Vector3.right * Width + Vector3.down * Height);
    }
}
