using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBomb : MonoBehaviour {

    public AnimationClip Idle, Explode;
    public float ExplosionRadius;

    private Animator animator;
    public static WaitForSeconds waitExplosionDuration;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        waitExplosionDuration = new WaitForSeconds(Explode.length);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }

    public void Ignite(Vector3 velocity,float explosionTimer)
    {
        StartCoroutine(_Ignite(velocity, explosionTimer));
    }

    IEnumerator _Ignite(Vector3 velocity, float explosionTimer)
    {
        Transform parent = transform.parent;
        transform.position = parent.position;
        transform.parent = null;

        while (0 < explosionTimer)
        {
            explosionTimer -= Time.deltaTime;
            transform.position += velocity * Time.deltaTime;
            yield return null;
        }
        //BOOM!
        //animator.SetTrigger("Boom"); // animator.GetCurrentStateInfo(0)[0] 에 State Update가 안되냐
        animator.Play("SpikeBomb_Explode");
        yield return null;

        float distanceSqr = .0f;
        //foreach (MainObject meteo in newMeteo.meteoList)
        for (int i= newMeteo.meteoList.Count-1; 0<=i; --i)
        {
            MainObject meteo = newMeteo.meteoList[i];
            distanceSqr = (meteo.ballPos - transform.position).sqrMagnitude;
            if(distanceSqr <= ExplosionRadius*ExplosionRadius)
            {
                meteo.stone.GetComponent<CrushMeteo>().Crush(4);
            }
        }

        yield return waitExplosionDuration;

        transform.parent = parent;
        gameObject.SetActive(false);
    }

}
