using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TestInput : MonoBehaviour
{
    private ControlMapping input_ControlMapping;
    //private RebindUISampleActions input_ControlMapping;
    public Image image;
    public Color imageColorToBeUsed = Color.green;
    public Color imageColorToBeUsed_w = Color.white;
    public float currentImageColorAlpha = 0.5f;
    // public InputActionAsset actions;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        //New InputSystem
        input_ControlMapping = new ControlMapping();
        input_ControlMapping.Actionmap1.Movement.Enable();

    }
    private void OnEnable()
    {
        input_ControlMapping = new ControlMapping();
        input_ControlMapping.Actionmap1.Movement.Enable(); 
       

    }

    private void OnDisable()
    {
       
        input_ControlMapping.Actionmap1.Movement.Disable();
    }


        // Update is called once per frame
        void Update()
    {


        image = GetComponent<Image>();
        //Get the alpha value of Color
        currentImageColorAlpha = image.color.a;
        //Assign Color

        float vertical = input_ControlMapping.Actionmap1.Movement.ReadValue<Vector2>().x;
        if (vertical != 0)
        {
            image.color = imageColorToBeUsed;
        }
        else
        {
            image.color = imageColorToBeUsed_w;

        }
        
    }
}
