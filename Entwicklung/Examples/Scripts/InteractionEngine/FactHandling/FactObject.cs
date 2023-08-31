using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/// <summary>
/// <see cref="Fact.Id"/>/ <c>MonoBehaviour</c> wrapper to be attached to <see cref="Fact.Representation"/>
/// </summary>
public class FactObject : MonoBehaviour, ISerializationCallbackReceiver
{
    /// <summary>
    /// <see cref="Fact.Id"/> to identify arbitrary <see cref="Fact"/> by its <see cref="Fact.Representation"/>
    /// </summary>
    public string URI;

    public enum FactMaterials
    {
        Default = 0,
        Selected = 1,
        Hint = 2,
        Solution = 3,
    }

    public Material[] materials;
    public new Renderer[] renderer;
    public List<FactObject> cascade;

    #region Unity Serialization
    public Material Default;
    public Material Selected;
    public Material Hint;
    public Material Solution;
    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        Default = materials[(int)FactMaterials.Default];
        Selected = materials[(int)FactMaterials.Selected];
        Hint = materials[(int)FactMaterials.Hint];
        Solution = materials[(int)FactMaterials.Solution];
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        materials = new Material[4];
        materials[(int)FactMaterials.Default] = Default;
        materials[(int)FactMaterials.Selected] = Selected;
        materials[(int)FactMaterials.Hint] = Hint;
        materials[(int)FactMaterials.Solution] = Solution;
    }
    #endregion

    private void Awake()
    {
        cascade = new() { this, };
        cascade.AddRange(transform.GetComponentsInChildren<FactObject>(includeInactive: true));
    }

    public void CascadeForChildren(Action<FactObject> func)
    {
        foreach (FactObject fo in cascade)
            func(fo);
    }

    public void ForAllRenderer(Action<Renderer> func)
    {
        foreach (Renderer ren in renderer)
            func(ren);
    }

    public void CoroutineCascadeForChildrenAllRenderer(Func<FactObject, Renderer, IEnumerator> func)
    {
        this.StopAllCoroutines();

        foreach (FactObject fo in cascade)
            foreach (Renderer ren in fo.renderer)
                StartCoroutine(func(fo, ren));
    }
}
