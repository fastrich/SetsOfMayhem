using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinyThings : MonoBehaviour
{
    public WorldCursor Cursor;
    //Attributes for Highlighting of Facts when Mouse-Over
    private FactObject lastFactSelection;

    //Variables for Pushout-Highlighting
    private static float timerDuration = 2.5f;
    private static float lerpTime = 1f;

    private IEnumerator rain_wait;
    private IEnumerator rain;

    public Light directionalLight;
    private Color darkColor = new(0.2f, 0.2f, 0.2f);
    private Color light_colour;
    private GameObject active_rainwork;

    public GameObject
        Fireworks_Animation,
        RainPrefab;


    private void Awake()
    {
        CommunicationEvents.PushoutFactEvent.AddListener(HighlightFact);
        CommunicationEvents.AnimateExistingFactEvent.AddListener(HighlightFact);
        CommunicationEvents.PushoutFactFailEvent.AddListener(LetItRain);
        CommunicationEvents.AnimateExistingAsSolutionEvent.AddListener(HighlightWithFireworks);

        rain = rain_wait = 0f.LerpInTime(0, 0, -1); // yield return break
    }

    public void Start()
    {
        if (Cursor == null)
            Cursor = GetComponent<WorldCursor>();

        if (directionalLight == null)
            directionalLight = FindObjectOfType<Light>();

        light_colour = directionalLight.color;
    }

    // Update is called once per frame
    public void Update()
    {
        Highlighting(Cursor.Hit);
    }

    private void Highlighting(RaycastHit hit)
    {
        FactObject selected_fact_obj = hit.transform?.GetComponentInChildren<FactObject>();

        //Set the last Fact unselected
        if (this.lastFactSelection != null
         && (selected_fact_obj == null || this.lastFactSelection != selected_fact_obj))
        {
            ApplyMaterial(lastFactSelection, lastFactSelection.Default);
            this.lastFactSelection = null;
        }

        //Set the Fact that was Hit as selected
        if (selected_fact_obj != null && hit.transform != null
            && (hit.transform.CompareTag("Selectable") || hit.transform.CompareTag("SnapZone"))
            && (this.lastFactSelection == null || this.lastFactSelection != selected_fact_obj))
        {
            ApplyMaterial(selected_fact_obj, selected_fact_obj.Selected);
            this.lastFactSelection = selected_fact_obj;
        }

        void ApplyMaterial(FactObject root, Material new_mat) =>
            root.CoroutineCascadeForChildrenAllRenderer(
                (_, renderer) =>
                    renderer.ProgrammMaterialChange(new[] {
                            (0f, lerpTime, new_mat),
                    })
                );
    }

    public static void HighlightFact(Fact startFact, FactObject.FactMaterials tmp_mat)
        {
        //this happens, but it should not! TODO: Fix Fact Hint creation
        if (startFact == null)
        {
            Debug.Log("FEHLER! fact = null");
            return;
        }
        if (startFact.Representation == null)
            return;
        FactObject selected_fact_obj = startFact.Representation.GetComponentInChildren<FactObject>();

        selected_fact_obj.CoroutineCascadeForChildrenAllRenderer(
            (fact_obj, renderer) =>
                renderer.ProgrammMaterialChange(new[] {
                    (0f, lerpTime, fact_obj.materials[(int) tmp_mat]),
                    (GlobalBehaviour.hintAnimationDuration, lerpTime, fact_obj.Default),
                })
            );
    }

    //Highlight winning fact with fireworks. material is not used;
    public void HighlightWithFireworks(Fact fact, FactObject.FactMaterials material) 
    {
        while (rain_wait.MoveNext()) ; //stop rain

        StartCoroutine(BlossomAndDie());
        HighlightFact(fact, FactObject.FactMaterials.Solution);

        IEnumerator BlossomAndDie()
        {
            GameObject firework = GameObject.Instantiate
                (Fireworks_Animation
                , fact.Representation.transform
                );

            yield return new WaitForSeconds(timerDuration);

            firework.transform.GetChild(0)
                .GetComponent<ParticleSystem>()
                .Stop();
            var sparks = firework.transform.GetChild(1)
                .GetComponent<ParticleSystem>();
            sparks.Stop();

            while (sparks.IsAlive())
                yield return null;

            GameObject.Destroy(firework);
        }
    }

    public void LetItRain(Fact startFact, Scroll.ScrollApplicationInfo info)
    {
        //-----------------
        bool restart = !rain_wait.MoveNext();

        if (restart)
        {
            StopCoroutine(rain);
            StartCoroutine(rain = BlossomAndDie());
        }
        rain_wait = 0f.LerpInTime(0, 0, timerDuration);

        IEnumerator BlossomAndDie()
        {
            Destroy(active_rainwork);
            active_rainwork = GameObject.Instantiate(RainPrefab, new Vector3(0, 40, 0), Quaternion.identity);

            Color start = directionalLight.color;
            for (IEnumerator<float> lerper = MathfExtensions.LerpInTime(0, 1, lerpTime)
                ; lerper.MoveNext();)
            {
                directionalLight.color = Color.Lerp(start, darkColor, lerper.Current);
                yield return null;
            }

            while (rain_wait.MoveNext())
                yield return null;

            for (IEnumerator<float> lerper = MathfExtensions.LerpInTime(0, 1, lerpTime)
                ; lerper.MoveNext();)
            {
                directionalLight.color = Color.Lerp(darkColor, light_colour, lerper.Current);
                yield return null;
            }

            GameObject.Destroy(active_rainwork);
        }
    }
}
