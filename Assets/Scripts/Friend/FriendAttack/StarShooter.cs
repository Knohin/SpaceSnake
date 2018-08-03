using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarShooter : MonoBehaviour {

    public GameObject StarPrefab;
    [Space(10)]
    public float ShotDelay;
    public float ShotSpeed;

    const int MaxNumOfStar = 3;
    List<GameObject> stars;

    private FriendMover friendMover;
    private float elaspedTime;

    private void Awake()
    {
        stars = new List<GameObject>(4);
        for (int i = 0; i < MaxNumOfStar; ++i)
        {
            GameObject star = Instantiate(StarPrefab);
            star.transform.parent = transform;
            star.SetActive(false);
            stars.Add(star);
        }

        friendMover = GetComponentInParent<FriendMover>();
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
            GameObject remainStar = null;
            for(int i=0; i< MaxNumOfStar; i++)
            {
                if(stars[i].activeSelf == false)
                {
                    remainStar = stars[i];
                    break;
                }
            }
            if (remainStar == null)
                return;

            StartCoroutine(Shot(remainStar));
            elaspedTime = 0;
        }
    }

    IEnumerator Shot(GameObject star)
    {
        star.transform.parent = null;
        star.transform.position = transform.position;
        star.SetActive(true);

        Vector3 dir = (Vector2)transform.position - friendMover.CircleCenter;
        dir.Normalize();

        float moved = 0.0f;
        while (moved < 9.0f)
        {
            star.transform.position += dir * ShotSpeed * Time.deltaTime;

            moved += ShotSpeed * Time.deltaTime;
            yield return null;
        }

        star.SetActive(false);
        star.transform.parent = transform;
        yield return null;
    }
}
