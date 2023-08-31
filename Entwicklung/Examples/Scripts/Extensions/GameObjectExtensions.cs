using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static void SetActiveByTagRecursive(this GameObject root, string tag, bool enable)
        => root.ForAllChildren(child => {
            if (child.CompareTag(tag))
                child.SetActive(enable);
            else
                SetActiveByTagRecursive(child, tag, enable);
        });

    public static void DestroyAllChildren(this GameObject root)
        => root.ForAllChildren(child => GameObject.Destroy(child));

    public static void SetActiveAllChildren(this GameObject root, bool active)
        => root.ForAllChildren(child => child.SetActive(active));

    public static void ForAllChildren(this GameObject root, Action<GameObject> func_on_child)
    {
        for (int i = 0; i < root.transform.childCount; i++)
            func_on_child(root.transform.GetChild(i).gameObject);
    }

    public static GameObject GetNthChild(this GameObject root, IEnumerable<int> pos)
    {
        GameObject ret = root;
        foreach (var i in pos)
            ret = ret.transform.GetChild(i).gameObject;

        return ret;
    }
}
