using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class newMeteo : MonoBehaviour {

    public Vector2 minPos;
    public Vector2 maxPos;

    public bool level0Active;
    public GameObject level0;
    public bool level1Active;
    public GameObject level1;
    public bool level2Active;
    public GameObject level2;
    public bool level3Active;
    public GameObject level3;

    public GameObject Hole;

    public static bool active_Hole;

    public Text text;

    private float x;
    private float y;

    public static float period;

    // For pooling
    Transform meteoPoolHolder;
    public static List<MainObject> meteoList = new List<MainObject>(); // active meteors

    List<GameObject> InactiveMeteoLevel0;
    List<GameObject> InactiveMeteoLevel1;
    List<GameObject> InactiveMeteoLevel2;
    List<GameObject> InactiveMeteoLevel3;
    const int MaxNumOfMeteoLevel0 = 50;
    const int MaxNumOfMeteoLevel1 = 30;
    const int MaxNumOfMeteoLevel2 = 15;
    const int MaxNumOfMeteoLevel3 = 5;

    static float[] radiusOfMeteo = new float[4];

    private void Awake()
    {
        meteoList = new List<MainObject>(); // active meteors
        period = 2.0f;

        Hole.gameObject.SetActive(false);
        active_Hole = false;

        meteoPoolHolder = GameObject.Find("MeteoPoolHolder").transform;
        if (meteoPoolHolder == null)
            meteoPoolHolder = (new GameObject("MeteoPoolHolder")).transform;
        
        // Pre-Make for Pooling
        {
            // Pre-Make Meteo Level 0
            InactiveMeteoLevel0 = new List<GameObject>();
            GameObject newMeteo = null;
            for (int i = MaxNumOfMeteoLevel0; 0 < i; --i)
            {
                newMeteo = Instantiate(level0);
                newMeteo.transform.parent = meteoPoolHolder;
                newMeteo.SetActive(false);
                InactiveMeteoLevel0.Add(newMeteo);
            }
            // Pre-Make Meteo Level 1
            InactiveMeteoLevel1 = new List<GameObject>();
            for (int i = MaxNumOfMeteoLevel1; 0 < i; --i)
            {
                newMeteo = Instantiate(level1);
                newMeteo.transform.parent = meteoPoolHolder;
                newMeteo.SetActive(false);
                InactiveMeteoLevel1.Add(newMeteo);
            }
            // Pre-Make Meteo Level 2
            InactiveMeteoLevel2 = new List<GameObject>();
            for (int i = MaxNumOfMeteoLevel2; 0 < i; --i)
            {
                newMeteo = Instantiate(level2);
                newMeteo.transform.parent = meteoPoolHolder;
                newMeteo.SetActive(false);
                InactiveMeteoLevel2.Add(newMeteo);
            }
            // Pre-Make Meteo Level 1
            InactiveMeteoLevel3 = new List<GameObject>();
            for (int i = MaxNumOfMeteoLevel3; 0 < i; --i)
            {
                newMeteo = Instantiate(level3);
                newMeteo.transform.parent = meteoPoolHolder;
                newMeteo.SetActive(false);
                InactiveMeteoLevel3.Add(newMeteo);
            }
        }

        GameObject[] meteos = new GameObject[4] { level0, level1, level2, level3 };
        for(int i=0; i<4; i++)
        {
            float scale = Mathf.Min(new float[]{
                meteos[i].transform.localScale.x,
                meteos[i].transform.localScale.y,
                meteos[i].transform.localScale.z
            });
            radiusOfMeteo[i] = meteos[i].GetComponent<CircleCollider2D>().radius * scale;
        }
    }

    IEnumerator Start()
    {
        //yield return StartCoroutine("ReadyToStart");

        Invoke("SpawnMeteo", 1.5f);
        Invoke("SpawnMeteo", 1.5f);
        Invoke("SpawnMeteo", 1.5f);

        if (level0Active)
        {
            StartCoroutine("Meteo_routine");
            StartCoroutine("Meteo_routine");
        }
        if (level1Active)
            StartCoroutine("Meteo1_routine");
        if (level2Active)
            StartCoroutine("Meteo2_routine");
        if (level3Active)
            StartCoroutine("Meteo3_routine");


        yield return null;
    }

    void Update()
    {
        // 난이도 조절
        if (period > 0.3f)
            period -= 0.0005f;

        for (int i = 0; i < meteoList.Count; i++)
        {
            if (!meteoList[i].b_Blackhole)
            {
                meteoList[i].ballPos.x += meteoList[i].moveSpeed * meteoList[i].moveValue.x;
                meteoList[i].ballPos.y += meteoList[i].moveSpeed * meteoList[i].moveValue.y;

                CorrectPosition(i);
            }
            else
            {
                if (CrushMagnet.active_Hole)
                {
                    // 크기 별 중력 다르게 적용?
                    meteoList[i].stone.transform.position = Vector3.MoveTowards(meteoList[i].stone.transform.position, CrushHole.hole_Pos, Time.deltaTime * 1.5f);

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
    }

    IEnumerator Meteo_routine()
    {
        yield return new WaitForSeconds(1.0f);

        while (true)
        {
            SpawnMeteo();

            yield return new WaitForSeconds(period);
        }
    }
    IEnumerator Meteo1_routine()
    {
        yield return new WaitForSeconds(20.0f);
        period += 0.2f;
        while (true)
        {
            SpawnMeteo1();

            yield return new WaitForSeconds(period * 5.0f);
        }
    }
    IEnumerator Meteo2_routine()
    {
        yield return new WaitForSeconds(40.0f);
        period += 0.3f;

        while (true)
        {
            SpawnMeteo2();

            yield return new WaitForSeconds(period * 10.0f);
        }
    }
    IEnumerator Meteo3_routine()
    {
        yield return new WaitForSeconds(60.0f);
        period += 0.4f;

        while (true)
        {
            SpawnMeteo3();

            yield return new WaitForSeconds(period * 15.0f);
        }
    }

    void SpawnMeteo()
    {
        Vector2 direction = Quaternion.Euler(.0f, .0f, Random.Range(.0f, 360.0f)) * Vector3.one;
        Vector2 position = Quaternion.Euler(.0f, .0f, Random.Range(.0f, 360.0f)) * Vector3.up * maxPos.magnitude;

        int lastIndex = InactiveMeteoLevel0.Count - 1;
        if (lastIndex == -1)
            return;

        GameObject newMeteo = InactiveMeteoLevel0[lastIndex];
        InactiveMeteoLevel0.RemoveAt(lastIndex);

        meteoList.Add(new MainObject(newMeteo, direction, position));
        newMeteo.SetActive(true);

        //newMeteo.transform.parent = meteoPoolHolder;
        CorrectPosition(meteoList.Count - 1);
    }

    void SpawnMeteo1()
    {
        Vector2 direction = Quaternion.Euler(.0f, .0f, Random.Range(.0f, 360.0f)) * Vector3.one;
        Vector2 position = Quaternion.Euler(.0f, .0f, Random.Range(.0f, 360.0f)) * Vector3.up * maxPos.magnitude;

        int lastIndex = InactiveMeteoLevel1.Count - 1;
        if (lastIndex == -1)
            return;

        GameObject newMeteo = InactiveMeteoLevel1[lastIndex];
        InactiveMeteoLevel1.RemoveAt(lastIndex);

        meteoList.Add(new MainObject(newMeteo, direction, position, 0.025f,2,1));
        newMeteo.SetActive(true);

        CorrectPosition(meteoList.Count - 1);
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
            meteoList.Add(new MainObject(newMeteo, new Vector2(1, -1), new Vector3(x, y, 0f),0.02f, 3, 2));
        else if (y >= 0 && t == 2)
            meteoList.Add(new MainObject(newMeteo, new Vector2(-1, -1), new Vector3(x, y, 0f), 0.02f, 3, 2));
        else if (y < 0 && t == 1)
            meteoList.Add(new MainObject(newMeteo, new Vector2(1, 1), new Vector3(x, y, 0f), 0.02f, 3, 2));
        else
            meteoList.Add(new MainObject(newMeteo, new Vector2(-1, 1), new Vector3(x, y, 0f), 0.02f, 3, 2));
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
            meteoList.Add(new MainObject(newMeteo, new Vector2(1, -1), new Vector3(x, y, 0f), 0.015f, 4, 3));
        else if (y >= 0 && t == 2)
            meteoList.Add(new MainObject(newMeteo, new Vector2(-1, -1), new Vector3(x, y, 0f), 0.015f, 4, 3));
        else if (y < 0 && t == 1)
            meteoList.Add(new MainObject(newMeteo, new Vector2(1, 1), new Vector3(x, y, 0f), 0.015f, 4, 3));
        else
            meteoList.Add(new MainObject(newMeteo, new Vector2(-1, 1), new Vector3(x, y, 0f), 0.015f, 4, 3));
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

    // Put the index of meteo for parameter
    void CorrectPosition(int i)
    {
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

    public void RemoveMeteo(GameObject meteo)
    {
        for (int i = meteoList.Count - 1; 0 <= i; --i)
        {
            if (meteoList[i].equal_Stone(meteo))
            {
                meteoList[i].stone.SetActive(false);
                if (meteoList[i].level == 0)
                    InactiveMeteoLevel0.Add(meteoList[i].stone);
                else if (meteoList[i].level == 1)
                    InactiveMeteoLevel1.Add(meteoList[i].stone);
                else if (meteoList[i].level == 2)
                    InactiveMeteoLevel2.Add(meteoList[i].stone);
                else if (meteoList[i].level == 3)
                    InactiveMeteoLevel3.Add(meteoList[i].stone);
                meteoList.RemoveAt(i);
                break;
            }
        }
    }

    public static MainObject FindMeteo(GameObject go)
    {
        foreach(MainObject mo in meteoList)
        {
            if (mo.equal_Stone(go))
                return mo;
        }
        return null;
    }

    public static MainObject FindNearestMeteo(Vector3 position, float maxRange = Mathf.Infinity)
    {
        float nearestDistance = maxRange;
        MainObject nearestMeteo = null;

        foreach (MainObject meteo in newMeteo.meteoList)
        {
            float meteoRadius = radiusOfMeteo[meteo.level];
            float distance = (meteo.ballPos - position).magnitude - meteoRadius;
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestMeteo = meteo;
            }
        }

        return nearestMeteo;
    }
}
