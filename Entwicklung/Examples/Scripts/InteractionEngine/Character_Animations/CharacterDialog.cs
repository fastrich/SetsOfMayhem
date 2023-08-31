using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static TaskCharakterAnimation;
using static UIconfig;

public class CharacterDialog : MonoBehaviour
{
    public RaycastHit Hit;
    public TextMeshPro textDisplay;
    public TextMeshPro textHint;
    public TaskCharakterAnimation CharakterAnimation;
    public string[] sentences;
    private int sentenceIndex;
    private int letterIndex = 0;
    private bool typingActive = false;
    //speed for typing the Text
    public float typingSpeed;
    private float timer = 0;
    private double numinputtrigger = 0;
    public float waitingBetweenInputs = 0.2f;
    private int idleTime = 0;
    private int oversentence = 0;

    //Only reset once after Player is out of range of the TaskCharacter
    private bool textReseted = true;
    //If gameSucceeded -> Disable Talking with TaskCharacter
    private bool gameSucceeded = false;

    private ControlMapping input_ControlMapping;

    // Start is called before the first frame update
    void Start()
    {
        CommunicationEvents.gameSucceededEvent.AddListener(StartGameSucceededSentence);
        CommunicationEvents.gameNotSucceededEvent.AddListener(StopGameSucceededSentence);
        //Type first sentence
        typingActive = true;
        TypeFkt();
        StartCoroutine(IdleTimer());
    }

    private void Awake()
    {
        //New InputSystem
        input_ControlMapping = new ControlMapping();
        input_ControlMapping.Actionmap1.Talking.Enable();
        

    }
    private void OnEnable()
    {
        input_ControlMapping.Actionmap1.Talking.Enable();

    }

    private void OnDisable()
    {
        input_ControlMapping.Actionmap1.Talking.Disable();

    }

    


    public void nextSentenceBttn_pressed()
    {
        if (numinputtrigger == 0)
        {
                numinputtrigger++;
                StartCoroutine(slowInput());
                //Type Next sentence if player is in the talkinZone around the TaskCharacter AND the player typed the return-Key
                idleTime = 0;
                NextSentence();

        }
    }


    private void Update()
    {
        TypeFkt();

        if (!gameSucceeded && CharakterAnimation.getPlayerInTalkingZone())
        {


            //print("InTalkingZone");
            //print("CharacterDialog" + gameSucceeded + " && " + CharakterAnimation.getPlayerInTalkingZone());

            if (UIconfig.InputManagerVersion == 1) {
                if (Input.GetKeyDown(KeyCode.C)) {
                    //Type Next sentence if player is in the talkinZone around the TaskCharacter AND the player typed the return-Key
                    NextSentence();
                    idleTime = 0;
                    //print("CharacterDialogLos1");
                }
            }
            if ((nextDialogPlease > 0 || UIconfig.InputManagerVersion == 2) && numinputtrigger == 0)
            {
                if ((nextDialogPlease > 0 || input_ControlMapping.Actionmap1.Talking.ReadValue<float>() != 0))
                {
                    nextSentenceBttn_pressed();

                }
            }
        }
        
        if ((!gameSucceeded) && (!textReseted) )
        {
            if ((!CharakterAnimation.getPlayerInTalkingZone()) || (idleTime > 7)) 
            {
                //Reset Sentence if Player is out of range of the TaskCharacter and it's not already reseted
                ResetSentence();
                idleTime = 0;
            }
        }
    }

    IEnumerator IdleTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (idleTime < 100 && textReseted == false) { idleTime++; }
            //print(idleTime);
        }

    }


    public void TypeFkt() {
        if (typingActive)
        {
            if (this.timer >= this.typingSpeed)
            {
                if (letterIndex < sentences[sentenceIndex].Length)
                {
                    textDisplay.text += sentences[sentenceIndex].ToCharArray()[letterIndex];
                    letterIndex++;
                }
                else
                {
                    this.typingActive = false;
                    letterIndex = 0;
                }

                this.timer = 0;
            }
            else
            {
                this.timer += Time.deltaTime;
            }
        }
    }


    public void NextSentence() {
        //-2 because the last sentence is only for SucceededPushout-Purposes
        if (sentenceIndex < sentences.Length - 2)
        {
            CharakterAnimation.setTaskCharacterAddressed(true);
            sentenceIndex++;
            letterIndex = 0;
            typingActive = true;
            timer = 0;
            textDisplay.text = "";
            TypeFkt();
            textReseted = false;
        }
        else {
            letterIndex = 0;
            typingActive = false;
            timer = 0;
            textReseted = false;
            textDisplay.text = "";
            oversentence++;
            if (oversentence > 2)
            {
                ResetSentence();
                oversentence = 0;
            }
        }
    }

    public void ResetSentence() {
        CharakterAnimation.setTaskCharacterAddressed(false);
        sentenceIndex = 0;
        letterIndex = 0;
        typingActive = true;
        timer = 0;
        textDisplay.text = "";
        //Type first sentence again
        TypeFkt();
        textReseted = true;
    }

    public void StartGameSucceededSentence()
    {
        if (!gameSucceeded)
        {
            textDisplay.text = "";
            //Last Sentence is the Pushout-Sentence
            sentenceIndex = sentences.Length - 1;
            letterIndex = 0;
            typingActive = true;
            timer = 0;
            gameSucceeded = true;
            //Disable Hint With Enter-Key for Talking
            textHint.GetComponent<MeshRenderer>().enabled = false;
            //Type final message
            TypeFkt();
        }
    }

    public void StopGameSucceededSentence()
    {
        if (gameSucceeded)
        {
            gameSucceeded = false;
            //Enable Hint With Enter-Key for Talking
            textHint.GetComponent<MeshRenderer>().enabled = true;
            ResetSentence();
        }
    }



    IEnumerator slowInput()
    {

        yield return new WaitForSecondsRealtime(waitingBetweenInputs);

        numinputtrigger = 0;
        nextDialogPlease = 0;


        yield return null;

    }

}
