using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombThrower : MonoBehaviour {

    public GameObject SpikeBombPrefab;
    [Space(10)]
    public float AttackDelay;
    public float BombTimer;
    public float ThrowPower;

    private SpikeBomb spikeBomb;   // instance

    const int MaxNumOfSpikeBomb = 4;

    FriendMover friendMover;
    new AudioSource audio;

    private float elaspedTime;

    private void Awake()
    {
        for (int i = 0; i < MaxNumOfSpikeBomb; ++i)
        {
            spikeBomb = Instantiate(SpikeBombPrefab).GetComponent<SpikeBomb>();
            spikeBomb.transform.parent = transform;
            spikeBomb.gameObject.SetActive(false);
        }

        friendMover = GetComponentInParent<FriendMover>();
        audio = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        elaspedTime = .0f;
    }

    private void Update()
    {
        if (friendMover.state != FriendMover.eState.Moving)
            return;

        elaspedTime += Time.deltaTime;

        if( AttackDelay <= elaspedTime )
        {
            if (transform.childCount == 0)
                return;

            Vector2 dir = (Vector2)transform.position - friendMover.CircleCenter;
            dir.Normalize();

            spikeBomb.gameObject.SetActive(true);
            spikeBomb.Ignite(dir * ThrowPower, BombTimer);

            audio.PlayDelayed(1.0f);
            elaspedTime = 0.0f;
        }
    }
}
