using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MainObject
{
    public GameObject stone;
    public Vector2 moveValue;
    public Vector3 ballPos;

    public float moveSpeed = 0.03f;
    public int health;
    public bool b_Blackhole;

    public int level=0;

    public MainObject()
    {
        stone = null;
        moveValue = Vector2.one;
        ballPos = Vector3.one;
        health = 1;
        b_Blackhole = false;
    }
    public MainObject(GameObject obj)
    {
        stone = obj;
        moveValue = Vector2.one;
        ballPos = Vector3.one;
        health = 1;
        b_Blackhole = false;
    }
    public MainObject(GameObject obj, Vector2 direction, Vector3 pos)
    {
        stone = obj;
        moveValue = direction;
        ballPos = pos;
        health = 1;
        b_Blackhole = false;
    }
    public MainObject(GameObject obj, Vector2 direction, Vector3 pos, float speed, int hp=1, int _level=0)
    {
        stone = obj;
        moveValue = direction;
        ballPos = pos;
        moveSpeed = speed;
        health = hp;
        b_Blackhole = false;

        level = _level;
    }
    public bool equal_Stone(GameObject obj)
    {
        if (this.stone.Equals(obj))
            return true;
        else
            return false;
    }
}
