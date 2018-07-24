using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Explosion : MonoBehaviour {

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Boom(Vector2 pos)
    {
        StartCoroutine("BoomCoroutine");
        transform.position = pos;
    }
    IEnumerator BoomCoroutine()
    {
        spriteRenderer.enabled = true;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.enabled = false;
    }
}
