using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Cooldown
{
    float c;
    public float d;
    public Cooldown(float _d, bool initC = false)
    {
        
        d = _d;
        if (initC)
        {
            c = d;
        }
        else
        {
            c = 0;
        }
    }
    public bool CountDown(float ratio = 1f)
    {
        c -= Time.deltaTime * ratio;
        if(c <= 0)
        {
            c = d;
            return true;
        }
        return false;
    }
	
}
