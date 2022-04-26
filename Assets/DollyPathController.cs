using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DollyPathController : MonoBehaviour
{
    [SerializeField] private GameObject dollyPathParent;
    private CinemachineSmoothPath dollyPath;
    
    // Start is called before the first frame update
    void Start()
    {
        dollyPath = GetComponent<CinemachineSmoothPath>();
        
        dollyPath.m_Waypoints = new CinemachineSmoothPath.Waypoint[dollyPathParent.transform.childCount];
        for (var i = 0; i < dollyPath.m_Waypoints.Length; i++)
        {
            dollyPath.m_Waypoints[i].position = dollyPathParent.transform.GetChild(i).position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
