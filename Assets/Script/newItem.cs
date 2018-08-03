using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newItem : MonoBehaviour
{

    public Vector2 minPos;
    public Vector2 maxPos;

    public static List<MainObject> itemList;

    public GameObject magnet;
    public GameObject crown;
    public GameObject bullet;
    IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(7.0f);

        while (true)
        {
            if (itemList.Count < 2)
            {
                yield return new WaitForSeconds(5.0f);

                float x = Random.Range(-5.2f, 5.2f);
                float y = Random.Range(-9.2f, 9.2f);

                int cha = Random.Range(1, 7); // 1 ~ 6 -> 1 -> 무적, 2, 3, 4 -> 총알, 5,6 -> 터뜨리기

                if (cha == 1)
                {
                    itemList.Add(new MainObject(Instantiate(crown, new Vector3(x, y, 0f), Quaternion.identity), new Vector2(1, -1), new Vector3(x, y, 0f), 0.02f));
                }
                else if (cha == 2 || cha == 3 || cha == 4)
                {
                    itemList.Add(new MainObject(Instantiate(bullet, new Vector3(x, y, 0f), Quaternion.identity), new Vector2(-1, -1), new Vector3(x, y, 0f), 0.02f));

                }
                else
                {
                    itemList.Add(new MainObject(Instantiate(magnet, new Vector3(x, y, 0f), Quaternion.identity), new Vector2(1, 1), new Vector3(x, y, 0f), 0.02f));
                }

                yield return new WaitForSeconds(10.0f);
            }
        }
    }

    void Start()
    {
        itemList = new List<MainObject>(5);
        StartCoroutine(SpawnItem());
    }

    void Update()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].ballPos.x += itemList[i].moveSpeed * itemList[i].moveValue.x;
            itemList[i].ballPos.y += itemList[i].moveSpeed * itemList[i].moveValue.y;

            if (itemList[i].ballPos.x < minPos.x)
            {
                itemList[i].moveValue.x *= -1;
                itemList[i].ballPos.x += minPos.x - itemList[i].ballPos.x;
            }
            else if (itemList[i].ballPos.x > maxPos.x)
            {
                itemList[i].moveValue.x *= -1;
                itemList[i].ballPos.x += maxPos.x - itemList[i].ballPos.x;

            }

            if (itemList[i].ballPos.y < minPos.y)
            {
                itemList[i].moveValue.y *= -1;
                itemList[i].ballPos.y += minPos.y - itemList[i].ballPos.y;

            }
            else if (itemList[i].ballPos.y > maxPos.y)
            {
                itemList[i].moveValue.y *= -1;
                itemList[i].ballPos.y += maxPos.y - itemList[i].ballPos.y;

            }

            itemList[i].stone.transform.position = itemList[i].ballPos;
        }
    }
}
