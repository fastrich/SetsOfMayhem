using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static CommunicationEvents;
using System.Linq;
using UnityEngine.UI;
using System.Collections;

public class DisplayFacts : MonoBehaviour
{
    public static Dictionary<Type, GameObject> prefabDictionary;
    public static Dictionary<string, GameObject> displayedFacts = new();

    [SerializeField] private Transform factscreenContent;
    [SerializeField] private GameObject factSpotPrefab;

    private bool sortDescending = false;
    private bool showGrouped = false;
    private bool showOnlyFavorites = false;

    [Header("FactPrefabs")]
    public GameObject prefab_Point;
    public GameObject prefab_Distance;
    public GameObject prefab_Angle;
    public GameObject prefab_Default;
    public GameObject prefab_OnLine;
    public GameObject prefab_Line;
    public GameObject prefab_ParallelLineFact;
    public GameObject prefab_RectangleFact;
    public GameObject prefab_RadiusFact;
    public GameObject prefab_AreaCircle;
    public GameObject prefab_ConeVolume;
    public GameObject prefab_OrthogonalCircleLine;
    public GameObject prefab_TruncatedConeVolume;
    public GameObject prefab_RightAngle;
    public GameObject prefab_CylinderVolume;
    public GameObject prefab_EqualFact;
    public GameObject prefab_UnEqualFact;

    public GameObject prefab_CircleFact;
    public GameObject prefab_OnCircleFact;
    public GameObject prefab_AngleCircleLineFact;

    public GameObject prefab_TestFact;

    #region UnityMethods
    //Start is called before the first frame update
    void Start()
    {
        prefabDictionary = new Dictionary<Type, GameObject>() {
            {typeof(PointFact), prefab_Point},
            {typeof(LineFact), prefab_Distance},
            {typeof(RayFact), prefab_Line},
            {typeof(AngleFact), prefab_Angle},
            {typeof(OnLineFact), prefab_OnLine},
            {typeof(ParallelLineFact), prefab_ParallelLineFact},

            {typeof(CircleFact), prefab_CircleFact},
            {typeof(OnCircleFact), prefab_OnCircleFact},
            {typeof(AngleCircleLineFact), prefab_AngleCircleLineFact},
            {typeof(RadiusFact), prefab_RadiusFact},
            {typeof(AreaCircleFact), prefab_AreaCircle},
            {typeof(ConeVolumeFact), prefab_ConeVolume},
            {typeof(OrthogonalCircleLineFact), prefab_OrthogonalCircleLine },
            {typeof(TruncatedConeVolumeFact), prefab_TruncatedConeVolume },
            {typeof(RightAngleFact), prefab_RightAngle },
            {typeof(CylinderVolumeFact), prefab_CylinderVolume},
            {typeof(EqualCirclesFact), prefab_EqualFact },
            {typeof(UnEqualCirclesFact), prefab_UnEqualFact },

            {typeof(TestFact), prefab_TestFact },
        };

        AddFactEvent.AddListener(AddFact);
        RemoveFactEvent.AddListener(RemoveFact);
        //AnimateExistingFactEvent.AddListener(AnimateFact);
        FactFavorisation.ChangeFavoriteEvent.AddListener(OnFavoriteChange);
    }
    #endregion UnityMethods

    #region Implementation
    public void AddFact(Fact fact) {
        // index where the new display will be inserted
        int siblingIdx = sortDescending ? 0 : factscreenContent.childCount;
        if (showGrouped)
        {
            var facts = GetChildObjects(factscreenContent.transform).Select(c => c.GetComponentInChildren<FactWrapper>().fact).ToList();
            if (!sortDescending)
                siblingIdx = GetIndexInSortedList(fact, facts);
            else
            {
                facts.Reverse();
                var _siblingIdx = GetIndexInSortedList(fact, facts);
                siblingIdx = factscreenContent.childCount - _siblingIdx;
            }
        }

        // create display
        var display = CreateDisplay(transform, fact);
        display.transform.localPosition = Vector3.zero;
        displayedFacts.Add(fact.Id, display);

        // disable if showOnlyFavorites is true and fact is no favorite
        display.transform.parent.gameObject.SetActive(!(showOnlyFavorites && !display.GetComponent<FactFavorisation>().IsFavorite));
        
        display.transform.parent.transform.SetSiblingIndex(siblingIdx);
    }

