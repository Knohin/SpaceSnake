using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour {

    public GameObject MissilePrefab;
    [Space(10)]
    public float MissileSpeed;
    public float MissileDegreeDelta;
    public float ShotDelay;

    const float MaxNumOfMissile = 3;

    FriendMover friendMover;
    new AudioSource audio;

    float elapsedTime;

    private void Awake()
    {
        for (int i = 0; i < MaxNumOfMissile; ++i)
        {
            GameObject missile = Instantiate(MissilePrefab);
            missile.transform.parent = transform;
            missile.SetActive(false);
        }

        friendMover = GetComponentInParent<FriendMover>();
        audio = GetComponent<AudioSource>();

        elapsedTime = .0f;
    }

    private void Update()
    {
        if (friendMover.state != FriendMover.eState.Moving)
            return;

        elapsedTime += Time.deltaTime;

        if (ShotDelay <= elapsedTime)
        {
            // Check the pool is Empty
            if (transform.childCount == 0)
                return;

            GameObject missile = transform.GetChild(0).gameObject;

            // Initiate the missile
            missile.transform.position = transform.position;
            missile.GetComponent<HomingMissile>().Speed = MissileSpeed;
            missile.GetComponent<HomingMissile>().MaxDegreeDelta = MissileDegreeDelta;
            // Active the missile
            missile.SetActive(true);

            audio.Play();

            elapsedTime = .0f;
        }
    }
}
