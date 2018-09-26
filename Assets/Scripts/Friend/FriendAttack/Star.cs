using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {

    Vector3 direction;
    float speed;
    float angularVelocity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "meteo")
        {
            collision.GetComponent<CrushMeteo>().Crush(2);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "meteo")
        {
            collision.GetComponent<CrushMeteo>().Crush(2);
        }
    }

    public void Shot(Vector3 _dir, float _speed, float _angularVelocity = 5.0f)
    {
        direction = _dir;
        speed = _speed;
        angularVelocity = _angularVelocity;
        StartCoroutine(Shot());
    }

    IEnumerator Shot()
    {
        Transform parent = transform.parent;
        transform.parent = null;
        transform.position = parent.position;

        float moved = 0.0f;
        while (moved < 10.0f)
        {
            transform.position += direction * speed * Time.deltaTime;
            transform.rotation *= Quaternion.Euler(0, 0, angularVelocity);

            moved += speed * Time.deltaTime;
            yield return null;
        }

        transform.parent = parent;
        gameObject.SetActive(false);
    }
}