    private GameObject CreateDisplay(Transform transform, Fact fact)
    {
        var spot = Instantiate(factSpotPrefab, factscreenContent);
        return fact.instantiateDisplay(prefabDictionary[fact.GetType()], spot.transform);
    }

    public void RemoveFact(Fact fact)
    {
        // destroy factSpot (parent of displayed fact) and the fact display with it
        Destroy(displayedFacts[fact.Id].transform.parent.gameObject);
        displayedFacts.Remove(fact.Id);
    }

    public void AnimateFact(Fact fact) {
        var factIcon = displayedFacts[fact.Id];
        factIcon.GetComponentInChildren<ImageHintAnimation>().AnimationTrigger();
    }

    #region Sorting
    #region AscDesc
    public void AscDescChanged(Toggle t)
    {
        sortDescending = !sortDescending;

        // revert current order
        var children = GetChildObjects(factscreenContent.transform);
        foreach (var child in children)
        {
            child.SetAsFirstSibling();
        }
    }
    #endregion AscDesc

    #region Grouping
    public void GroupingChanged(Toggle t)
    {
        showGrouped = t.isOn;

        List<Transform> vals = GetChildObjects(factscreenContent.transform);
        List<Transform> ordered = new();
        if (showGrouped)
        {
            var comparer = new FactTypeComparer();
            ordered = vals.OrderBy(tr => tr.GetComponentInChildren<FactWrapper>().fact, comparer).ToList();
        }
        else
            ordered = vals.OrderBy(tr => displayedFacts.Keys.ToList().IndexOf(tr.GetComponentInChildren<FactWrapper>().fact.Id)).ToList();

        if (sortDescending)
            ordered.Reverse();

        for (int i = 0; i < ordered.Count; i++)
            ordered[i].transform.SetSiblingIndex(i);
    }
    private int GetIndexInSortedList(Fact f, List<Fact> toCheck)
    {
        var index = toCheck.BinarySearch(f, new FactTypeComparer());
        if (index < 0) index = ~index;
        return index;
    }

    internal class FactTypeComparer : IComparer<Fact>
    {
        /// <summary>
        /// Compare two facts by type and label
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(Fact x, Fact y)
        {
            if (x.GetType() == y.GetType()) // same type: compare labels
                return string.Compare(x.Label, y.Label);
            else // different types: compare type
                return string.Compare(x.GetType().ToString(), y.GetType().ToString());
        }
    }
    #endregion Grouping
    #endregion Sorting

    #region Favorites
    public void FavoritesFilterChanged(Toggle t)
    {
        showOnlyFavorites = t.isOn;
        if (!showOnlyFavorites) // show all
            displayedFacts.Values.ToList().ForEach(nFav => nFav.transform.parent.gameObject.SetActive(!showOnlyFavorites));
        else
        {
            // hide not favorites
            var notFavorites = displayedFacts.Values.Where(go => !go.GetComponent<FactFavorisation>().IsFavorite).ToList();
            notFavorites.ForEach(nFav => nFav.transform.parent.gameObject.SetActive(false));
        }
    }

    private void OnFavoriteChange(Fact changedFact, bool isFavourite)
    {
        if (!showOnlyFavorites)
            return;

        var id = changedFact.Id;
        if (displayedFacts.ContainsKey(id))
            displayedFacts[id].transform.parent.gameObject.SetActive(isFavourite);
    }
    #endregion Favorites

    #region Helper
    private static List<Transform> GetChildObjects(Transform parent)
    {
        List<Transform> children = new();
        foreach (Transform val in parent)
            children.Add(val);
        return children;
    }
    #endregion Helper
    #endregion Implementation
}
