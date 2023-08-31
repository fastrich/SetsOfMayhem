using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static CommunicationEvents;

public class RenderedScrollFact : MonoBehaviour
{

    public int ID;
    public TextMeshProUGUI LabelMesh;
    private string _label;
    public string factUri;

    public GameObject ScrollParameterObject;

    public string Label
    {
        get => _label;
        set
        {
            if (_label == value) return;
            _label = value;
            LabelMesh.text = value;
        }
    }

    void Start()
    {
        HintAvailableEvent.AddListener(OnHintAvailable);
    }

    public void OnClickHintButton()
    {
        ScrollFactHintEvent.Invoke(this.ScrollParameterObject, factUri);
    }

    public void OnHintAvailable(List<string> uris)
    {
        GameObject hintButton = ScrollParameterObject.transform.GetChild(2).gameObject;
        
        hintButton.SetActive(value: uris.Contains(factUri));
    }
}
