using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class ToggleRotateImage : MonoBehaviour {
    [SerializeField] Transform targetGraphic; 

    Toggle _toggle;
    Toggle toggle
    {
        get { return _toggle ?? (_toggle = GetComponent<Toggle>()); }
    }

    void Awake()
    {
        toggle.onValueChanged.AddListener(OnTargetToggleValueChanged);
    }

    void OnTargetToggleValueChanged(bool on)
    {
        targetGraphic.rotation = on ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;
    }
}