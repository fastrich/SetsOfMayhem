﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json;
using static CommunicationEvents;

public class ScrollDetails : MonoBehaviour
{
    public WorldCursor cursor;
    public GameObject parameterDisplayPrefab;
    public Scroll ActiveScroll;
    public GameObject mmtAnswerPopUp;

    public static List<GameObject> ParameterDisplays;
    private static List<Scroll.ScrollAssignment> LatestCompletions;
    private static List<Fact> LatestRenderedHints;

    public string currentMmtAnswer;

    public bool dynamicScrollDescriptionsActive = true;
    public bool automaticHintGenerationActive = true;

    // Start is called before the first frame update
    void Start()
    {
        if (cursor == null) cursor = GameObject.FindObjectOfType<WorldCursor>();

        ScrollFactHintEvent.AddListener(animateHint);
        NewAssignmentEvent.AddListener(newAssignmentTrigger);

        mmtAnswerPopUp.GetComponent<PopupBehavior>().hidePopUp();
    }

    public void setScroll(Scroll s)
    {
        Transform originalScroll = gameObject.transform.GetChild(1).transform;
        Transform originalScrollView = originalScroll.GetChild(1);
        Transform originalViewport = originalScrollView.GetChild(0);
        this.ActiveScroll = s;

        originalScroll.GetChild(0).GetComponent<TextMeshProUGUI>().text = s.description;

        //Clear all current ScrollFacts
        for (int i = 0; i < originalViewport.GetChild(0).childCount; i++) {
            GameObject.Destroy(originalViewport.GetChild(0).transform.GetChild(i).gameObject);
        }

        ParameterDisplays = new List<GameObject>();
        for (int i = 0; i < s.requiredFacts.Count; i++)
        {
            var originalObj = Instantiate(parameterDisplayPrefab, Vector3.zero, Quaternion.identity, transform);
            var originalScrollFact = originalObj.transform.GetChild(0).GetComponent<RenderedScrollFact>();
            originalScrollFact.ID = i;
            originalScrollFact.Label = s.requiredFacts[i].label;
            originalScrollFact.factUri = s.requiredFacts[i].@ref.uri;

            originalObj.transform.SetParent(originalViewport.GetChild(0));

            ParameterDisplays.Add(originalObj);
        }

        //set active scroll for ErrorMessagePopup
        PopupBehavior popup = mmtAnswerPopUp.GetComponent<PopupBehavior>();
        popup.setScroll(this.ActiveScroll);
        popup.setParameterDisplays(ParameterDisplays);
    }

    public void magicButtonTrigger() {
        StartCoroutine(magicButton());
    }

    IEnumerator magicButton()
    {
        //Non blocking wait till sendView() is finished
        yield return sendView("/scroll/apply");
        if (currentMmtAnswer == null)
        {
            Debug.Log("DAS HAT NICHT GEKLAPPT");   
            PushoutFactFailEvent.Invoke(null, null);
        }
        else
        {
            Debug.Log(currentMmtAnswer);
            Scroll.ScrollApplicationInfo pushout = JsonConvert.DeserializeObject<Scroll.ScrollApplicationInfo>(currentMmtAnswer);
            if (pushout.acquiredFacts.Count == 0)
                PushoutFactFailEvent.Invoke(null, pushout);
            readPushout(pushout.acquiredFacts);
        }
    }

    public void newAssignmentTrigger() {
        if(this.automaticHintGenerationActive || this.dynamicScrollDescriptionsActive)
            StartCoroutine(newAssignment());
    }

    IEnumerator newAssignment()
    {
        //Non blocking wait till sendView() is finished
        yield return sendView("/scroll/dynamic");

        if (currentMmtAnswer == null)
        {
            Debug.Log("DAS HAT NICHT GEKLAPPT");
        }
        else
        {
            // Todo delte maybe
            Debug.Log("Current mmt answer:  "+currentMmtAnswer);
            Scroll.ScrollDynamicInfo scrollDynamicInfo = JsonConvert.DeserializeObject<Scroll.ScrollDynamicInfo>(currentMmtAnswer);
            processScrollDynamicInfo(scrollDynamicInfo);
        }
    }

