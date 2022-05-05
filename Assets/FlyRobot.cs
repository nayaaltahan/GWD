using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyRobot : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float delay;
    [SerializeField] float flyTime;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        yield return new WaitForSeconds(delay);
        float timer = 0;

        while (timer < flyTime)
        {
            timer += Time.deltaTime;
            transform.position += speed * transform.up;
            yield return null;

        }


    }
}
