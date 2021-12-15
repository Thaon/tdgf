using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MADD;

public class LocationGO : MonoBehaviour
{
    public Location locationInfo;
    
    void Start()
    {
        transform.position = Utils.FibDisc(locationInfo.index, TDGF.Instance._game.locationsAmount, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
