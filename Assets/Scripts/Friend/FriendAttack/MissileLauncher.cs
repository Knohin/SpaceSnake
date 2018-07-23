using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour {

    public GameObject MissilePrefab;
    [Space(10)]
    public float MissileSpeed;
    public float MissileDegreeDelta;
    public float ShotDelay;

    GameObject missile;

    FriendMover friendMover;

    float elapsedTime;

    private void Awake()
    {
        missile = Instantiate(MissilePrefab);
        missile.SetActive(false);
        missile.transform.parent = transform;

        friendMover = GetComponentInParent<FriendMover>();
        elapsedTime = .0f;
    }

    private void Update()
    {
        if (!friendMover.isMoving)
            return;

        if (ShotDelay <= elapsedTime)
        {
            StartCoroutine(Launch());
            elapsedTime = .0f;
        }

        if (missile.activeSelf == false)
            elapsedTime += Time.deltaTime;
    }

    IEnumerator Launch()
    {
        // Initiate the missile
        missile.transform.position = transform.position;
        missile.GetComponent<HomingMissile>().Speed = MissileSpeed;
        missile.GetComponent<HomingMissile>().MaxDegreeDelta = MissileDegreeDelta;
        // Active the missile
        missile.SetActive(true);

        yield return null;
    }

}
