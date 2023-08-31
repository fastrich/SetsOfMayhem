using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static JSONManager;

public class PopupBehavior : MonoBehaviour
{

    [SerializeField] GameObject canvas;
    [SerializeField] Button CloseButton;
    [SerializeField] TMP_Text message;

    private Scroll activeScroll;
    private List<GameObject> parameterDisplays;

    public string ServerErrorMessage = "unknown server error";
    public string NonTotalMessage = "Scroll application not complete";
    public string UnknownErrorMessage = "Unkown error - did you apply all facts?";
    public string InvalidAssignmentMessage = "Invalid Assignment";

    private string errorMessage = "";
    // Start is called before the first frame update
    void Awake()
    {
        CommunicationEvents.PushoutFactFailEvent.AddListener(onFailedScrollInput);

        CloseButton.onClick.RemoveAllListeners();
        CloseButton.onClick.AddListener(hidePopUp);
    }

    public void setMessage(string errorMessage)
    {
        this.message.text = errorMessage;
    }

    public void setScroll(Scroll scroll)
    {
        this.activeScroll = scroll;
    }
    public void setParameterDisplays(List<GameObject> parameters)
    {
        this.parameterDisplays = parameters;
    }

    public void showPopUp()
    {
        canvas.SetActive(true);
        StartCoroutine(hideAfter5sec());
    }

    private IEnumerator hideAfter5sec()
    {
        yield return new WaitForSeconds(5);
        hidePopUp();
    }

    public void hidePopUp()
    {
        canvas.SetActive(false);
    }

    public void onFailedScrollInput(Fact startfact, Scroll.ScrollApplicationInfo errorInfo)
    {
        setMessage(generateHelpfulMessage(errorInfo));
        showPopUp();
    }

    private string generateHelpfulMessage(Scroll.ScrollApplicationInfo errorInfo)
    {
        if(errorInfo == null)
        {
            return ServerErrorMessage;
        }
        int invAssCount = 0;
        errorMessage = "";
        for (int i = 0; i < errorInfo.errors.Length; i++) { 
            Scroll.ScrollApplicationCheckingError error = errorInfo.errors[i];
        
            if (error.kind == "nonTotal")
            {
                errorMessage += NonTotalMessage;
                errorMessage += '\n';
            } else if (error.kind == "invalidAssignment")
            {
                invAssCount++;
                Scroll.ScrollFact fact = parseFactFromError(error);
                foreach (GameObject g in parameterDisplays)
                {
                    RenderedScrollFact scrollfact = g.transform.GetChild(0).GetComponent<RenderedScrollFact>();
                    if(scrollfact.factUri == fact.@ref.uri)
                    {
                        scrollfact.ScrollParameterObject.GetComponentInChildren<ImageHintAnimation>().AnimationTrigger();
                    }
                }
            } else if (error.kind == "unknown")
            {
                errorMessage += UnknownErrorMessage;
                errorMessage += '\n';
            }
        }

        //invalid assignment message
        if(invAssCount > 0)
        {
            errorMessage += invAssCount.ToString() + " " + InvalidAssignmentMessage;
            if(invAssCount > 1) //plural for invalid assignments
            {
                errorMessage += 's';
            }
            errorMessage += '\n';
        }

        return errorMessage;
    }

    //this should be changed, the Fact Object should be parsed by JSON. This is a workaround because the MMT servers JSON serialization contains a bug
    private Scroll.ScrollFact parseFactFromError(Scroll.ScrollApplicationCheckingError error)
    {
        if(error == null || error.msg == null)
        {
            return null;
        }
        string message = error.msg;

        //cut start of string
        int indexFact = message.IndexOf('[');
        string factUri = message.Substring(indexFact + 1);

        // cut end of string
        int indexFactEnd = factUri.IndexOf(']');
        string rest = factUri.Substring(indexFactEnd);
        factUri = factUri.Substring(0, indexFactEnd);

        //get fact Label from the rest of the string
        int factNameLength = rest.IndexOf('?') - 2;
        string factLabel = rest.Substring(2, factNameLength);

        //add ?factName URI
        factUri += "?" + factLabel;

        //Debug.Log("Parsed URI: " + factUri + " parsed fact label: " + factLabel);

        foreach (Scroll.ScrollFact f in activeScroll.requiredFacts)
        {
            //Debug.Log("KIND: " + f.kind + " Label: " + f.label + " Uri: " + f.@ref.uri);
            //Debug.Log("Uri: " + f.@ref.uri);
            if (f.@ref.uri.Equals(factUri))
            {
                return f;
            }
        }
        return null;
    }
}
