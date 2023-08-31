using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(FactWrapper), typeof(RectTransform))]
public class FactFavorisation : MonoBehaviour, IPointerClickHandler
{
    #region InspectorVariables
    [Header("Prefabs")]
    [SerializeField] private GameObject favoriteDisplayPrefab;
    #endregion InspectorVariables

    #region Static Variables
    public static readonly UnityEvent<Fact, bool> ChangeFavoriteEvent = new();
    private static readonly List<Fact> favorites = new();
    #endregion Static Variables

    #region Variables
    private GameObject favoriteDisplay;
    private Fact fact;
    private const float COOLDOWN_DURATION = 0.15f; // cooldown of the double touch
    private bool touchOnCooldown = false;
    #endregion Variables

    #region Properties
    private bool isFavorite = false;
    public bool IsFavorite
    {
        get { return isFavorite; }
        set { ChangeFavoriteEvent.Invoke(fact, value); }
    }
    #endregion Properties

    #region UnityMethods
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            ToggleFavorite();
        }
    }

    private void Update()
    {
        if (!touchOnCooldown)
            HandleTouches();
    }

    private void Start()
    {
        fact = transform.GetComponent<FactWrapper>().fact;
        ChangeFavoriteEvent.AddListener(OnFavoriteChange);

        // if there already was a favoriteDisplayPrefab child (e.g. due to cloning) remove it
        gameObject.ForAllChildren(child => {
            if (child.name.StartsWith(favoriteDisplayPrefab.name))
                Destroy(child);
        });
        // instantiate new favoriteDisplay
        favoriteDisplay = Instantiate(favoriteDisplayPrefab, transform);

        // check if fact is currenty a favorite
        isFavorite = favorites.Contains(fact);

        UpdateDisplay();
    }
    #endregion UnityMethods

    #region TouchControls
    private void HandleTouches()
    {
        if (Input.touchCount != 1)
            return;

        var touch = Input.touches[0];
        if (RectTransformUtility.RectangleContainsScreenPoint(transform.GetComponent<RectTransform>(), touch.position) && touch.tapCount == 2)
        {
            StartCoroutine(Cooldown());
            ToggleFavorite();
        }
    }

    private IEnumerator Cooldown()
    {
        touchOnCooldown = true;
        yield return new WaitForSeconds(COOLDOWN_DURATION);
        touchOnCooldown = false;
    }
    #endregion TouchControls

    #region Implementation
    private void OnFavoriteChange(Fact changedFact, bool isFavorite)
    {
        if (fact == changedFact)
        {
            this.isFavorite = isFavorite;
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        favoriteDisplay.SetActive(isFavorite);
    }

    private void ToggleFavorite()
    {
        // write to property to invoke event
        IsFavorite = !IsFavorite;

        // update favorites list
        if (isFavorite)
            favorites.Add(fact);
        else
            favorites.Remove(fact);
    }
    #endregion Implementation
}
