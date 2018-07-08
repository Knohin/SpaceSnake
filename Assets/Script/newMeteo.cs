using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class newMeteo : MonoBehaviour {

    public Vector2 minPos;
    public Vector2 maxPos;

    public static List<MainObject> meteoList;

    public GameObject level0;
    public GameObject level1;
    public GameObject level2;
    public GameObject level3;

    public GameObject Portal;
    public GameObject Hole;

    public static bool active_Hole;

    public Text text;

    private float x;
    private float y;

    public static float period;

    private Transform meteoPoolHolder;

    IEnumerator Start()
    {
        meteoList = new List<MainObject>(50);
        period = 2.0f;

        Portal.gameObject.SetActive(false);
        Hole.gameObject.SetActive(false);
        active_Hole = false;

        meteoPoolHolder = GameObject.Find("MeteoPoolHolder").transform;
        if (meteoPoolHolder == null)
            meteoPoolHolder = (new GameObject("MeteoPoolHolder")).transform;

        //yield return StartCoroutine("ReadyToStart");

        Invoke("SpawnMeteo", 1.5f);
        Invoke("SpawnMeteo", 1.5f);
        Invoke("SpawnMeteo", 1.5f);

        InvokeRepeating("SpawnMeteo", 2.0f, period);
        InvokeRepeating("SpawnMeteo1", 20, period * 5.0f);
        InvokeRepeating("SpawnMeteo2", 40, period * 10.0f);
        InvokeRepeating("SpawnMeteo3", 60, period * 15.0f);

        StartCoroutine("SpawnPortal");
        StartCoroutine("SpawnHole");

        yield return null;
    }

    void Update()
    {
        for (int i = 0; i < meteoList.Count; i++)
        {
            if (!meteoList[i].b_Blackhole)
            {
                meteoList[i].ballPos.x += meteoList[i].moveSpeed * meteoList[i].moveValue.x;
                meteoList[i].ballPos.y += meteoList[i].moveSpeed * meteoList[i].moveValue.y;

                if (meteoList[i].ballPos.x < minPos.x)
                {
                    meteoList[i].moveValue.x *= -1;
                    meteoList[i].ballPos.x += minPos.x - meteoList[i].ballPos.x;
                }
                else if (meteoList[i].ballPos.x > maxPos.x)
                {
                    meteoList[i].moveValue.x *= -1;
                    meteoList[i].ballPos.x += maxPos.x - meteoList[i].ballPos.x;

                }

                if (meteoList[i].ballPos.y < minPos.y)
                {
                    meteoList[i].moveValue.y *= -1;
                    meteoList[i].ballPos.y += minPos.y - meteoList[i].ballPos.y;

                }
                else if (meteoList[i].ballPos.y > maxPos.y)
                {
                    meteoList[i].moveValue.y *= -1;
                    meteoList[i].ballPos.y += maxPos.y - meteoList[i].ballPos.y;

                }
                meteoList[i].stone.transform.Rotate(0, 0, 3.0f);
                meteoList[i].stone.transform.position = meteoList[i].ballPos;
            }
            else
            {
                if (active_Hole)
                {
                    // 크기 별 중력 다르게 적용?
                    meteoList[i].stone.transform.position = Vector3.MoveTowards(meteoList[i].stone.transform.position, CrushHole.hole_Pos, Time.deltaTime * 2.0f);

                    if (meteoList[i].stone.transform.position.Equals(CrushHole.hole_Pos))
                    {
                        Destroy(meteoList[i].stone);
                        meteoList[i].stone = null;
                        meteoList.RemoveAt(i);
                    }
                }
                else
                {
                    meteoList[i].ballPos = meteoList[i].stone.transform.position;
                    meteoList[i].b_Blackhole = false;
                }
            }

        }

        if (Hole.gameObject.activeSelf)
        {
            Hole.gameObject.transform.Rotate(0, 0, 3.0f);
            active_Hole = true;
        }
        else
        {
            active_Hole = false;
        }

        if (Portal.gameObject.activeSelf)
        {
            Portal.gameObject.transform.Rotate(0, 0, 3.0f);
        }
    }

    IEnumerator SpawnPortal()
    {
        yield return new WaitForSeconds(10.0f);

        while (true)
        {
            float x = Random.Range(-5.0f, 5.0f);
            float y = Random.Range(-9.0f, 9.0f);

            Portal.gameObject.transform.position = new Vector3(x, y, 0);

            Portal.gameObject.SetActive(true);

            yield return new WaitForSeconds(5.0f);

            Portal.gameObject.SetActive(false);

            yield return new WaitForSeconds(10.0f);
        }

    }

    IEnumerator SpawnHole()
    {
        yield return new WaitForSeconds(10.0f);

        while (true)
        {
            float x = Random.Range(-5.0f, 5.0f);
            float y = Random.Range(-9.0f, 9.0f);

            Hole.gameObject.transform.position = new Vector3(x, y, 0);

            Hole.gameObject.SetActive(true);

            yield return new WaitForSeconds(5.0f);

            Hole.gameObject.SetActive(false);

            yield return new WaitForSeconds(5.0f);

        }

    }

    void SpawnMeteo()
    {
        int t = Random.Range(1, 3);  // -6.8 ~ 5.8    // 1 or 2
        y = Random.Range(minPos.x, maxPos.y);

        if (t == 1)
            x = minPos.x;
        else
            x = maxPos.x;

        GameObject newMeteo = Instantiate(level0, new Vector3(x, y, 0f), Quaternion.identity);
        newMeteo.transform.parent = meteoPoolHolder;
        if (y >= 0 && t == 1)
            meteoList.Add(new MainObject(newMeteo, new Vector2(1,-1), new Vector3(x,y,0f)));
        else if(y >= 0 && t == 2)
            meteoList.Add(new MainObject(newMeteo, new Vector2(-1,-1), new Vector3(x, y, 0f)));
        else if (y < 0 && t == 1)
            meteoList.Add(new MainObject(newMeteo, new Vector2(1, 1), new Vector3(x, y, 0f)));
        else
            meteoList.Add(new MainObject(newMeteo, new Vector2(-1, 1), new Vector3(x, y, 0f)));

    }

    void SpawnMeteo1()
    {
        int t = Random.Range(1, 3);  // -6.8 ~ 5.8    // 1 or 2
        y = Random.Range(minPos.x, maxPos.y);

        if (t == 1)
            x = minPos.x;
        else
            x = maxPos.x;

        GameObject newMeteo = Instantiate(level1, new Vector3(x, y, 0f), Quaternion.identity);
        newMeteo.transform.parent = meteoPoolHolder;
        if (y >= 0 && t == 1)
            meteoList.Add(new MainObject(newMeteo, new Vector2(1, -1), new Vector3(x, y, 0f)));
        else if (y >= 0 && t == 2)
            meteoList.Add(new MainObject(newMeteo, new Vector2(-1, -1), new Vector3(x, y, 0f)));
        else if (y < 0 && t == 1)
            meteoList.Add(new MainObject(newMeteo, new Vector2(1, 1), new Vector3(x, y, 0f)));
        else
            meteoList.Add(new MainObject(newMeteo, new Vector2(-1, 1), new Vector3(x, y, 0f)));
    }

    void SpawnMeteo2()
    {
        int t = Random.Range(1, 3);  // -6.8 ~ 5.8    // 1 or 2
        y = Random.Range(minPos.x, maxPos.y);

        if (t == 1)
            x = minPos.x;
        else
            x = maxPos.x;

        GameObject newMeteo = Instantiate(level2, new Vector3(x, y, 0f), Quaternion.identity);
        newMeteo.transform.parent = meteoPoolHolder;
        if (y >= 0 && t == 1)
            meteoList.Add(new MainObject(newMeteo, new Vector2(1, -1), new Vector3(x, y, 0f)));
        else if (y >= 0 && t == 2)
            meteoList.Add(new MainObject(newMeteo, new Vector2(-1, -1), new Vector3(x, y, 0f)));
        else if (y < 0 && t == 1)
            meteoList.Add(new MainObject(newMeteo, new Vector2(1, 1), new Vector3(x, y, 0f)));
        else
            meteoList.Add(new MainObject(newMeteo, new Vector2(-1, 1), new Vector3(x, y, 0f)));
    }

    void SpawnMeteo3()
    {
        int t = Random.Range(1, 3);  // -6.8 ~ 5.8    // 1 or 2
        y = Random.Range(minPos.x, maxPos.y);

        if (t == 1)
            x = minPos.x;
        else
            x = maxPos.x;

        GameObject newMeteo = Instantiate(level3, new Vector3(x, y, 0f), Quaternion.identity);
        newMeteo.transform.parent = meteoPoolHolder;
        if (y >= 0 && t == 1)
            meteoList.Add(new MainObject(newMeteo, new Vector2(1, -1), new Vector3(x, y, 0f)));
        else if (y >= 0 && t == 2)
            meteoList.Add(new MainObject(newMeteo, new Vector2(-1, -1), new Vector3(x, y, 0f)));
        else if (y < 0 && t == 1)
            meteoList.Add(new MainObject(newMeteo, new Vector2(1, 1), new Vector3(x, y, 0f)));
        else
            meteoList.Add(new MainObject(newMeteo, new Vector2(-1, 1), new Vector3(x, y, 0f)));
    }

    IEnumerator ReadyToStart()
    {
        int i = 3;

        while(i > 0)
        {
            yield return new WaitForSeconds(0.5f);
            text.text = i.ToString();
            text.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            i--;
        }
        yield return new WaitForSeconds(0.5f);
        text.text = "Go!";
        yield return new WaitForSeconds(0.5f);

        text.gameObject.SetActive(false);
    }
}
