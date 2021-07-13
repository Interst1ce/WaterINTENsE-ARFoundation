using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MaintenanceScenario : MonoBehaviour
{

    [SerializeField]
    Text debugText;
    [SerializeField]
    Text debugText2;
    [SerializeField]
    Text ampValue;
    [SerializeField]
    Text dripValue;
    [SerializeField]
    Text thermoValue;
    [SerializeField]
    GameObject AmpReader;
    [SerializeField]
    GameObject dripReader;
    [SerializeField]
    GameObject thermoReader;
    [SerializeField]
    GameObject ampSelect;
    [SerializeField]
    GameObject dripSelect;
    [SerializeField]
    GameObject thermoSelect;
    [SerializeField]
    GameObject answerYes;
    [SerializeField]
    GameObject answerNo;
    [SerializeField]
    GameObject endButton;
    [SerializeField]
    GameObject playButton;
    [SerializeField]
    GameObject resizeSceneButton;
    [SerializeField]
    GameObject valueWithinRangeQ;

    [SerializeField]
    GameObject pump;
    [SerializeField]
    GameObject motor;
    [SerializeField]
    GameObject bearing;
    [SerializeField]
    GameObject seal;
    
    [SerializeField]
    Material glowMat;
    [SerializeField]
    Material motorMat;
    [SerializeField]
    Material bearingMat;
    [SerializeField]
    Material sealMat;

    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip correctAudio;
    [SerializeField]
    AudioClip incorrectAudio;
    [SerializeField]
    AudioClip partWorking;
    [SerializeField]
    AudioClip replacePart;
    [SerializeField]
    AudioClip introClip;

    [SerializeField]
    Animator pumpAnimator;

    [SerializeField]
    PauseMenu pauseMenu;


    [SerializeField]
    ARObjectPlacement aRObjectPlacement;

    Scenario meterSelection;
    float ampMeterValue;
    float thermMeterValue;
    float dripMeterValue;

    bool ampCompleted = false;
    bool dripCompleted = false;
    bool thermCompleted = false;

    bool ampButtonDisable = false;
    bool dripButtonDisable = false;
    bool thermoButtonDisable = false;



    
    public enum Scenario 
    { 
    
        amp,
        drip,
        therm
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = introClip;
        //StartScenarioModule();
        StartCoroutine(MeterCompleteCheck());

        AmpReader.SetActive(false);
        dripReader.SetActive(false);
        thermoReader.SetActive(false);
        answerYes.SetActive(false);
        answerNo.SetActive(false);

        ampSelect.SetActive(false);
        dripSelect.SetActive(false);
        thermoSelect.SetActive(false);

        endButton.SetActive(false);
        valueWithinRangeQ.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.isPlaying == true)
        {

            ampSelect.GetComponent<Button>().interactable = false;
            thermoSelect.GetComponent<Button>().interactable = false;
            dripSelect.GetComponent<Button>().interactable = false;
            
        }
        if (!audioSource.isPlaying)
        {

            ampSelect.GetComponent<Button>().interactable = true;
            thermoSelect.GetComponent<Button>().interactable = true;
            dripSelect.GetComponent<Button>().interactable = true;
            

        }
    }
    public void StartScenarioModule() 
    {
        StartCoroutine(PlayIntroAudio());
       //StartCoroutine(DisableButtonDuringAudio());
        
    }

    public IEnumerator PlayIntroAudio() 
    {
        debugText.text = "PlayIntroAudio()";

        while (true) 
        {
            if(aRObjectPlacement.sceneSpawned == true) 
            {
                ampSelect.SetActive(true);
                dripSelect.SetActive(true);
                thermoSelect.SetActive(true);
                audioSource.clip = introClip;
                audioSource.Play();
                break;            
            
            }
            yield return null;
        }

        //not playing intro audio 
        //debugText.text = "PlayIntroAudio()"     
        
    }


    //win condition logic (all meters complete)
    public IEnumerator MeterCompleteCheck() 
    {
        while (true) 
        {
            if (ampButtonDisable)
            {

                ampSelect.SetActive(false);

            }
            if (dripButtonDisable)
            {

                dripSelect.SetActive(false);

            }
            if (thermoButtonDisable)
            {

                thermoSelect.SetActive(false);

            }
            if(thermoButtonDisable && dripButtonDisable && ampButtonDisable) 
            {
                if (!audioSource.isPlaying && !PauseMenu.paused)
                {

                    debugText.text = "All buttons disabled";
                    
                    answerYes.SetActive(false);
                    answerNo.SetActive(false);
                    dripReader.SetActive(false);
                    thermoReader.SetActive(false);
                    ampSelect.SetActive(false);
                    dripSelect.SetActive(false);
                    thermoSelect.SetActive(false);
                    
                    
                    pauseMenu.Pause();
                    
                    playButton.GetComponent<Button>().interactable = false;
                    resizeSceneButton.GetComponent<Button>().interactable = false;
                    endButton.SetActive(true);
                    break;

                }
            
            }

            yield return null;

        }
        yield return null;
    }

    public void PauseDisableButtons() 
    {

        answerYes.GetComponent<Button>().interactable = false;
        answerNo.GetComponent<Button>().interactable = false;

        

        ampSelect.SetActive(false);
        dripSelect.SetActive(false);
        thermoSelect.SetActive(false);

        



        //finished = false;
        endButton.SetActive(false);


    }

    public void PauseMenuReenable() 
    {
        if(!answerYes.activeInHierarchy && !answerNo.activeInHierarchy) 
        {

            ampSelect.SetActive(true);
            dripSelect.SetActive(true);
            thermoSelect.SetActive(true);

        }
 

        answerYes.GetComponent<Button>().interactable = true;
        answerNo.GetComponent<Button>().interactable = true;


    }
    public void ReloadScene()
    {


        answerYes.SetActive(false);
        answerNo.SetActive(false);
        dripReader.SetActive(false);
        thermoReader.SetActive(false);
        ampSelect.SetActive(true);
        dripSelect.SetActive(true);
        thermoSelect.SetActive(true);


        ampButtonDisable = false;
        dripButtonDisable = false;
        thermoButtonDisable = false;

        ampCompleted = false;
        dripCompleted = false;
        thermCompleted = false;

        pumpAnimator.SetBool("ReplaceMotor", false);
        pumpAnimator.SetBool("MotorIsReplaced", false);

        pumpAnimator.SetBool("ReplaceBearing", false);
        pumpAnimator.SetBool("BearingIsReplaced", false);

        pumpAnimator.SetBool("ReplaceSeal", false);
        pumpAnimator.SetBool("SealIsReplaced", false);


        //finished = false;
        endButton.SetActive(false);

        audioSource.clip = introClip;
        

        pauseMenu.Pause();
        audioSource.Play();

        playButton.GetComponent<Button>().interactable = true;
        resizeSceneButton.GetComponent<Button>().interactable = true;

        StartCoroutine(MeterCompleteCheck());
    }


    public void StartAmpScenario()
    {
        StartCoroutine(AmpScenario());
    }

    public void StartThermScenario()
    {
        StartCoroutine(ThermScenario());
    }

    public void StartDripScenario()
    {
        StartCoroutine(DripScenario());
    }



    public IEnumerator AmpScenario()
    {


        //holds meter selected and passses to answer check functions
        meterSelection = Scenario.amp;

        motor.GetComponent<MeshRenderer>().material = glowMat;
        bearing.GetComponent<MeshRenderer>().material = bearingMat;
        seal.GetComponent<MeshRenderer>().material = sealMat;
        

        AmpReader.SetActive(true);
        ampSelect.SetActive(false);
        dripSelect.SetActive(false);
        thermoSelect.SetActive(false);

        answerYes.SetActive(true);
        answerNo.SetActive(true);

        valueWithinRangeQ.SetActive(true);
        while (true)
        {
            //coroutine entry check
            debugText.text = "we're in my coroutine!";

           float scenarioCorrectIncorrect = Random.value;
            
            //scenarioCorrectIncorrect = 1f;
            //scenarioCorrectIncorrect == 1f

            //delete this, replace it with scenarioCorrectIncorrect<0.5f
            if (scenarioCorrectIncorrect < .05f) 
            {

                debugText.text = "AmpNotWithinRange";

                //scenario will produce a value NOT WITHIN the acceptable range of values on AMP meter
                string ampWrongValue = Random.Range(11.5f, 20f).ToString();

                //storing value in meter text display 
                ampValue.text = ampWrongValue;

                break;
                //CheckForCorrectAnswerYes(meterSelection, ampValue.text);
            }
            else 
            {

                debugText.text = "AmpWithinRange";

                //scenario will produce a value WITHIN the acceptable ranges on AMP meter
                string ampCorrectValue = Random.Range(0f, 11.5f).ToString();

                //storing value in meter text display 
                ampValue.text = ampCorrectValue;
                break;
                //for testing
                //CheckForCorrectAnswerYes(meterSelection, ampValue.text);

            }


            yield return null;
        }
        yield return null;
    }

    //****TODO: create coroutines for similar to AmpScenario****
    public IEnumerator ThermScenario()
    {
        meterSelection = Scenario.therm;

        bearing.GetComponent<MeshRenderer>().material = glowMat;
        seal.GetComponent<MeshRenderer>().material = sealMat;
        motor.GetComponent<MeshRenderer>().material = motorMat;


        thermoReader.SetActive(true);
        ampSelect.SetActive(false);
        dripSelect.SetActive(false);
        thermoSelect.SetActive(false);

        answerYes.SetActive(true);
        answerNo.SetActive(true);

        valueWithinRangeQ.SetActive(true);
        while (true)
        {
            //coroutine entry check
            debugText.text = "we're in my coroutine!";

            float scenarioCorrectIncorrect = Random.value;
            
            //scenarioCorrectIncorrect = 1f;
            //scenarioCorrectIncorrect == 1f;

            //delete this, replace it with scenarioCorrectIncorrect<0.5f
            if (scenarioCorrectIncorrect < 0.5f)
            {

                debugText.text = "ThermNotWithinRange";

                //scenario will produce a value NOT WITHIN the acceptable range of values on AMP meter
                string thermWrongValue = Random.Range(239f, 350f).ToString();

                //storing value in meter text display 
                thermoValue.text = thermWrongValue;
                break;
                //CheckForCorrectAnswerYes(meterSelection, ampValue.text);
            }
            else
            {

                debugText.text = "ThermWithinRange";

                //scenario will produce a value WITHIN the acceptable ranges on AMP meter
                string thermCorrectValue = Random.Range(0f, 238f).ToString();

                //storing value in meter text display 
                thermoValue.text = thermCorrectValue;
                break;
                //for testing
                //CheckForCorrectAnswerYes(meterSelection, ampValue.text);

            }
            yield return null;
        }
        yield return null;
    }

    public IEnumerator DripScenario()
    {
        meterSelection = Scenario.drip;

        seal.GetComponent<MeshRenderer>().material = glowMat;
        bearing.GetComponent<MeshRenderer>().material = bearingMat;        
        motor.GetComponent<MeshRenderer>().material = motorMat;

        dripReader.SetActive(true);
        ampSelect.SetActive(false);
        dripSelect.SetActive(false);
        thermoSelect.SetActive(false);

        answerYes.SetActive(true);
        answerNo.SetActive(true);

        valueWithinRangeQ.SetActive(true);
        while (true)
        {
            //coroutine entry check
            debugText.text = "we're in my coroutine!";

            float scenarioCorrectIncorrect = Random.value;

            //scenarioCorrectIncorrect = 1f;
            //scenarioCorrectIncorrect == 1f;

            //delete this, replace it with scenarioCorrectIncorrect<0.5f
            if (scenarioCorrectIncorrect < 0.5f)
            {

                debugText.text = "DripNotWithinRange";

                //scenario will produce a value NOT WITHIN the acceptable range of values on AMP meter
                string dripWrongValue = Random.Range(0, 2).ToString();

                //storing value in meter text display 
                dripValue.text = dripWrongValue;
                break;
                //CheckForCorrectAnswerYes(meterSelection, ampValue.text);
            }
            else
            {

                debugText.text = "DripWithinRange";

                //scenario will produce a value WITHIN the acceptable ranges on AMP meter
                string dripCorrectValue = Random.Range(30, 50).ToString();

                //storing value in meter text display 
                dripValue.text = dripCorrectValue;
                break;
                //for testing
                //CheckForCorrectAnswerYes(meterSelection, ampValue.text);

            }
            yield return null;
        }

        yield return null;
    }
    //checking to see if the meter values are within acceptable ranges. The user answered YES in this situation, all possible cases below.
    public void CheckForCorrectAnswerYes() 
    {
        float ampMeterValue = float.Parse(ampValue.text);
        float thermMeterValue = float.Parse(thermoValue.text);
        float dripMeterValue = float.Parse(dripValue.text);

        
        switch (meterSelection) 
        {
            case Scenario.amp:
                if (ampMeterValue >11.5f) 
                {
                    
                    debugText.text = "incorrect";
                    audioSource.clip = incorrectAudio;
                    audioSource.Play();

                    motor.GetComponent<MeshRenderer>().material = motorMat;

                    answerYes.SetActive(false);
                    answerNo.SetActive(false);
                    dripReader.SetActive(false);
                    thermoReader.SetActive(false);
                    ampSelect.SetActive(true);
                    dripSelect.SetActive(true);
                    thermoSelect.SetActive(true);

                    valueWithinRangeQ.SetActive(false);

                    if (ampButtonDisable)
                    {

                        ampSelect.SetActive(false);

                    }
                    if (dripButtonDisable)
                    {

                        dripSelect.SetActive(false);

                    }
                    if (thermoButtonDisable)
                    {

                        thermoSelect.SetActive(false);

                    }


                }
                else 
                {

                    debugText.text = "correct";

                    audioSource.clip = partWorking;
                    audioSource.Play();
                    motor.GetComponent<MeshRenderer>().material = motorMat;
                    //PlayCorrectSelectionSequence(pumpAnimator, audioSource);

                    answerYes.SetActive(false);
                    answerNo.SetActive(false);
                    AmpReader.SetActive(false);
                    dripReader.SetActive(false);
                    thermoReader.SetActive(false);

                    ampSelect.SetActive(false);
                    dripSelect.SetActive(true);
                    thermoSelect.SetActive(true);

                    valueWithinRangeQ.SetActive(false);

                    ampCompleted = true;
                    ampButtonDisable = true;
      
                    
                }
                break;

            case Scenario.therm:

                if (thermMeterValue > 238f)
                {
                    //
                    debugText.text = "incorrect";
                    audioSource.clip = incorrectAudio;
                    audioSource.Play();

                    seal.GetComponent<MeshRenderer>().material = sealMat;

                    answerYes.SetActive(false);
                    answerNo.SetActive(false);
                    dripReader.SetActive(false);
                    thermoReader.SetActive(false);
                    ampSelect.SetActive(true);
                    dripSelect.SetActive(true);
                    thermoSelect.SetActive(true);

                    valueWithinRangeQ.SetActive(false);

                    if (ampButtonDisable)
                    {

                        ampSelect.SetActive(false);

                    }
                    if (dripButtonDisable)
                    {

                        dripSelect.SetActive(false);

                    }
                    if (thermoButtonDisable)
                    {

                        thermoSelect.SetActive(false);

                    }

                }
                else
                {

                    debugText.text = "correct";
                    audioSource.clip = partWorking;
                    audioSource.Play();
                    seal.GetComponent<MeshRenderer>().material = sealMat;

                    //PlayCorrectSelectionSequence(pumpAnimator, audioSource);

                    answerYes.SetActive(false);
                    answerNo.SetActive(false);
                    AmpReader.SetActive(false);
                    dripReader.SetActive(false);
                    thermoReader.SetActive(false);

                    ampSelect.SetActive(true);
                    dripSelect.SetActive(true);
                    thermoSelect.SetActive(false);

                    valueWithinRangeQ.SetActive(false);

                    thermCompleted = true;
                    thermoButtonDisable = true;
                    

                }
                break;

            case Scenario.drip:

                if (dripMeterValue > 30)
                {
                    //
                    debugText.text = "incorrect";
                    audioSource.clip = incorrectAudio;
                    audioSource.Play();

                    bearing.GetComponent<MeshRenderer>().material = bearingMat;

                    answerYes.SetActive(false);
                    answerNo.SetActive(false);
                    dripReader.SetActive(false);
                    thermoReader.SetActive(false);
                    ampSelect.SetActive(true);
                    dripSelect.SetActive(true);
                    thermoSelect.SetActive(true);

                    valueWithinRangeQ.SetActive(false);

                    if (ampButtonDisable)
                    {

                        ampSelect.SetActive(false);

                    }
                    if (dripButtonDisable)
                    {

                        dripSelect.SetActive(false);

                    }
                    if (thermoButtonDisable)
                    {

                        thermoSelect.SetActive(false);

                    }

                }
                else
                {

                    debugText.text = "correct";
                    audioSource.clip = partWorking;
                    audioSource.Play();
                    bearing.GetComponent<MeshRenderer>().material = bearingMat;

                    answerYes.SetActive(false);
                    answerNo.SetActive(false);
                    AmpReader.SetActive(false);
                    dripReader.SetActive(false);
                    thermoReader.SetActive(false);

                    ampSelect.SetActive(true);
                    dripSelect.SetActive(false);
                    thermoSelect.SetActive(true);

                    valueWithinRangeQ.SetActive(false);
                    //PlayCorrectSelectionSequence(pumpAnimator, audioSource);

                    dripCompleted = true;
                    dripButtonDisable = true;

                }
                break;
        }
 
    
    }

    public void CheckForCorrectAnswerNo() 
    {

        ampMeterValue = float.Parse(ampValue.text);
        thermMeterValue = float.Parse(thermoValue.text);
        dripMeterValue = float.Parse(dripValue.text);

        //checking to see if the meter values are within acceptable ranges. The user answered NO in this situation, all possible cases below.
        switch (meterSelection)
        {
            case Scenario.amp:
                
                if (ampMeterValue > 11.5f)
                {
                    audioSource.clip = replacePart;
                    audioSource.Play();
                    PlayCorrectSelectionSequence(pumpAnimator, audioSource);
                    
                    debugText.text = "correct";
                    motor.GetComponent<MeshRenderer>().material = motorMat;

                    answerYes.SetActive(false);
                    answerNo.SetActive(false);
                    AmpReader.SetActive(false);
                    dripReader.SetActive(false);
                    thermoReader.SetActive(false);

                    ampSelect.SetActive(false);
                    dripSelect.SetActive(true);
                    thermoSelect.SetActive(true);

                    valueWithinRangeQ.SetActive(false);

                    break;
                }
                else
                {
                    audioSource.clip = incorrectAudio;
                    audioSource.Play();

                    motor.GetComponent<MeshRenderer>().material = motorMat;

                    answerYes.SetActive(false);
                    answerNo.SetActive(false);
                    dripReader.SetActive(false);
                    thermoReader.SetActive(false);
                    ampSelect.SetActive(true);
                    dripSelect.SetActive(true);
                    thermoSelect.SetActive(true);

                    valueWithinRangeQ.SetActive(false);

                    //disables selection buttons if they have already been completed
                    if (ampButtonDisable)
                    {

                        ampSelect.SetActive(false);

                    }
                    if (dripButtonDisable)
                    {

                        dripSelect.SetActive(false);

                    }
                    if (thermoButtonDisable)
                    {

                        thermoSelect.SetActive(false);

                    }
                    debugText.text = "incorrect";
                    break;

                }
            case Scenario.therm:

                if (thermMeterValue > 238f)
                {
                    audioSource.clip = replacePart;
                    audioSource.Play();
                    PlayCorrectSelectionSequence(pumpAnimator, audioSource);
                    debugText.text = "correct";

                    seal.GetComponent<MeshRenderer>().material = sealMat;

                    answerYes.SetActive(false);
                    answerNo.SetActive(false);
                    AmpReader.SetActive(false);
                    dripReader.SetActive(false);
                    thermoReader.SetActive(false);

                    ampSelect.SetActive(true);
                    dripSelect.SetActive(true);
                    thermoSelect.SetActive(false);

                    valueWithinRangeQ.SetActive(false);

                }
                else
                {

                    seal.GetComponent<MeshRenderer>().material = sealMat;
                    debugText.text = "incorrect";
                    audioSource.clip = incorrectAudio;
                    audioSource.Play();
                    answerYes.SetActive(false);
                    answerNo.SetActive(false);
                    dripReader.SetActive(false);
                    thermoReader.SetActive(false);
                    ampSelect.SetActive(true);
                    dripSelect.SetActive(true);
                    thermoSelect.SetActive(true);

                    valueWithinRangeQ.SetActive(false);

                    //disables selection buttons if they have already been completed
                    if (ampButtonDisable)
                    {

                        ampSelect.SetActive(false);

                    }
                    if (dripButtonDisable)
                    {

                        dripSelect.SetActive(false);

                    }
                    if (thermoButtonDisable)
                    {

                        thermoSelect.SetActive(false);

                    }
                }
                break;

            case Scenario.drip:

                if (dripMeterValue > 30)
                {
                    audioSource.clip = replacePart;
                    audioSource.Play();
                    PlayCorrectSelectionSequence(pumpAnimator, audioSource);
                    debugText.text = "correct";

                    bearing.GetComponent<MeshRenderer>().material = bearingMat;

                    answerYes.SetActive(false);
                    answerNo.SetActive(false);
                    AmpReader.SetActive(false);
                    dripReader.SetActive(false);
                    thermoReader.SetActive(false);

                    ampSelect.SetActive(true);
                    dripSelect.SetActive(false);
                    thermoSelect.SetActive(true);

                    valueWithinRangeQ.SetActive(false);

                }
                else
                {
                    bearing.GetComponent<MeshRenderer>().material = bearingMat;
                    debugText.text = "incorrect";
                    audioSource.clip = incorrectAudio;
                    audioSource.Play();

                    answerYes.SetActive(false);
                    answerNo.SetActive(false);
                    dripReader.SetActive(false);
                    thermoReader.SetActive(false);
                    ampSelect.SetActive(true);
                    dripSelect.SetActive(true);
                    thermoSelect.SetActive(true);

                    valueWithinRangeQ.SetActive(false);

                    //disables selection buttons if they have already been completed
                    if (ampButtonDisable)
                    {

                        ampSelect.SetActive(false);

                    }
                    if (dripButtonDisable)
                    {

                        dripSelect.SetActive(false);

                    }
                    if (thermoButtonDisable)
                    {

                        thermoSelect.SetActive(false);

                    }


                }
                break;


        }

    }

    public void PlayCorrectSelectionSequence(Animator scenarioAnim, AudioSource audioSource) 
    {

        //AudioClip correct

        switch (meterSelection) 
        {
            case Scenario.amp:
                debugText.text = "inside playcorrectsequence";
                
                scenarioAnim.SetBool("ReplaceMotor", true);
                try 
                {

                    StartCoroutine(AnimationResetDelay(scenarioAnim, "MotorIsReplaced", "ReplaceMotor"));
                }
                catch 
                {

                    debugText2.text = "AnimationResetDelay() not working ";
                }
                

                ampCompleted = true;
                ampButtonDisable = true;
                break;

            case Scenario.drip:
                debugText.text = "inside playcorrectsequence";


                scenarioAnim.SetBool("ReplaceSeal", true);
                try
                {

                    StartCoroutine(AnimationResetDelay(scenarioAnim, "SealIsReplaced", "ReplaceSeal"));

                }
                catch
                {

                    debugText2.text = "AnimationResetDelay() not working ";
                }


                dripCompleted = true;
                dripButtonDisable = true;
                break;

            case Scenario.therm:
                debugText.text = "inside playcorrectsequence";
                
                scenarioAnim.SetBool("ReplaceBearing", true);

                try
                {

                    StartCoroutine(AnimationResetDelay(scenarioAnim, "BearingIsReplaced", "ReplaceBearing"));

                }
                catch
                {

                    debugText2.text = "AnimationResetDelay() not working ";
                }
                

                thermCompleted = true;
                thermoButtonDisable = true;
                break;

        }
        
        //reset scenarios, if successfully completed removes scenario button. 
        if (ampCompleted) 
        {
            //StartCoroutine(AnimationResetDelay(scenarioAnim, "MotorIsReplaced", "ReplaceMotor"));

            motor.GetComponent<MeshRenderer>().material = motorMat;

            answerYes.SetActive(false);
            answerNo.SetActive(false);
            AmpReader.SetActive(false);
            dripReader.SetActive(false);
            thermoReader.SetActive(false);

            ampSelect.SetActive(false);
            dripSelect.SetActive(true);
            thermoSelect.SetActive(true);

            valueWithinRangeQ.SetActive(false);

            ampCompleted = false;

            

            if (ampButtonDisable)
            {

                ampSelect.SetActive(false);

            }
            if (dripButtonDisable)
            {

                dripSelect.SetActive(false);

            }
            if (thermoButtonDisable)
            {

                thermoSelect.SetActive(false);

            }
        }
        else if (dripCompleted) 
        {

            //StartCoroutine(AnimationResetDelay(scenarioAnim, "SealIsReplaced", "ReplaceSeal"));
    

            seal.GetComponent<MeshRenderer>().material = sealMat;

            answerYes.SetActive(false);
            answerNo.SetActive(false);
            AmpReader.SetActive(false);
            dripReader.SetActive(false);
            thermoReader.SetActive(false);

            ampSelect.SetActive(true);
            dripSelect.SetActive(false);
            thermoSelect.SetActive(true);

            valueWithinRangeQ.SetActive(false);

            dripCompleted = false;

            if (ampButtonDisable)
            {

                ampSelect.SetActive(false);

            }
            if (dripButtonDisable)
            {

                dripSelect.SetActive(false);

            }
            if (thermoButtonDisable)
            {

                thermoSelect.SetActive(false);

            }

        }
        else if (thermCompleted) 
        {


            //StartCoroutine(AnimationResetDelay(scenarioAnim, "BearingIsReplaced", "ReplaceBearing"));
            



            bearing.GetComponent<MeshRenderer>().material = bearingMat;

            answerYes.SetActive(false);
            answerNo.SetActive(false);
            AmpReader.SetActive(false);
            dripReader.SetActive(false);
            thermoReader.SetActive(false);

            ampSelect.SetActive(true);
            dripSelect.SetActive(true);
            thermoSelect.SetActive(false);

            valueWithinRangeQ.SetActive(false);

            thermCompleted = false;

            if (ampButtonDisable)
            {

                ampSelect.SetActive(false);

            }
            if (dripButtonDisable)
            {

                dripSelect.SetActive(false);

            }
            if (thermoButtonDisable)
            {

                thermoSelect.SetActive(false);

            }

        }


        /*//disables selection buttons if they have already been completed
        if (ampButtonDisable) 
        {

            ampSelect.SetActive(false);

        }
        if (dripButtonDisable) 
        {

            dripSelect.SetActive(false);

        }
        if (thermoButtonDisable) 
        {

            thermoSelect.SetActive(false);
        
        }*/




        //ampReplace.Play();


    
    }

    public IEnumerator AnimationResetDelay(Animator scenarioAnim, string trueParam, string falseParam) 
    {
        debugText2.text += "AnimationResetDelay()";
        yield return new WaitForSeconds(17f);

        scenarioAnim.SetBool(falseParam, false);
        scenarioAnim.SetBool(trueParam, true);
        
    }


}
