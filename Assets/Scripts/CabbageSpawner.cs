using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabbageSpawner : MonoBehaviour
{

    [SerializeField] GameObject cabbagePrefab;
    List<Cabbage> objectPool = new List<Cabbage>();

    [SerializeField] float speed;
    [SerializeField] float interval;

    [Range(0f, 5f)]
    [SerializeField] float intervalRandomness;

    [Range(0f, 5f)]
    [SerializeField] float velocityRandomness;

    [Range(0f, 360f)]
    [SerializeField] float angularRandomness;

    [SerializeField] bool emitOnEnable;


    private void OnEnable()
    {
        Emit();
    }

    public void Emit()
    {
        StartCoroutine(EmitRoutine());
    }

    public void StopEmitting()
    {
        StopCoroutine(EmitRoutine());
    }

    IEnumerator EmitRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval + Random.Range(-intervalRandomness, +intervalRandomness));
            SpawnCabbage();
        }
    }


    public void SpawnCabbage()
    {
        Vector3 velocity = Quaternion.Euler(0f, Random.Range(-angularRandomness, angularRandomness), 0) * transform.up * speed;

        foreach (Cabbage c in objectPool)
        {
            if (!c.gameObject.activeInHierarchy)
            {
                c.Respawn(transform.position, velocity);
                return;
            }
        }

        GameObject newCabbage = Instantiate(cabbagePrefab);
        Cabbage newCabbageComponent = newCabbage.GetComponent<Cabbage>();
        objectPool.Add(newCabbageComponent);
        newCabbageComponent.Respawn(transform.position, velocity);

    }

}