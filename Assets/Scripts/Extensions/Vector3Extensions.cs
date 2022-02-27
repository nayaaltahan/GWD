using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 WithX(this Vector3 vec, float newX)
    {
        return new Vector3(newX, vec.y, vec.z);
    }

    public static Vector3 WithY(this Vector3 vec, float newY)
    {
        return new Vector3(vec.x, newY, vec.z);
    }
    
    public static Vector3 WithZ(this Vector3 vec, float newZ)
    {
        return new Vector3(vec.x, vec.y, newZ);
    }
}
