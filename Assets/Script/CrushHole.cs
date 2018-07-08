using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushHole : MonoBehaviour {

    public static Vector3 hole_Pos;

    private FriendsManager fm;

	// Use this for initialization
	void Start () {
        hole_Pos = Vector3.one;
        fm = GameObject.Find("GameManager").GetComponent<FriendsManager>();
	}
	

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag.Equals("meteo"))
        {
            // Vector3.MoveTowards , LookAt, RotateTowards
            for(int i = 0; i < newMeteo.meteoList.Count; i++)
            {
                if (newMeteo.meteoList[i].equal_Stone(other.gameObject))
                {
                    newMeteo.meteoList[i].b_Blackhole = true;
                    hole_Pos = this.gameObject.transform.position;
                    //newMeteo.meteoList[i].moveSpeed = 0;
                    //newMeteo.meteoList[i].stone.transform.position = Vector3.MoveTowards(newMeteo.meteoList[i].ballPos, this.gameObject.transform.position, Time.deltaTime * 0.02f);
                }

            }

        }
        else if(other.gameObject.tag.Equals("Friend"))
        {
            StartCoroutine("Drag", other.gameObject);
        }
        else if(other.gameObject.tag.Equals("tail"))
        {
            for (int i = 0; i < newFriends.friendList.Count; i++)
            {
                if(newFriends.friendList[i].equal_Stone(other.gameObject))
                {
                    newFriends.friendList[i].b_Blackhole = true;
                    hole_Pos = this.gameObject.transform.position;
                }
            }
        }
    }

    IEnumerator Drag(GameObject go)
    {
        while (newMeteo.active_Hole)
        {
            go.transform.position = Vector3.MoveTowards(go.transform.position,
            CrushHole.hole_Pos, Time.deltaTime * 2.0f);

            if (go.transform.position.Equals(CrushHole.hole_Pos))
            {
                fm.DetachFriend(go);
                fm.FloatingFriends.Remove(go);
                break;
            }

            yield return null;
        }

        yield return null;
    }
}
