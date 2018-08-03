using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class HomingMissile : MonoBehaviour
{
    public float Speed = 1.0f;
    public float MaxDegreeDelta = 1.0f;

    Vector3 targetPos;
    Vector3 aim;

    bool kaboom;

    private void OnEnable()
    {
        kaboom = false;
        StartCoroutine(Flying());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "meteo")
        {
            collision.GetComponent<CrushMeteo>().Crush();
            kaboom = true;
        }
    }

    IEnumerator Flying()
    {
        // position이 부모로부터 계층적??이기 때문에 잠깐 빼놓고 이동시키기 위해 부모를 저장해둔다.
        Transform parent = transform.parent;
        transform.parent = null;

        Vector3 littleRigth = new Vector3(0.1f, 1f);

        while (!kaboom)
        {
            MainObject nearestMeteo = newMeteo.FindNearestMeteo(transform.position);
            
            if (nearestMeteo == null)
                targetPos = transform.position + transform.rotation * littleRigth;
            else
                targetPos = nearestMeteo.ballPos;

            aim = targetPos - transform.position;

            // Rotate Toward
            Quaternion toAim = Quaternion.FromToRotation(Vector3.up , aim);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toAim, MaxDegreeDelta * Time.deltaTime);
            // Move Forward
            transform.position += (transform.rotation * Vector3.up) * Speed * Time.deltaTime;

            yield return null;
        }

        transform.parent = parent;

        gameObject.SetActive(false);
    }
}
