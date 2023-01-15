using System;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    private static Dictionary<Transform, Transform[]> cachedTransforms;
    public static void Reset(this Transform transform)
    {
        ResetLocal(transform);
        ResetScale(transform);
    }
    public static void ResetLocal(this Transform transform)
    {
        ResetPosition(transform);
        ResetRotation(transform);
    }

    public static void ResetRotation(this Transform transform)
    {
        transform.localRotation = Quaternion.identity;
    }

    public static void ResetPosition(this Transform transform)
    {
        transform.localPosition = Vector3.zero;
    }

    public static void ResetScale (this Transform transform)
    {
        transform.localScale = Vector3.one;
    }

    public static void SetRotationY(this Transform transform, bool isLocal)
    {
        //if(isLocal)
          //  transform.localRotation = Quaternion.Euler();
    }

    public static void AddWorldX(this Transform transform, float x)
         {
             var position = transform.position;
             position = new Vector3(position.x + x, position.y, position.z);
             transform.position = position;
         }
    
    public static void AddWorldY(this Transform transform, float y)
    {
        var position = transform.position;
        position = new Vector3(position.x, position.y + y, position.z);
        transform.position = position;
    }
    
    public static Transform FirstChildOrDefault(this Transform parent, Func<Transform, bool> query)
    {
        if (parent.childCount == 0)
        {
            return null;
        }

        Transform result = null;
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            if (query(child))
            {
                return child;
            }
            result = FirstChildOrDefault(child, query);
        }

        return result;
    }

    public static T FindObjectOfTypeInChildren<T>(this Transform parent) where T : class
    {
        
        if (parent.childCount == 0)
        {
            return null;
        }

        T result = null;
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            if (child.GetComponent<T>() != null)
            {
                return child.GetComponent<T>();
            }
            result = FindObjectOfTypeInChildren<T>(child);
        }

        return result;
    }
    
    
    public static List<Transform> FindAllLeaves(this Transform transform)
    {
        var transforms = new List<Transform>();
        var queue = new Queue<Transform>();

        queue.Enqueue(transform);

        while(queue.Count > 0)
        {
            var t = queue.Dequeue();
            if(transform.childCount > 0)
            {
                foreach (Transform child in t.GetComponentsInChildren<Transform>())
                {
                    queue.Enqueue(child);
                }
            } else
            {
                transforms.Add(t);
            }
        }
        return transforms;
    }

    public static Transform FindInAllLeaves(this Transform transform, string name, bool useCache = false)
    {
        Transform[] childTransforms;
        if (useCache)
        {
            if (cachedTransforms == null || !cachedTransforms.TryGetValue(transform, out childTransforms))
            {
                childTransforms = transform.GetComponentsInChildren<Transform>();
                cachedTransforms ??= new Dictionary<Transform, Transform[]>();
                cachedTransforms.Add(transform, childTransforms);
            }
        }
        else
        {
            childTransforms = transform.GetComponentsInChildren<Transform>();
        }
        
        foreach (var child in childTransforms)
        {
            if (child != null && child.name.Equals(name))
                return child;
        }
        return null;
    }

    
    public static List<GameObject> FindObjectsWithTag(this Transform parent, string tag)
    {
        List<GameObject> taggedGameObjects = new List<GameObject>();
 
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == tag)
            {
                taggedGameObjects.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                taggedGameObjects.AddRange(FindObjectsWithTag(child, tag));
            }
        }
        return taggedGameObjects;
    }
    
    public static List<Transform> FindTransformsWithTag(this Transform parent, string tag)
    {
        List<Transform> taggedGameObjects = new List<Transform>();
 
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == tag)
            {
                taggedGameObjects.Add(child.transform);
            }
            if (child.childCount > 0)
            {
                taggedGameObjects.AddRange(FindTransformsWithTag(child, tag));
            }
        }
        return taggedGameObjects;
    }
    
    public static T FindTypeInChildren<T>(this Transform parent) where T : class
    {
        T result = null;
        var queue = new Queue<Transform>();

        queue.Enqueue(parent);
    
        while(queue.Count > 0)
        {
            var t = queue.Dequeue();
            if(parent.childCount > 0)
            {
                foreach (Transform child in t.GetComponentsInChildren<Transform>())
                {
                    if (child.GetComponent<T>() != null)
                    {
                        
                        return child.GetComponent<T>();
                    }
                    queue.Enqueue(child);
                }
            } 
            else
            {
                if (t.GetComponent<T>() != null)
                    return t.GetComponent<T>();
            }
        }

        return result;
    }
    
    /// <summary>
    /// Finds components only in first children
    /// </summary>
    /// <param name="tr"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T[] RGetComponentsInFirstChildren<T>(this Transform tr) where T : Component
    {
        var list = new T[tr.childCount];
        for (int i = 0; i < tr.childCount; i++)
        {

            list[i] = tr.GetChild(i).GetComponent<T>();
        }

        return list;
    }
    
    /// <summary>
    /// Look at a GameObject
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target">The thing to look at</param>
    public static void LookAt(this Transform self, GameObject target)
    {
        self.LookAt(target.transform);
    }

    /// <summary>
    /// Find the rotation to look at a Vector3
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target">The thing to look at</param>
    /// <returns></returns>
    public static Quaternion GetLookAtRotation(this Transform self, Vector3 target)
    {
        return Quaternion.LookRotation(target - self.position);
    }

    /// <summary>
    /// Find the rotation to look at a Transform
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target">The thing to look at</param>
    /// <returns></returns>
    public static Quaternion GetLookAtRotation(this Transform self, Transform target)
    {
        return GetLookAtRotation(self, target.position);
    }

    /// <summary>
    /// Find the rotation to look at a GameObject
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target">The thing to look at</param>
    /// <returns></returns>
    public static Quaternion GetLookAtRotation(this Transform self, GameObject target)
    {
        return GetLookAtRotation(self, target.transform.position);
    }

    public static void LookTowards(this Transform self, Vector3 target)
    {
        self.rotation = GetLookAtRotation(self, target);
    }

    public static void LookTowardsWithY(this Transform self, Vector3 target)
    {
        var rotation = GetLookAtRotation(self, target);
        rotation.eulerAngles = rotation.eulerAngles.WithX(0).WithZ(0);
        self.rotation = rotation;
    }


    /// <summary>
    /// Instantly look away from a target Vector3
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target">The thing to look away from</param>
    public static void LookAwayFrom(this Transform self, Vector3 target)
    {
        self.rotation = GetLookAwayFromRotation(self, target);
    }

    /// <summary>
    /// Instantly look away from a target transform
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target">The thing to look away from</param>
    public static void LookAwayFrom(this Transform self, Transform target)
    {
        self.rotation = GetLookAwayFromRotation(self, target);
    }

    /// <summary>
    /// Instantly look away from a target GameObject
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target">The thing to look away from</param>
    public static void LookAwayFrom(this Transform self, GameObject target)
    {
        self.rotation = GetLookAwayFromRotation(self, target);
    }


    /// <summary>
    /// Find the rotation to look away from a target Vector3
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target">The thing to look away from</param>
    public static Quaternion GetLookAwayFromRotation(this Transform self, Vector3 target)
    {
        return Quaternion.LookRotation(self.position - target);
    }

    /// <summary>
    /// Find the rotation to look away from a target transform
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target">The thing to look away from</param>
    public static Quaternion GetLookAwayFromRotation(this Transform self, Transform target)
    {
        return GetLookAwayFromRotation(self, target.position);
    }

    /// <summary>
    /// Find the rotation to look away from a target GameObject
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target">The thing to look away from</param>
    public static Quaternion GetLookAwayFromRotation(this Transform self, GameObject target)
    {
        return GetLookAwayFromRotation(self, target.transform.position);
    }
}
