using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Barrier : MonoBehaviour
{
    public float Delay;

    FriendMover friendMover;
    SpriteRenderer barrierImage;
    CircleCollider2D barrierCollider;

    float elapsedTime;
    bool isTurnedOn;
    
    private void Awake()
    {
        friendMover = GetComponentInParent<FriendMover>();
        barrierImage = GetComponent<SpriteRenderer>();
        barrierImage.enabled = false;
        barrierCollider = GetComponent<CircleCollider2D>();
        barrierCollider.enabled = false;
        
        elapsedTime = .0f;
        isTurnedOn = false;
    }

    private void Update()
    {
        if (!friendMover.isMoving)
            return;

        if (!isTurnedOn)
        {
            if (Delay < elapsedTime)
            {
                barrierImage.enabled = true;
                barrierCollider.enabled = true;
                isTurnedOn = true;
            }
            elapsedTime += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "meteo")
        {
            collision.GetComponent<CrushMeteo>().Crush();

            barrierImage.enabled = false;
            barrierCollider.enabled = false;
            isTurnedOn = false;
            elapsedTime = .0f;
        }
    }
}
