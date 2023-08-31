using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class FactExplorer : MonoBehaviour
{
    #region InspectorVariables
    [Header("PrefabComponents")]
    [SerializeField] private Transform factParentsUI;
    [SerializeField] private Transform mainFactUI;
    [SerializeField] private Transform factChildrenUI;
    [SerializeField] private Transform linesUI;

    [Header("Prefabs")]
    [SerializeField] private GameObject factSpotPrefab;
    [SerializeField] private GameObject parentLine;
    [SerializeField] private GameObject childLine;
    #endregion InspectorVariables

    #region Variables
    private Fact mainFact;
    private List<Fact> parentFacts;
    private List<Fact> childFacts;
    #endregion Variables

    #region UnityMethods
    private void Update()
    {
        DestroyIfClickedOutside();
    }

    public void Initialize(Fact fact, Vector3 factPosition)
    {
        mainFact = fact;
        parentFacts = GetParentFacts();
        childFacts = GetChildFacts();

        //Debug.Log($"Parents of {mainFact.Label}:  {string.Join(", ", parentFacts.Select(cf => cf.Label))}");
        //Debug.Log($"Children of {mainFact.Label}: {string.Join(", ", childFacts.Select(cf => cf.Label))}");

        UpdateFactExplorerUI();

        MoveToPreferredPosition(factPosition);
    }
    #endregion UnityMethods

    #region Implementation
    private List<Fact> GetParentFacts()
    {   
        _ = StageStatic.stage.factState.safe_dependencies(mainFact.Id, out var parentFactIds);
        return parentFactIds.Distinct().Select(factId => StageStatic.stage.factState[factId]).Where(f => f != mainFact).ToList();
    }

    private List<Fact> GetChildFacts()
    {
        return mainFact.getDependentFactIds().Distinct().Select(factId => StageStatic.stage.factState[factId]).ToList();
    }

    private void UpdateFactExplorerUI()
    {
        SpawnUIFacts(factParentsUI, parentFacts);
        SpawnUIFacts(mainFactUI, new List<Fact>() { mainFact });
        SpawnUIFacts(factChildrenUI, childFacts);

        // Force rebuild of FactExplorer layout, since the positions of the factObjects will be wrong otherwise
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());

        SpawnParentLines(factParentsUI.gameObject, mainFactUI);
        SpawnChildLines(factChildrenUI.gameObject, mainFactUI);
    }

    private void DestroyIfClickedOutside()
    {
        // Destroy on tab press or left click outside of FactExplorer
        if (Input.GetKeyDown(KeyCode.Tab) 
            || (Input.GetMouseButtonDown(0) && !RectTransformUtility.RectangleContainsScreenPoint(transform.GetComponent<RectTransform>(), Input.mousePosition, null)))
        {
            Destroy(gameObject);
        }
    }

    private void MoveToPreferredPosition(Vector3 prefPos)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());
        // calculate optimal position
        var deltaPos = mainFactUI.position - prefPos;
        transform.position -= deltaPos;

        // clamp position, so that no parts of the FactExplorer are out of screen
        RectTransform rect = GetComponent<RectTransform>();
        RectTransform canvasRect = GetComponentInParent<Canvas>().transform.GetComponent<RectTransform>();

        var sizeDelta = canvasRect.sizeDelta - rect.sizeDelta;
        var panelPivot = rect.pivot;
        var position = rect.anchoredPosition;
        position.x = Mathf.Clamp(position.x, -sizeDelta.x * panelPivot.x, sizeDelta.x * (1 - panelPivot.x));
        position.y = Mathf.Clamp(position.y, -sizeDelta.y * panelPivot.y, sizeDelta.y * (1 - panelPivot.y));
        rect.anchoredPosition = position;
    }
    #endregion Implementation

    #region Spawner
    private void SpawnUIFacts(Transform uiParent, List<Fact> toSpawn)
    {
        // if uiParent has no children: deactivate it
        if (toSpawn.Count == 0)
            uiParent.gameObject.SetActive(false);

        foreach (Fact f in toSpawn)
        {
            var spot = Instantiate(factSpotPrefab, uiParent);

            // TODO: this link to DisplayFacts is not ideal: maybe refactor to SciptableObject or such
            var display = f.instantiateDisplay(DisplayFacts.prefabDictionary[f.GetType()], spot.transform);
            display.transform.localPosition = Vector3.zero;
        }
    }

    private void SpawnParentLines(GameObject parent, Transform mainFactUI)
    {
        var mainTransform = mainFactUI.GetComponent<RectTransform>();
        var factWidth = mainTransform.rect.width;
        // transform.positions are weird due to LayoutGroups => manually calculate offset
        float xOffset = -factParentsUI.GetComponent<RectTransform>().rect.width / 2 +  factWidth / 2;
        float yOffset = transform.GetComponent<VerticalLayoutGroup>().spacing;
        parent.ForAllChildren(par =>
        {
            // position at the bottom center of par rect
            var position = par.transform.TransformPoint(new Vector2(0, par.GetComponent<RectTransform>().rect.yMin));
            var line = Instantiate(parentLine, position, Quaternion.identity, par.transform);

            var uiLine = line.GetComponent<UILine>();
            uiLine.points = new List<Vector2>() { Vector2.zero, new Vector2(-xOffset, -yOffset) };

            xOffset += factWidth + factParentsUI.GetComponent<HorizontalLayoutGroup>().spacing;
        });
    }

    private void SpawnChildLines(GameObject parent, Transform mainFactUI)
    {
        var mainTransform = mainFactUI.GetComponent<RectTransform>();
        var factWidth = mainTransform.rect.width;
        // transform.positions are weird due to LayoutGroups => manually calculate offset
        float xOffset = -factChildrenUI.GetComponent<RectTransform>().rect.width / 2 + factWidth / 2;
        float yOffset = -transform.GetComponent<VerticalLayoutGroup>().spacing;
        parent.ForAllChildren(par =>
        {
            // position at the top center of par rect
            var position = par.transform.TransformPoint(new Vector2(0, par.GetComponent<RectTransform>().rect.yMax));
            var line = Instantiate(childLine, position, Quaternion.identity, par.transform);

            var uiLine = line.GetComponent<UILine>();
            uiLine.points = new List<Vector2>() {
                Vector2.zero, 
                new Vector2(0, -yOffset/2),
                new Vector2(-xOffset, -yOffset/2),
                new Vector2(-xOffset, -yOffset)
            };

            xOffset += factWidth + factChildrenUI.GetComponent<HorizontalLayoutGroup>().spacing;
        });
    }
    #endregion Spawner
}
