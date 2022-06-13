using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public static class ArrayExtension
{
    public static ref T RandomElement<T>(this T[] array) => ref array[Random.Range(0, array.Length)];

    public static T[] SubArray<T>(this T[] array, int offset, int length)
    {
        var result = new T[length];
        Array.Copy(array, offset, result, 0, length);
        return result;
    }

    public static T[] AddRange<T>(this T[] array, T[] added)
    {
        var result = new T[array.Length + added.Length];
        Array.Copy(array, 0, result, 0, array.Length);
        Array.Copy(added, 0, result, array.Length, added.Length);
        return result;
    }

    public static T[] Add<T>(this T[] array, T added)
    {
        var result = new T[array.Length + 1];
        Array.Copy(array, 0, result, 0, array.Length);
        result[array.Length - 1] = added;
        return result;
    }

    public static void Swap<T>(ref T a, ref T b)
    {
        var tmp = a;
        a = b;
        b = tmp;
    }

    public static T[] ForEach<T>(this T[] source, Action<T> action)
    {
        for (var i = 0; i < source.Length; i++)
            if (source[i] != null)
                action(source[i]);

        return source;
    }


    public static void ShuffleElements<T>(this T[] arr)
    {
        for (var i = 0; i < arr.Length; i++)
            Swap(ref arr[i], ref arr.RandomElement());
    }
    
    public static IEnumerable<T> Reverse<T>(this LinkedList<T> list) {
        var el = list.Last;
        while (el != null) {
            yield return el.Value;
            el = el.Previous;
        }
    }
}