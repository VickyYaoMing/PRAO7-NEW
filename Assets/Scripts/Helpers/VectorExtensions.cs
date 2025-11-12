using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 WithX(this Vector3 vector, float x = 0) => new Vector3(x, vector.y, vector.z);
    public static Vector3 WithY(this Vector3 vector, float y = 0) => new Vector3(vector.x, y, vector.z);
    public static Vector3 WithZ(this Vector3 vector, float z = 0) => new Vector3(vector.x, vector.y, z);
}