    IEnumerator sendView(string endpoint)
    {
        string body = prepareScrollAssignments();
        System.DateTime startTime = System.DateTime.UtcNow;

        using UnityWebRequest www = UnityWebRequest.Put(ServerAdress + endpoint, body);
        www.method = UnityWebRequest.kHttpVerbPOST;
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        //TODO delete below if above works fine
      //  UnityWebRequest www = UnityWebRequest.Put(ServerAdress + endpoint, body);
      //  www.method = UnityWebRequest.kHttpVerbPOST;
     //   www.SetRequestHeader("Content-Type", "application/json");
     //   var async = www.SendWebRequest();
     //   while (!async.isDone) {
            //Non blocking wait for one frame, for letting the game do other things
     //       yield return null;
      //  }
        System.DateTime twoTime = System.DateTime.UtcNow;

        if (www.result == UnityWebRequest.Result.ConnectionError 
         || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
            currentMmtAnswer = null;
        }
        else
        {
            string answer = www.downloadHandler.text;
            currentMmtAnswer = answer;
        }
        System.DateTime endTime = System.DateTime.UtcNow;

        string diffInSeconds = (endTime -startTime).TotalMilliseconds.ToString();
        string diff = (twoTime - startTime).TotalMilliseconds.ToString();


    }

    private string prepareScrollAssignments()
    {
        Fact tempFact;
        List<Scroll.ScrollAssignment> assignmentList = new();

        for (int i = 0; i < ParameterDisplays.Count; i++)
        {
            tempFact = ParameterDisplays[i].GetComponentInChildren<DropHandling>().currentFact;
            if (tempFact != null)
            {
                Scroll.ScrollAssignment listEntry = new()
                {
                    fact = new Scroll.UriReference(ActiveScroll.requiredFacts[i].@ref.uri),
                    assignment = new JSONManager.OMS(tempFact.Id)
                };
                assignmentList.Add(listEntry);
            }
        }

        Scroll.FilledScroll filledScroll = new(ActiveScroll.@ref, assignmentList);
        return JsonConvert.SerializeObject(filledScroll);
    }

    private void readPushout(List<Scroll.ScrollFact> pushoutFacts)
    {
        bool samestep = false;
        for (int i = 0; i < pushoutFacts.Count; i++, samestep = true)
        {
            //TODO Delete
            //Debug.Log(pushoutFacts.Count);
            //Debug.Log("StartUri " +pushoutFacts[i].getType()+ " over");
            //Debug.Log("Applicant" + pushoutFacts[i].getApplicant() + " over");

            //close error Window
            this.mmtAnswerPopUp.GetComponent<PopupBehavior>().hidePopUp();

            string type = pushoutFacts[i].getType();
            Fact newFact = ParsingDictionary.parseFactDictionary[pushoutFacts[i].getType()].Invoke(pushoutFacts[i]);
            if (newFact != null)
            {
                PushoutFactEvent.Invoke(FactManager.AddFactIfNotFound(newFact, out _, samestep, null, ActiveScroll.label), FactObject.FactMaterials.Solution);
                //PushoutFactEvent.Invoke(FactManager.AddFactIfNotFound(newFact, out _, samestep, null, ActiveScroll.label));

            }
            else {
                Debug.Log("Parsing on pushout-fact returned null -> One of the dependent facts does not exist");
            }
        }
    }

    public void processScrollDynamicInfo(Scroll.ScrollDynamicInfo scrollDynamicInfo) {

        if (scrollDynamicInfo.completions.Count != 0)
            LatestCompletions = scrollDynamicInfo.completions[0];
        else
            LatestCompletions = new List<Scroll.ScrollAssignment>();

        LatestRenderedHints = new List<Fact>();

        List<string> hintUris = new();
        foreach (Scroll.ScrollAssignment currentCompletion in LatestCompletions) {
            hintUris.Add(currentCompletion.fact.uri);
        }

        //Update Scroll, process data for later hints and update Uri-List for which hints are available
        hintUris = processRenderedScroll(scrollDynamicInfo.rendered, hintUris);

        if (this.automaticHintGenerationActive)
        {
            //Show that Hint is available for ScrollParameter
            HintAvailableEvent.Invoke(hintUris);
        }
    }

