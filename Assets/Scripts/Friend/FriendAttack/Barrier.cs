using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public float Delay;
    public float BarrierRadius;

    SpriteRenderer barrierImage;
    new AudioSource audio;

    FriendMover friendMover;

    float elapsedTime;
    bool isTurnedOn;
    
    private void Awake()
    {
        friendMover = GetComponentInParent<FriendMover>();
        barrierImage = GetComponent<SpriteRenderer>();
        barrierImage.enabled = false;

        audio = GetComponent<AudioSource>();
        
        isTurnedOn = false;
    }
    private void OnEnable()
    {
        elapsedTime = .0f;
    }
    private void OnDisable()
    {
        barrierImage.enabled = false;
        isTurnedOn = false;
    }

    private void Update()
    {
        if (friendMover.state != FriendMover.eState.Moving)
        {
            barrierImage.enabled = false;
            isTurnedOn = false;

            return;
        }

        if (isTurnedOn)
        {
            MainObject nearestMeteo = newMeteo.FindNearestMeteo(transform.position, BarrierRadius);

            if (nearestMeteo != null)
            {
                nearestMeteo.stone.GetComponent<CrushMeteo>().Crush();

                barrierImage.enabled = false;
                isTurnedOn = false;
            }
        }
        else
        {
            if (Delay < elapsedTime)
            {
                barrierImage.enabled = true;
                isTurnedOn = true;
                audio.Play();
                elapsedTime = .0f;
            }
            elapsedTime += Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, BarrierRadius);
    }
}
