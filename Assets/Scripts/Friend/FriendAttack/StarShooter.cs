using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarShooter : MonoBehaviour {

    public GameObject StarPrefab;
    [Space(10)]
    public float ShotDelay;
    public float ShotSpeed;

    const int MaxNumOfStar = 4;

    private FriendMover friendMover;
    private new AudioSource audio;

    private float elaspedTime;

    private void Awake()
    {
        for (int i = 0; i < MaxNumOfStar; ++i)
        {
            GameObject star = Instantiate(StarPrefab);
            star.transform.parent = transform;
            star.SetActive(false);
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

        if (ShotDelay <= elaspedTime)
        {
            // Check the pool is Empty
            if (transform.childCount == 0)
                return;

            Vector3 dir = (Vector2)transform.position - friendMover.CircleCenter;
            dir.Normalize();

            GameObject star = transform.GetChild(0).gameObject;
            star.SetActive(true);
            star.GetComponent<Star>().Shot(dir, ShotSpeed, 5.0f);

            audio.Play();

            elaspedTime = 0;
        }
    }

}
