using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    [SerializeField] private Vector3 color;
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(color.x, color.y, color.z);
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
