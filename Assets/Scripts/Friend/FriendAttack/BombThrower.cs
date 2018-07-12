using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FriendMover))]
public class BombThrower : MonoBehaviour {

    public GameObject SpikeBombPrefab;
    [Space(10)]
    public float AttackDelay;
    public float BombTimer;
    public float ThrowPower;

    private SpikeBomb spikeBomb;   // instance
    private FriendMover friendMover;
    private float elaspedTime;

    private void Awake()
    {
        friendMover = GetComponent<FriendMover>();
        spikeBomb = Instantiate(SpikeBombPrefab).GetComponent<SpikeBomb>();
        spikeBomb.transform.parent = transform;
        spikeBomb.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        elaspedTime = .0f;
    }

    private void Update()
    {
        if (!friendMover.isMoving)
            return;

        elaspedTime += Time.deltaTime;

        if( AttackDelay <= elaspedTime )
        {
            elaspedTime -= AttackDelay;

            if(!spikeBomb.isActiveAndEnabled)
                ThrowObject(); 
        }
    }

    void ThrowObject()
    { 
        Vector2 dir = (Vector2)transform.position - friendMover.CircleCenter;
        dir.Normalize();
        
        spikeBomb.gameObject.SetActive(true);
        spikeBomb.Ignite(dir * ThrowPower, BombTimer);
    }
}
