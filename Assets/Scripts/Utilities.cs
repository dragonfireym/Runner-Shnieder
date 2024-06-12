using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static void Shuffle<T> ( this T[] array)
    {
        int n = array.Length;
        for (var i = 0; i < n; i++) {
            int r = Random.Range(i, n);
            T temp = array[i];
            array[i] = array[r];
            array[r] = temp;
        }
    }
}
