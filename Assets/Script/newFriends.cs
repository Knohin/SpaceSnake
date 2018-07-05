using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newFriends : MonoBehaviour {

    public GameObject Muzi;
    public GameObject Ryan;
    public GameObject Apeach;
    public GameObject Frodo;
    public GameObject Neo;
    public GameObject Tube;
    public GameObject Con;
    public GameObject JayG;

    public GameObject Portal;
    public GameObject Hole;

    public static List<MainObject> friendList;

    public static bool active_Hole;
    
    void SpawnFriends()
    {

        if (friendList.Count > 5)
            return;

        float x = Random.Range(-6.5f, 5.5f);
        float y = Random.Range(-11f, 11f);

        int cha = Random.Range(1, 9); // 1 ~ 8
        switch(cha)
        {
            case 1:
                {
                    friendList.Add(new MainObject(Instantiate(Muzi, new Vector3(x, y, 0f), Quaternion.identity), Vector2.zero, new Vector3(x,y,0)));
                }
                break;
            case 2:
                {
                    friendList.Add(new MainObject(Instantiate(Ryan, new Vector3(x, y, 0f), Quaternion.identity), Vector2.zero, new Vector3(x, y, 0)));
                }
                break;
            case 3:
                {
                    friendList.Add(new MainObject(Instantiate(Apeach, new Vector3(x, y, 0f), Quaternion.identity), Vector2.zero, new Vector3(x, y, 0)));
                }
                break;
            case 4:
                {
                    friendList.Add(new MainObject(Instantiate(Frodo, new Vector3(x, y, 0f), Quaternion.identity), Vector2.zero, new Vector3(x, y, 0)));
                }
                break;
            case 5:
                {
                    friendList.Add(new MainObject(Instantiate(Tube, new Vector3(x, y, 0f), Quaternion.identity), Vector2.zero, new Vector3(x, y, 0)));
                }
                break;
            case 6:
                {
                    friendList.Add(new MainObject(Instantiate(Con, new Vector3(x, y, 0f), Quaternion.identity), Vector2.zero, new Vector3(x, y, 0)));
                }
                break;
            case 7:
                {
                    friendList.Add(new MainObject(Instantiate(Neo, new Vector3(x, y, 0f), Quaternion.identity), Vector2.zero, new Vector3(x, y, 0)));
                }
                break;
            case 8:
                {
                    friendList.Add(new MainObject(Instantiate(JayG, new Vector3(x, y, 0f), Quaternion.identity), Vector2.zero, new Vector3(x, y, 0)));
                }
                break;

        }
    }
    
    IEnumerator SpawnPortal()
    {
        yield return new WaitForSeconds(10.0f);

        while(true)
        {
            float x = Random.Range(-6.0f, 5.0f);
            float y = Random.Range(-10.5f, 10.5f);

            Portal.gameObject.transform.position = new Vector3(x, y, 0);

            Portal.gameObject.SetActive(true);

            yield return new WaitForSeconds(5.0f);

            Portal.gameObject.SetActive(false);

            yield return new WaitForSeconds(10.0f);
        }
        
    }
    
    /*
     void SpawnPortal()
    {
        float x = Random.Range(-6.0f, 5.0f);
        float y = Random.Range(-10.5f, 10.5f);

        GameObject obj = Instantiate(Portal, new Vector3(x, y, 0), Quaternion.identity);

        Destroy(obj, 5.0f);
    }
    */

    IEnumerator SpawnHole() 
    {
        yield return new WaitForSeconds(10.0f);

        while (true)
        {
            float x = Random.Range(-6.0f, 5.0f);
            float y = Random.Range(-10.5f, 10.5f);

            Hole.gameObject.transform.position = new Vector3(x, y, 0);

            Hole.gameObject.SetActive(true);

            yield return new WaitForSeconds(5.0f);

            Hole.gameObject.SetActive(false);

            yield return new WaitForSeconds(5.0f);

        }

    }

    // Use this for initialization
    void Start () {

        friendList = new List<MainObject>(10);

        Portal.gameObject.SetActive(false);
        Hole.gameObject.SetActive(false);
        active_Hole = false;
        
        InvokeRepeating("SpawnFriends", 4.5f, 5);
        //InvokeRepeating("SpawnPortal", 10, 10);
        StartCoroutine("SpawnPortal");
        StartCoroutine("SpawnHole");
    }

    // Update is called once per frame
    void Update () {
            
        if(Hole.gameObject.activeSelf)
        {
            Hole.gameObject.transform.Rotate(0, 0, 3.0f);
            active_Hole = true;
        }
        else
        {
            active_Hole = false;
        }

        if(Portal.gameObject.activeSelf)
        {
            Portal.gameObject.transform.Rotate(0, 0, 3.0f);
        }

        //for(int i = 0; i < friendList.Count; i++)
        //{
        //    if(!friendList[i].b_Blackhole)
        //    {
        //        friendList[i].stone.transform.position = friendList[i].ballPos;
        //    }
        //    else
        //    {

        //        if(Hole.gameObject.activeSelf)
        //        {
        //            friendList[i].stone.transform.position = Vector3.MoveTowards(friendList[i].stone.transform.position, CrushHole.hole_Pos, Time.deltaTime * 2.0f);

        //            if (friendList[i].stone.transform.position.Equals(CrushHole.hole_Pos))
        //            {
        //                Destroy(friendList[i].stone);
        //                friendList[i].stone = null;
        //                friendList.RemoveAt(i);
        //            }
        //        }
        //        else
        //        {
        //            friendList[i].ballPos = friendList[i].stone.transform.position;
        //            friendList[i].b_Blackhole = false;
        //        }

        //    }
        //}
    }
}
