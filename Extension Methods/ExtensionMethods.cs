

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;


public static class MyExtensionMethods
{



    /// <summary>
    /// returns the mean of all the values
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static float Mean(this float[] values)
    {
        return values.Sum() / (float)values.Length;
    }
    public static float Mean(this IEnumerable<float> values)
    {
        //get the length first
        var _length = 0f;
        foreach (float f in values) _length++;
        return values.Sum() * _length;
    }
    /// <summary>
    /// returns the sum of all the provided values
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static float Sum(this IEnumerable<float> values)
    {


        float sum = 0f;
        foreach (float f in values)
        {
            sum += f;
        }
        return sum;
    }

    public static IEnumerable<float> AsEnumarable(this Vector3 v)
    {
        yield return v.x;
        yield return v.y;
        yield return v.z;
    }
    public static IEnumerable<float> AsEnumarable(this Quaternion v)
    {
        yield return v.x;
        yield return v.y;
        yield return v.z;
        yield return v.w;
    }
    public static IEnumerable<float> AsEnumarable(this Vector4 v)
    {
        yield return v.x;
        yield return v.y;
        yield return v.z;
        yield return v.w;
    }
    public static T GetRandomMember<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new NullReferenceException();
        return list[UnityEngine.Random.Range(0, list.Count)];

    }
    public static Vector2 RemoveY(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }
    /// <summary>
    /// creates a new random vector between the x and y components of the provided
    /// </summary>
    /// <param name="v"></param>
    public static Vector2 RandomV2(this Vector2 v)
    {
        return new Vector2(UnityEngine.Random.Range(v.x, v.y),
            UnityEngine.Random.Range(v.x, v.y));
    }
    /// <summary>
    /// creates a new random vector between the x and y components of the provided
    /// </summary>
    /// <param name="v"></param>
    public static Vector3 RandomV3(this Vector2 v)
    {
        return new Vector3(UnityEngine.Random.Range(v.x, v.y),
            UnityEngine.Random.Range(v.x, v.y),
           UnityEngine.Random.Range(v.x, v.y));
    }
    public static Vector3 ToXZPlane(this Vector2 v2)
    {
        return new Vector3(v2.x, 0, v2.y);
    }
    public static Color AsColor(this Vector3 v)
    {
        return new Color(v.x, v.y, v.z);
    }
    public static bool IsAlphaNum(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return false;

        return (str.ToCharArray().All(c => Char.IsLetter(c) || Char.IsNumber(c)));
    }
    public static Vector3 AsVector(this Color c)
    {
        return new Vector3(c.r, c.g, c.b);
    }
    /// <summary>
    /// Returns the vector 3 in world space in
    /// terms of the provided transform
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public static Vector3 ToLocalSpace(this Vector3 v3, Transform localSpace)
    {
        return (v3.x * localSpace.right + v3.y * localSpace.up + v3.z * localSpace.forward);
    }
    public static Rect Lerp(Rect from, Rect to, float lerp)
    {
        return new Rect
        {
            width = Mathf.Lerp(from.width, to.width, lerp),
            height = Mathf.Lerp(from.height, to.height, lerp),

        };
    }

    /// <summary>
    /// Returns the vector 2 in world space in
    /// terms of the provided transform
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public static Vector3 ToWorldSpace(this Vector2 v2, Transform localSpace)
    {


        return (v2.x * localSpace.right + v2.y * localSpace.forward);
    }

    /// <summary>
    /// The vector from this vector to the provided
    /// </summary>
    /// <param name="v3"></param>
    /// <returns></returns>
    public static Vector3 To(this Vector3 v3, Vector3 to)
    {
        return to - v3;
    }
    /// <summary>
    /// The vector to this vector from the provided
    /// </summary>
    /// <param name="v3"></param>
    /// <returns></returns>
    public static Vector3 From(this Vector3 v3, Vector3 from)
    {
        return v3 - from;
    }

    /// <summary>
    /// Get the desired component as a list recursively visiting ALL childern of a gameobject
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="g"></param>
    /// <returns></returns>

    public static List<T> GetComponentsInAllChildren<T>(this GameObject g)
    {
        var l = new List<T>();
        GetComponenetsInChildrenHelper<T>(l, g);
        return l;
    }
    private static void GetComponenetsInChildrenHelper<T>(List<T> list, GameObject g)
    {
        if (g.TryGetComponent<T>(out var t)){
            list.Add(t);
        }
        for(int i = 0; i < g.transform.childCount; i++) GetComponenetsInChildrenHelper<T>(list, g.transform.GetChild(i).gameObject);
    }

    public static List<Transform> GetAllChildren(this GameObject g)
    {
        var l = new List<Transform>();
        GetAllnChildrenHelper(l, g);
        return l;
    }
    private static void GetAllnChildrenHelper(List<Transform> list, GameObject g)
    {
        list.Add(g.transform);
        for( int i = 0; i < g.transform.childCount  ; i++)
        {
            var child = g.transform.GetChild(i);
            list.Add(child);
            GetAllnChildrenHelper(list, child.gameObject);
        }

    }



    public static bool FuzzyEquals(this float f1, float f2, float tol)
    {
        return (Mathf.Abs(f1 - f2) < tol);
     
    }

    public static bool FuzzyEquals(this Vector3 _v1, Vector3 _v2, float tol)
    {
        return (_v1.x.FuzzyEquals(_v2.x, tol) && _v1.y.FuzzyEquals(_v2.y, tol) && _v1.z.FuzzyEquals(_v2.z, tol));
    }
    public static bool FuzzyEquals(this Quaternion _q1, Quaternion _q2, float tol)
    {
        return (_q1.x.FuzzyEquals(_q2.x, tol) && _q1.y.FuzzyEquals(_q2.y, tol) && _q1.z.FuzzyEquals(_q2.z, tol) &&
           _q1.w.FuzzyEquals(_q2.w, tol));
    }

    /// <summary>
    /// Transforms a rectTransform into the world rect
    /// </summary>
    /// <param name="rectTransform"></param>
    /// <returns></returns>
    public static Rect GetWorldRect(this RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        // Get the bottom left corner.
        Vector3 position = corners[0];

        Vector2 size = new Vector2(
            rectTransform.lossyScale.x * rectTransform.rect.size.x,
            rectTransform.lossyScale.y * rectTransform.rect.size.y);

        return new Rect(position, size);
    }


    public static bool isLoaded(this AsyncOperation op)
    {
        return (op.progress >= .9f || op.isDone);
    }
}
public class PriorityQueue<T>
{
    IComparer<T> comparer;
    T[] heap;
    public int Count { get; private set; }
    public PriorityQueue() : this(null) { }
    public PriorityQueue(int capacity) : this(capacity, null) { }
    public PriorityQueue(IComparer<T> comparer) : this(16, comparer) { }
    public PriorityQueue(int capacity, IComparer<T> comparer)
    {
        this.comparer = (comparer == null) ? Comparer<T>.Default : comparer;
        this.heap = new T[capacity];
    }
    public void push(T v)
    {
        if (Count >= heap.Length) Array.Resize(ref heap, Count * 2);
        heap[Count] = v;
        SiftUp(Count++);
    }
    public T pop()
    {
        var v = top();
        heap[0] = heap[--Count];
        if (Count > 0) SiftDown(0);
        return v;
    }
    public T top()
    {
        if (Count > 0) return heap[0];
        throw new NullReferenceException();
    }
    void SiftUp(int n)
    {
        var v = heap[n];
        for (var n2 = n / 2; n > 0 && comparer.Compare(v, heap[n2]) > 0; n = n2, n2 /= 2) heap[n] = heap[n2];
        heap[n] = v;
    }
    void SiftDown(int n)
    {
        var v = heap[n];
        for (var n2 = n * 2; n2 < Count; n = n2, n2 *= 2)
        {
            if (n2 + 1 < Count && comparer.Compare(heap[n2 + 1], heap[n2]) > 0) n2++;
            if (comparer.Compare(v, heap[n2]) >= 0) break;
            heap[n] = heap[n2];
        }
        heap[n] = v;
    }
}

