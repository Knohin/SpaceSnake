using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject BulletPrefab;
    [Space(10)]
    public float BulletSpeed;
    public float ShotDelay;
    public float Thickness;

    static private int BulletNum = 6;

    GameObject[] bullets;
    Vector3[] bulletsDirection;
    FriendMover friendMover;

    float elapsedTime;

    private void Awake()
    {
        bullets = new GameObject[BulletNum];
        bulletsDirection = new Vector3[BulletNum];
        Quaternion rot;
        for(int i=0; i < BulletNum; ++i)
        {
            rot = Quaternion.Euler(.0f, .0f, i * (360.0f/ BulletNum));
            bullets[i] = Instantiate(BulletPrefab, Vector3.zero, rot);
            bullets[i].transform.parent = transform;
            bullets[i].SetActive(false);

            bulletsDirection[i] = rot * Vector3.right;
        }

        friendMover = GetComponentInParent<FriendMover>();
        elapsedTime = .0f;

        SetBulletColor(Color.yellow);
    }

    private void Update()
    {
        if (!friendMover.isMoving)
            return;

        if (ShotDelay <= elapsedTime)
        {
            StartCoroutine(Fire());
            elapsedTime = .0f;
        }
        elapsedTime += Time.deltaTime;
    }

    public void SetBulletColor(Color color)
    {
        for(int i=0; i<BulletNum; ++i)
            bullets[i].GetComponent<SpriteRenderer>().color = color;
    }

    IEnumerator Fire()
    {
        Transform parent = transform;

        float bulletLength = BulletSpeed * (100 / bullets[0].GetComponent<SpriteRenderer>().sprite.rect.width);
        float pixel2Unit = bullets[0].GetComponent<SpriteRenderer>().sprite.pixelsPerUnit / bullets[0].GetComponent<SpriteRenderer>().sprite.rect.width;
        for (int i = 0; i < BulletNum; ++i)
        {
            // Set position
            bullets[i].transform.localPosition = new Vector3(0, 0, bullets[i].transform.localPosition.z) + bulletsDirection[i]* BulletSpeed / 2.0f;
            // Set scale
            bullets[i].transform.localScale = new Vector3(BulletSpeed * pixel2Unit,
                                                         Thickness,
                                                         bullets[i].transform.localScale.z);
            // Active
            bullets[i].SetActive(true);
            // Detach from Parent
            bullets[i].transform.parent = null;
        }

        const float MaxRange = 20f;
        float distanceMoved = .0f;
        float actualSpeed = BulletSpeed * 0.7f; // 너무 빠르면 중간중간 떨어져있는 느낌?
        while (distanceMoved < MaxRange)
        {
            // Move
            for (int i=0; i< BulletNum; ++i)
            {
                bullets[i].transform.position += bulletsDirection[i] * actualSpeed;
            }

            distanceMoved += actualSpeed;
            yield return null;
        }

        // Inactive all
        for (int i = 0; i < BulletNum; ++i)
        {
            bullets[i].transform.parent = parent;
            bullets[i].SetActive(false);
        }
    }
}
