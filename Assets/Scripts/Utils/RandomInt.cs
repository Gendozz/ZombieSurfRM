using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomInt
{
    public static int GetRandomInt(int topCap)
    {
        return Random.Range(0, topCap);
    }
}
