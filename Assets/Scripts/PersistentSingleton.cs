using UnityEngine;
using UnityEngine.Events;
using System;
using System.Reflection;
using System.Collections.Generic;

/// <summary>
/// Generic singleton base that persists across scene loads.
/// Fixes scene references, UnityEvents, and child references to always point to the active instance.
/// </summary>
public abstract class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    public void DestroyPersistor()
    {
        GameObject.Destroy(gameObject);
    }

    protected virtual void Awake()
    {
        transform.parent = null;
        if (Instance != null && Instance != this)
        {
            //Debug.Log($"Destroying duplicate instance of {typeof(T).Name} on {gameObject}");
            ReplaceReferences(typeof(T), this as T, Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
        DontDestroyOnLoad(gameObject);
        ReplaceReferences(typeof(T), null, Instance);
    }

    /// <summary>
    /// Replace references to oldRef (or its children) with newRef (or its children).
    /// </summary>
    private static void ReplaceReferences(Type singletonType, T oldRef, T newRef)
    {
        // Build mapping from old GameObject/Components to new ones
        var replacementMap = BuildReplacementMap(oldRef, newRef);

        var behaviours = FindObjectsOfType<MonoBehaviour>(true);
        foreach (var b in behaviours)
        {
            if (b == null) continue;

            var fields = b.GetType()
                          .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var value = field.GetValue(b);
                if (value == null) continue;

                // --- Direct field replacement (Component or GameObject) ---
                if (replacementMap.TryGetValue(value, out var replacement))
                {
                    field.SetValue(b, replacement);
                }

                // --- UnityEvent replacement ---
                if (value is UnityEventBase unityEvent)
                {
                    ReplaceUnityEventTargets(unityEvent, replacementMap);
                }
            }
        }
    }

    /// <summary>
    /// Builds a map of old GameObjects/Components to new ones, based on hierarchy paths.
    /// </summary>
    private static Dictionary<object, object> BuildReplacementMap(T oldRef, T newRef)
    {
        var map = new Dictionary<object, object>();
        if (newRef == null) return map;

        // Always include root component & GameObject
        if (oldRef != null)
        {
            map[oldRef] = newRef;
            map[oldRef.gameObject] = newRef.gameObject;

            var oldChildren = oldRef.GetComponentsInChildren<Transform>(true);
            var newChildren = newRef.GetComponentsInChildren<Transform>(true);

            // Match children by hierarchy path
            var pathToNew = new Dictionary<string, Transform>();
            foreach (var t in newChildren)
                pathToNew[GetPath(t, newRef.transform)] = t;

            foreach (var t in oldChildren)
            {
                string path = GetPath(t, oldRef.transform);
                if (pathToNew.TryGetValue(path, out var newT))
                {
                    map[t.gameObject] = newT.gameObject;

                    var oldComps = t.GetComponents<Component>();
                    var newComps = newT.GetComponents<Component>();
                    for (int i = 0; i < oldComps.Length && i < newComps.Length; i++)
                    {
                        if (oldComps[i] != null && newComps[i] != null &&
                            oldComps[i].GetType() == newComps[i].GetType())
                        {
                            map[oldComps[i]] = newComps[i];
                        }
                    }
                }
            }
        }
        else
        {
            // First-time init: just map the instance to itself
            map[newRef] = newRef;
            map[newRef.gameObject] = newRef.gameObject;
        }

        return map;
    }

    /// <summary>
    /// Gets hierarchy path of a transform relative to a root.
    /// </summary>
    private static string GetPath(Transform t, Transform root)
    {
        if (t == root) return "";
        return GetPath(t.parent, root) + "/" + t.name;
    }

    /// <summary>
    /// Replace UnityEvent persistent targets using replacement map.
    /// </summary>
    private static void ReplaceUnityEventTargets(UnityEventBase unityEvent, Dictionary<object, object> replacementMap)
    {
        int count = unityEvent.GetPersistentEventCount();

        for (int i = 0; i < count; i++)
        {
            var target = unityEvent.GetPersistentTarget(i);
            if (target == null) continue;

            if (replacementMap.TryGetValue(target, out var replacement))
            {
                string method = unityEvent.GetPersistentMethodName(i);
                if (string.IsNullOrEmpty(method)) continue;

                MethodInfo methodInfo = replacement.GetType().GetMethod(method,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (methodInfo == null) continue;

                // Build delegate type dynamically
                var parameters = methodInfo.GetParameters();
                Type delegateType = parameters.Length switch
                {
                    0 => typeof(UnityAction),
                    1 => typeof(UnityAction<>).MakeGenericType(parameters[0].ParameterType),
                    2 => typeof(UnityAction<,>).MakeGenericType(parameters[0].ParameterType, parameters[1].ParameterType),
                    3 => typeof(UnityAction<,,>).MakeGenericType(parameters[0].ParameterType, parameters[1].ParameterType, parameters[2].ParameterType),
                    4 => typeof(UnityAction<,,,>).MakeGenericType(parameters[0].ParameterType, parameters[1].ParameterType, parameters[2].ParameterType, parameters[3].ParameterType),
                    _ => null
                };
                if (delegateType == null) continue;

                var call = Delegate.CreateDelegate(delegateType, replacement, methodInfo, false);
                if (call == null) continue;

                MethodInfo removeMethod = unityEvent.GetType().GetMethod("RemoveListener");
                MethodInfo addMethod = unityEvent.GetType().GetMethod("AddListener");

                removeMethod?.Invoke(unityEvent, new object[] { call });
                addMethod?.Invoke(unityEvent, new object[] { call });
            }
        }
    }
}
