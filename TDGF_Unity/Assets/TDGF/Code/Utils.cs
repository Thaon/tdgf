using MADD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    //from: https://medium.com/@vagnerseibert/distributing-points-on-a-sphere-6b593cc05b42
    public static Vector3 FibDisc(int i, int numberOfNodes, int radius)
    {
        var k = i + .5f;
        var r = Mathf.Sqrt((k) / numberOfNodes);
        var theta = Mathf.PI * (1 + Mathf.Sqrt(5)) * k;
        var x = r * Mathf.Cos(theta) * radius;
        var y = r * Mathf.Sin(theta) * radius;

        return new Vector3(x, y, 0);
    }

    public static float DistanceBetweenNodes(int first, int second, int totalNodes)
    {
        Vector3 point1 = FibDisc(first, totalNodes, 1);
        Vector3 point2 = FibDisc(second, totalNodes, 1);
        return Vector3.Distance(point1, point2);
    }

    //public void GenerateMap(List<Location> locations)
    //{
    //    instantiated = new List<GameObject>();
    //    for (int i = 0; i < locations.Count; i++)
    //    {
    //        var go = Instantiate(prefab, transform);
    //        go.SetActive(true);
    //        go.transform.position = FibDisc(i, locations.Count);
    //        instantiated.Add(go);
    //    }
    //}
}