    public List<string> processRenderedScroll(Scroll rendered, List<string> hintUris)
    {
        Transform scroll = gameObject.transform.GetChild(1).transform;

        if (this.dynamicScrollDescriptionsActive)
        {
            //Update scroll-description
            scroll.GetChild(0).GetComponent<TextMeshProUGUI>().text = rendered.description;
        }

        for (int i = 0; i < rendered.requiredFacts.Count; i++)
        {
            var obj = ParameterDisplays.Find(x => x.transform.GetChild(0).GetComponent<RenderedScrollFact>().factUri.Equals(rendered.requiredFacts[i].@ref.uri));

            if (this.dynamicScrollDescriptionsActive)
            {
                //Update ScrollParameter label
                obj.transform.GetChild(0).GetComponent<RenderedScrollFact>().Label = rendered.requiredFacts[i].label;
            }

            //Check Hint Informations
            //If ScrollFact is assigned -> No Hint
            if (obj.transform.GetChild(0).GetComponent<DropHandling>().currentFact == null) {

              //  Debug.Log(" print out " + rendered.requiredFacts[i].getType());
                Fact currentFact = ParsingDictionary.parseFactDictionary[rendered.requiredFacts[i].getType()].Invoke(rendered.requiredFacts[i]);
                //If currentFact could be parsed: this fact maybe not yet exists in the global fact-list but there must be a fact
                // of the same type and the same dependent facts in the fact-list, otherwise currentFact could not have been parsed

                //If the fact could not be parsed -> Therefore not all dependent Facts exist -> No Hint
                //AND if fact has no dependent facts -> No Hint
                if (currentFact != null && currentFact.hasDependentFacts())
                {
                    //Hint available for abstract-problem uri
                    hintUris.Add(currentFact.Id);
                    LatestRenderedHints.Add(currentFact);
                }
            }
        }

        return hintUris;
    }

    public void animateHint(GameObject scrollParameter, string scrollParameterUri) {
        Scroll.ScrollAssignment suitableCompletion = LatestCompletions.Find(x => x.fact.uri.Equals(scrollParameterUri) );
        Fact fact;

        if (suitableCompletion != null)
        {
            if (StageStatic.stage.factState.ContainsKey(suitableCompletion.assignment.uri))
            {
                fact = StageStatic.stage.factState[suitableCompletion.assignment.uri];
                //Animate ScrollParameter
                scrollParameter.GetComponentInChildren<ImageHintAnimation>().AnimationTrigger();
                //Animate Fact in FactPanel
                AnimateExistingFactEvent.Invoke(fact, FactObject.FactMaterials.Hint);
            }
        }
        else if (LatestRenderedHints.Exists(x => x.Id.Equals(scrollParameterUri)))
        {
            fact = LatestRenderedHints.Find(x => x.Id.Equals(scrollParameterUri));
            var factId = fact.Id;

            //If there is an equal existing fact -> Animate that fact AND ScrollParameter
            if (StageStatic.stage.factState.ContainsKey(factId))
            {
                Fact existingFact = StageStatic.stage.factState[factId];

                //Animate ScrollParameter
                scrollParameter.GetComponentInChildren<ImageHintAnimation>().AnimationTrigger();
                //Animate Fact in FactPanel
                AnimateExistingFactEvent.Invoke(existingFact, FactObject.FactMaterials.Hint);
            }
            //If not -> Generate a Fact-Representation with such dependent facts
            else
            {
                //Animate ScrollParameter
                scrollParameter.GetComponentInChildren<ImageHintAnimation>().AnimationTrigger();
                //Generate new FactRepresentation and animate it
                AnimateNonExistingFactEvent.Invoke(fact);
            }
        }
    }

    public void animateInvalidAssignedFact(GameObject scrollParameter)
    {
        //Animate ScrollParameter
        scrollParameter.GetComponentInChildren<ImageHintAnimation>().AnimationTrigger();
    }

    public void animateScrollParameter(string label)
    {
        var obj = ParameterDisplays.Find(x => x.transform.GetChild(0).GetComponent<RenderedScrollFact>().Label == label);
        obj.GetComponentInChildren<Animator>().SetTrigger("animateHint");
    }
}
