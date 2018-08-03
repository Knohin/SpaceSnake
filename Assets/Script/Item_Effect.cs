using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Effect : MonoBehaviour {

    private FriendsManager fm;

    public GameObject Crown;
    public GameObject Power;

    public static bool b_Crown;
    public static bool b_Bullet;

    public static Coroutine co_Crown;

    // Use this for initialization
    void Start () {

        fm = GameObject.Find("GameManager").GetComponent<FriendsManager>();

        Crown.SetActive(false);
        Power.SetActive(false);
    }

    void Update()
    {

        if (b_Crown)
            StartCoroutine("Crown_Effect");

        if (b_Bullet)
            StartCoroutine("Bullet_Effect");
    }

    private void OnDisable()
    {
        if (b_Crown == true)
        {
            b_Crown = false;
            Crown.SetActive(false);
        }
        if (b_Bullet == true)
        {
            b_Bullet = false;
            Power.SetActive(false);
        }
    }
    void Power_Up()
    {
        for (int i = 0; i < fm.MovingFriends.Count; i++)
        {

            if (fm.MovingFriends[i].name == "StartFriend" || fm.MovingFriends[i].name == "Frodo(Clone)")
            {
                Gun gun = fm.MovingFriends[i].GetComponentInChildren<Gun>();
                gun.ShotDelay = 1.0f;
            }
            else if (fm.MovingFriends[i].name == "Tube(Clone)")
            {
                Barrier bar = fm.MovingFriends[i].GetComponentInChildren<Barrier>();
                bar.Delay = 1.0f;
            }
            else if (fm.MovingFriends[i].name == "Ryan(Clone)")
            {
                LaserGun lg = fm.MovingFriends[i].GetComponentInChildren<LaserGun>();
                lg.Thickness = 4.0f;
            }
            else if (fm.MovingFriends[i].name == "Neo(Clone)")
            {
                MissileLauncher ml = fm.MovingFriends[i].GetComponentInChildren<MissileLauncher>();
                ml.MissileSpeed = 8.0f;
            }
            else if (fm.MovingFriends[i].name == "Muzi(Clone)")
            {
                BombThrower bt = fm.MovingFriends[i].GetComponentInChildren<BombThrower>();
                bt.AttackDelay = 1.0f;
            }
            else if (fm.MovingFriends[i].name == "JayG(Clone)")
            {
                LightningAttack la = fm.MovingFriends[i].GetComponentInChildren<LightningAttack>();
                la.AttackRange = 5.0f;
                la.AttackDelayTime = 0.5f;
            }
            else if (fm.MovingFriends[i].name == "Peach(Clone)")
            {
                StarShooter ss = fm.MovingFriends[i].GetComponentInChildren<StarShooter>();
                ss.ShotDelay = 1.0f;
            }
            else // Con
            {

            }
        }
    }

    void Power_Down()
    {
        for (int i = 0; i < fm.MovingFriends.Count; i++)
        {

            if (fm.MovingFriends[i].name == "StartFriend" || fm.MovingFriends[i].name == "Frodo(Clone)")
            {
                Gun gun = fm.MovingFriends[i].GetComponentInChildren<Gun>();
                gun.ShotDelay = 2.0f;

            }
            else if (fm.MovingFriends[i].name == "Tube(Clone)")
            {
                Barrier bar = fm.MovingFriends[i].GetComponentInChildren<Barrier>();
                bar.Delay = 2.0f;
            }
            else if (fm.MovingFriends[i].name == "Ryan(Clone)")
            {
                LaserGun lg = fm.MovingFriends[i].GetComponentInChildren<LaserGun>();
                lg.Thickness = 2.0f;
            }
            else if (fm.MovingFriends[i].name == "Neo(Clone)")
            {
                MissileLauncher ml = fm.MovingFriends[i].GetComponentInChildren<MissileLauncher>();
                ml.MissileSpeed = 4.0f;
            }
            else if (fm.MovingFriends[i].name == "Muzi(Clone)")
            {
                BombThrower bt = fm.MovingFriends[i].GetComponentInChildren<BombThrower>();
                bt.AttackDelay = 2.0f;
            }
            else if (fm.MovingFriends[i].name == "JayG(Clone)")
            {
                LightningAttack la = fm.MovingFriends[i].GetComponentInChildren<LightningAttack>();
                la.AttackRange = 3.0f;
                la.AttackDelayTime = 1.0f;
            }
            else if (fm.MovingFriends[i].name == "Peach(Clone)")
            {
                StarShooter ss = fm.MovingFriends[i].GetComponentInChildren<StarShooter>();
                ss.ShotDelay = 2.0f;
            }
            else // Con
            {

            }
        }
    }

    IEnumerator Crown_Effect()
    {
        Vector3 nv = fm.MovingFriends[fm.MovingFriends.Count - 1].transform.position + new Vector3(-0.1f, 1.15f, 0f);

        Crown.transform.position = nv;

        if (!Crown.activeSelf)
        {
            Crown.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            b_Crown = false;
            Crown.SetActive(false);
        }
    }


    IEnumerator Bullet_Effect()
    {
        Power.SetActive(true);
        Vector3 nv = fm.MovingFriends[fm.MovingFriends.Count - 1].transform.position + new Vector3(0f, 0.15f, 0f);

        Power.transform.position = nv;

        Power_Up();
        // 총알 주기 or 파워 --> 정녕 3초 인가
        yield return new WaitForSeconds(3.0f);
        b_Bullet = false;
        Power_Down();

        Power.SetActive(false);


    }
}
