using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public GameObject LaserPrefab;
    [Space(10)]
    public float ShotDelay;
    public float Thickness;
    public float FadeOutValue;

    const int LaserNum = 4;

    GameObject[] lasers;
    Vector3[] lasersDirection;
    Color _laserColor;
    Color laserColor {
        get { return _laserColor; }
        set {
            _laserColor = value;
            for (int i = 0; i < LaserNum; ++i)
                lasers[i].GetComponent<SpriteRenderer>().color = _laserColor; ;
        }
    }

    FriendMover friendMover;
    new AudioSource audio;

    float elapsedTime;

    private void Awake()
    {
        lasers = new GameObject[LaserNum];
        lasersDirection = new Vector3[LaserNum];
        Quaternion rot;
        for (int i = 0; i < LaserNum; ++i)
        {
            rot = Quaternion.Euler(.0f, .0f, i * (360.0f / LaserNum));
            lasers[i] = Instantiate(LaserPrefab, Vector3.zero, rot);
            lasers[i].transform.parent = transform;
            lasers[i].SetActive(false);

            lasersDirection[i] = rot * Vector3.right;
        }

        friendMover = GetComponentInParent<FriendMover>();
        audio = GetComponent<AudioSource>();

        elapsedTime = .0f;

        laserColor = new Color(77.0f / 255, 255.0f / 255, 184.0f / 255);
        //SetLaserColor(new Color(77.0f/255, 255.0f/255, 184.0f/255));
    }
    private void OnEnable()
    {
        elapsedTime = .0f;
    }

    private void Update()
    {
        if (friendMover.state != FriendMover.eState.Moving)
            return;

        if (ShotDelay <= elapsedTime)
        {
            StartCoroutine(Fire());
            audio.Play();
            elapsedTime = .0f;
        }
        elapsedTime += Time.deltaTime;
    }

    public void SetLaserColor(Color color)
    {
        for (int i = 0; i < LaserNum; ++i)
            lasers[i].GetComponent<SpriteRenderer>().color = color;
    }

    IEnumerator Fire()
    {
        Transform parent = transform;

        float laserLength = 20.0f;
        float pixel2Unit = lasers[0].GetComponent<SpriteRenderer>().sprite.pixelsPerUnit / lasers[0].GetComponent<SpriteRenderer>().sprite.rect.width;
        for (int i = 0; i < LaserNum; ++i)
        {
            // Set position
            lasers[i].transform.localPosition = new Vector3(0, 0, lasers[i].transform.localPosition.z) +lasersDirection[i] * laserLength / 2.0f;
            // Set scale
            lasers[i].transform.localScale = new Vector3(laserLength * pixel2Unit,
                                                         Thickness,
                                                         lasers[i].transform.localScale.z);
            // Active
            lasers[i].SetActive(true);
            lasers[i].transform.parent = null;
        }

        float laserAlpha = 1.0f;
        while (.0f < laserAlpha)
        {
            // lower alpha
            laserColor = new Color(laserColor.r, laserColor.g, laserColor.b, laserAlpha);

            laserAlpha -= FadeOutValue * Time.deltaTime;
            yield return null;
        }

        // Inactive all
        for (int i = 0; i < LaserNum; ++i)
        {
            lasers[i].SetActive(false);
            lasers[i].transform.parent = parent;
        }
    }
}
