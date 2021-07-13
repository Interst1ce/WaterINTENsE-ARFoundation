using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MaintScenarioStory : MonoBehaviour
{
    /*
    //This script handles the main functionality for the Maintenance Scenarios module

    //references to get at the helper methods of the AudioManager and Utility scripts
    public AudioManager audioManager;
    public Utility utility;
    //boolean to track whether main functionality of module has started executing
    public bool storyHasStarted = false;
    //needed for Vuforia to work
    private TrackableBehaviour mTrackableBehaviour;
    //booleans to track whether user has selected a button for one of the instruments
    private bool AmpButtonPressed = false;
    private bool InfraredButtonPressed = false;
    private bool DripButtonPressed = false;
    //number that the module's big switch statement runs off of; random scenarios are accessed
    //by randomizing this number, and thus the case run by the switch statement
    public int scenarioIndex;
    public System.Random rnd = new System.Random();
    //boolean to track whether a particular scenario has been completed
    private bool scenarioFinished = false;
    //numbers used to populate the text field of the UI element associated with each instrument 
    private double ampValue;
    private double infraValue;
    private double dripValue;
    //boolean to track whether a scenario has begun
    private bool scenePicked = false;
    //boolean to track whether the text fields of the instrument UI elements have been populated
    private bool valuesPopulated = false;

    //references to text fields of each instrument's UI element
    public Text DripText;
    public Text InfraText;
    public Text AmpText;
    //booleans to track whether user has selected one of the red capsule labels
    //these booleans are set in the 'Utility' script, in the raycasting function
    public bool HitMotorCapsule = false;
    public bool HitBearingCapsule = false;
    public bool HitSealCapsule = false;
    //booleans to track whether user has selected the correct red capsule label for a particular scenario
    //these booleans are set in the 'Utility' script, in the raycasting function
    public bool MotorIsCorrect = false;
    public bool BearingIsCorrect = false;
    public bool SealIsCorrect = false;
    //variables used in the lerp functions
    private bool isLerping = true;
    private float timeStartedLerping;
    private bool notStartedLerpingYet = true;
    public float timeTakenDuringLerp = 1f;
    private float timeSinceStarted;
    private float percentageComplete;
    //the start/end positions for the lerp functions that animate the replacement of each part
    private Vector3 motorStartPosition;
    private Vector3 motorEndPosition;
    private Vector3 bearingStartPosition;
    private Vector3 bearingEndPosition;
    private Vector3 sealStartPosition;
    private Vector3 sealEndPosition;
    //public Animation MotorAnimation;
    //public Animation BearingAnimation;
    //public Animation SealAnimation;
    //public Animation ReplacementAnimations;
    public Animator ReplacementAnimator;

    //reference to the pause menu
    public GameObject pauseMenu;
    // Use this for initialization
    //reference to each part of the motor, the replacement part (initially invisible), the button for each instrument, and the red capsule label for each instrument
    public GameObject Motor;
    public GameObject ReplacementMotor;
    public GameObject MotorIndicator;
    public GameObject MotorLabel;
    public GameObject MotorLabelCapsule;
    public GameObject Bearing;
    public GameObject ReplacementBearing;
    public GameObject Seal;
    public GameObject ReplacementSeal;
    public GameObject RedoButton;
    //references to materials used in fade functions for each part
    public Material MotorFadeMaterial;
    public Material BearingFadeMaterial;
    public Material SealFadeMaterial;

    void Start()
    {
        //Set up the event handler for tracking from Vuforia
        mTrackableBehaviour = GameObject.Find("ImageTarget").GetComponent<TrackableBehaviour>();

        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        //initializing the variables for the start/end position of the lerp functions and the materials for the fade functions
        //This way you can easily change the start/end points of the lerp function by changing the position of these objects in the inspector
        Motor.SetActive(true);
        motorStartPosition = ReplacementMotor.gameObject.transform.position;
        motorEndPosition = Motor.gameObject.transform.position;
        bearingStartPosition = ReplacementBearing.gameObject.transform.position;
        bearingEndPosition = Bearing.gameObject.transform.position;
        sealStartPosition = ReplacementSeal.gameObject.transform.position;
        sealEndPosition = Seal.gameObject.transform.position;
        MotorFadeMaterial = GetComponent<Renderer>().material;
        BearingFadeMaterial = GetComponent<Renderer>().material;
        SealFadeMaterial = GetComponent<Renderer>().material;

        //RedoButton.SetActive(false);
        Debug.Log("Turned off redobutton?");

    }
    //used to get random numbers for text fields in instrument UI elements
    public double GetDoubleInRange(double minValue, double maxValue)
    {
        double result = rnd.NextDouble() * (maxValue - minValue) + minValue;
        return result;
    }
    //main coroutine handling the functioning of the module; other coroutines for specific bits of functionality are called from here
    private IEnumerator MaintenanceScenariosNarrative()
    {

        //play intro narration
        audioManager.PlaySound("MaintScenariosIntro", 4.0f, true, 2);
        yield return null;

        //the module's main loop
        while (true)
        {
            //once intro narration is done, play one of the scenarios
            if (audioManager.GetSound("MaintScenariosIntro").hasCompleted && scenarioFinished == false)
            {
                //if scenario isn't selected yet, assign a random number to 'scenarioIndex' so that the
                //switch statement will bring up a random scenario
                if (scenePicked == false)
                {
                    scenarioIndex = rnd.Next(1, 14);
                    //scenarioIndex = 1;
                    scenePicked = true;
                    Debug.Log("Picked your scene!");
                }

                //the value of scenarioIndex determines which scenario gets executed                               
                switch (scenarioIndex)
                {
                    //The scenarios mix together three different types of readings: one that is only slightly off, one that is very off, one that is in the correct range
                    //Bad amp range is above 10, Bad Drip range is above 3, excessive is above 30, Bad infra range is above 239 fahrenheit, above average Infra range is above 220F
                    case 1:
                        //Amp is very wrong, Drip is very wrong, Infra is very wrong
                        //RedoButton.SetActive(false);

                        //In each case, these booleans track which selection the user is supposed to make.
                        //In this particular case, the amperage meter, drip counter and infrared thermometer are wrong,
                        //so the 'correct' selections would be the motor (amp), bearing(infra), and seal(drip)
                        MotorIsCorrect = true;
                        BearingIsCorrect = true;
                        SealIsCorrect = true;
                        Debug.Log("Made it to case 1!");
                        ReplacementAnimator.SetBool("MotorBool", false);
                        //this 'if' check in each case ensures values don't repopulate every time the while loop and the switch statement are run
                        if (valuesPopulated == false)
                        {
                            //get random values to populate instrument UI with
                            ampValue = GetDoubleInRange(15, 21);
                            dripValue = GetDoubleInRange(35, 40);
                            infraValue = GetDoubleInRange(270, 280);
                            //populate text field of instrument UI
                            DripText.text += "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            InfraText.text += "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            AmpText.text += "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            valuesPopulated = true;
                        }

                        //the same pattern holds for each case: when the user selects the right red capsule labels for the right parts...
                        if (HitMotorCapsule && HitBearingCapsule && HitSealCapsule)
                        {
                            //...begin the replacement animations...

                            StartCoroutine("ReplaceMotor");
                            StartCoroutine("ReplaceBearing");
                            StartCoroutine("ReplaceSeal");
                            //...then populate the text field of the instrument UI with a number indicating a 'normal' reading
                            ampValue = GetDoubleInRange(7, 9);
                            dripValue = GetDoubleInRange(2, 4);
                            infraValue = GetDoubleInRange(190, 225);
                            DripText.text = "Baseline: 3-4dpm" + "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            InfraText.text = "Baseline: 190F" + "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            AmpText.text = "Baseline: 10A" + "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("You finished the scenario!");


                            yield return new WaitForSeconds(5);

                            //Once the scenario is done, set the scenarioFinished boolean to true
                            //so the code inside the relevant 'if' blocks stop running, and bring up the pause menu and the UI for reloading a new scenario
                            pauseMenu.SetActive(true);
                            RedoButton.SetActive(true);
                            scenarioFinished = true;
                        }

                        break;

                    case 2:
                        //Amp is right, Drip is very wrong, Infra is very wrong
                        MotorIsCorrect = false;
                        BearingIsCorrect = true;
                        SealIsCorrect = true;
                        if (valuesPopulated == false)
                        {
                            ampValue = GetDoubleInRange(1, 9);
                            dripValue = GetDoubleInRange(35, 40);
                            infraValue = GetDoubleInRange(270, 280);
                            DripText.text += "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            InfraText.text += "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            AmpText.text += "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("Made it to case 2!");
                            valuesPopulated = true;
                        }

                        if (!HitMotorCapsule && HitBearingCapsule && HitSealCapsule)
                        {
                            StartCoroutine("ReplaceSeal");
                            StartCoroutine("ReplaceBearing");
                            dripValue = GetDoubleInRange(2, 4);
                            infraValue = GetDoubleInRange(190, 225);
                            DripText.text = "Baseline: 3-4dpm" + "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            InfraText.text = "Baseline: 190F" + "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            Debug.Log("You finished the scenario!");
                            yield return new WaitForSeconds(5);
                            RedoButton.SetActive(true);
                            pauseMenu.SetActive(true);
                            scenarioFinished = true;
                        }

                        //in each case, if the user does not select the right red capsule label, play the error sound (the '.playing' boolean is used
                        //to ensure that the sound file isn't played 1000x times as the while loop keeps running)
                        if (HitMotorCapsule && audioManager.GetSound("ErrorMessage").playing == false)
                        {

                            audioManager.PlaySound("ErrorMessage", 4.0f, true, 2);
                            HitMotorCapsule = false;
                            HitSealCapsule = false;
                            HitBearingCapsule = false;

                        }


                        break;

                    case 3:
                        //Amp is right, Drip is right, Infra is very wrong
                        MotorIsCorrect = false;
                        BearingIsCorrect = true;
                        SealIsCorrect = false;
                        if (valuesPopulated == false)
                        {
                            ampValue = GetDoubleInRange(1, 9);
                            dripValue = GetDoubleInRange(2, 4);
                            infraValue = GetDoubleInRange(270, 280);
                            DripText.text += "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            InfraText.text += "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            AmpText.text += "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("Made it to case 3!");
                            valuesPopulated = true;
                        }

                        if (!HitMotorCapsule && HitBearingCapsule && !HitSealCapsule)
                        {
                            StartCoroutine("ReplaceBearing");
                            infraValue = GetDoubleInRange(190, 225);
                            InfraText.text = "Baseline: 190F" + "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            Debug.Log("You finished the scenario!");
                            yield return new WaitForSeconds(5);
                            RedoButton.SetActive(true);
                            pauseMenu.SetActive(true);
                            scenarioFinished = true;
                        }

                        if (HitMotorCapsule == true || HitSealCapsule == true)
                        {
                            if (audioManager.GetSound("ErrorMessage").playing == false)
                            {

                                audioManager.PlaySound("ErrorMessage", 4.0f, true, 2);
                                HitMotorCapsule = false;
                                HitSealCapsule = false;
                                HitBearingCapsule = false;
                            }


                        }
                        break;

                    case 4:
                        //Amp is right, Drip is very wrong, Infra is right
                        MotorIsCorrect = false;
                        BearingIsCorrect = false;
                        SealIsCorrect = true;
                        if (valuesPopulated == false)
                        {
                            ampValue = GetDoubleInRange(1, 9);
                            dripValue = GetDoubleInRange(35, 40);
                            infraValue = GetDoubleInRange(190, 225);
                            DripText.text += "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            InfraText.text += "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            AmpText.text += "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("Made it to case 4!");
                            valuesPopulated = true;
                        }


                        if (!HitMotorCapsule && !HitBearingCapsule && HitSealCapsule)
                        {
                            StartCoroutine("ReplaceSeal");
                            dripValue = GetDoubleInRange(2, 4);
                            DripText.text = "Baseline: 3-4dpm" + "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            Debug.Log("You finished the scenario!");
                            yield return new WaitForSeconds(5);
                            RedoButton.SetActive(true);
                            pauseMenu.SetActive(true);
                            scenarioFinished = true;
                        }

                        if (HitMotorCapsule == true || HitBearingCapsule == true)
                        {
                            if (audioManager.GetSound("ErrorMessage").playing == false)
                            {

                                audioManager.PlaySound("ErrorMessage", 4.0f, true, 2);
                                HitMotorCapsule = false;
                                HitSealCapsule = false;
                                HitBearingCapsule = false;
                            }

                        }
                        break;

                    case 5:
                        //Amp is very wrong, Drip is very wrong, Infra is right
                        MotorIsCorrect = true;
                        BearingIsCorrect = false;
                        SealIsCorrect = true;
                        if (valuesPopulated == false)
                        {
                            ampValue = GetDoubleInRange(20, 21);
                            dripValue = GetDoubleInRange(35, 40);
                            infraValue = GetDoubleInRange(190, 225);
                            DripText.text += "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            InfraText.text += "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            AmpText.text += "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("Made it to case 5!");
                            valuesPopulated = true;
                        }

                        if (HitMotorCapsule && !HitBearingCapsule && HitSealCapsule)
                        {
                            StartCoroutine("ReplaceMotor");
                            StartCoroutine("ReplaceSeal");
                            ampValue = GetDoubleInRange(7, 9);
                            dripValue = GetDoubleInRange(2, 4);
                            DripText.text += "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            AmpText.text += "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("You finished the scenario!");
                            yield return new WaitForSeconds(5);
                            RedoButton.SetActive(true);
                            pauseMenu.SetActive(true);
                            scenarioFinished = true;
                        }

                        if (HitBearingCapsule)
                        {
                            if (audioManager.GetSound("ErrorMessage").playing == false)
                            {

                                audioManager.PlaySound("ErrorMessage", 4.0f, true, 2);
                                HitMotorCapsule = false;
                                HitSealCapsule = false;
                                HitBearingCapsule = false;
                            }

                        }
                        break;

                    case 6:
                        //Amp is very wrong, Drip is right, Infra is very wrong
                        MotorIsCorrect = true;
                        BearingIsCorrect = true;
                        SealIsCorrect = false;
                        if (valuesPopulated == false)
                        {
                            ampValue = GetDoubleInRange(18, 21);
                            dripValue = GetDoubleInRange(1, 4);
                            infraValue = GetDoubleInRange(270, 280);
                            DripText.text += "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            InfraText.text += "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            AmpText.text += "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("Made it to case 6!");
                            valuesPopulated = true;
                        }

                        if (HitMotorCapsule && HitBearingCapsule && !HitSealCapsule)
                        {
                            StartCoroutine("ReplaceMotor");
                            StartCoroutine("ReplaceBearing");
                            ampValue = GetDoubleInRange(7, 9);
                            infraValue = GetDoubleInRange(190, 225);
                            InfraText.text = "Baseline: 190F" + "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            AmpText.text = "Baseline: 10A" + "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("You finished the scenario!");
                            yield return new WaitForSeconds(5);
                            RedoButton.SetActive(true);
                            pauseMenu.SetActive(true);
                            scenarioFinished = true;
                        }
                        if (HitSealCapsule)
                        {
                            if (audioManager.GetSound("ErrorMessage").playing == false)
                            {


                                audioManager.PlaySound("ErrorMessage", 4.0f, true, 2);
                                HitMotorCapsule = false;
                                HitSealCapsule = false;
                                HitBearingCapsule = false;

                            }
                        }
                        break;

                    case 7:
                        //Amp is very wrong, Drip is right, Infra is right
                        MotorIsCorrect = true;
                        BearingIsCorrect = false;
                        SealIsCorrect = false;
                        if (valuesPopulated == false)
                        {
                            ampValue = GetDoubleInRange(18, 21);
                            dripValue = GetDoubleInRange(2, 4);
                            infraValue = GetDoubleInRange(220, 230);
                            DripText.text += "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            InfraText.text += "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            AmpText.text += "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("Made it to case 7!");
                            valuesPopulated = true;
                        }
                        if (HitMotorCapsule)
                        {
                            if (audioManager.GetSound("CorrectAnswer").playing == false)
                            {
                                audioManager.PlaySound("CorrectAnswer", 4.0f, true, 2);
                            }
                            if (audioManager.GetSound("CorrectAnswer").hasCompleted)
                            {
                                audioManager.StopSound("CorrectAnswer", true, 2);
                            }
                        }
                        if (HitMotorCapsule && !HitBearingCapsule && !HitSealCapsule)
                        {
                            StartCoroutine("ReplaceMotor");
                            ampValue = GetDoubleInRange(7, 9);
                            AmpText.text = "Baseline: 10A" + "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("You finished the scenario!");
                            yield return new WaitForSeconds(5);
                            RedoButton.SetActive(true);
                            pauseMenu.SetActive(true);
                            scenarioFinished = true;
                        }
                        if (HitBearingCapsule == true || HitSealCapsule == true)
                        {
                            if (audioManager.GetSound("ErrorMessage").playing == false)
                            {

                                audioManager.PlaySound("ErrorMessage", 4.0f, true, 2);
                                HitMotorCapsule = false;
                                HitSealCapsule = false;
                                HitBearingCapsule = false;

                            }
                        }
                        break;

                    case 8:
                        //Amp is slightly wrong, Drip is slightly wrong, Infra is slightly wrong
                        RedoButton.SetActive(false);
                        MotorIsCorrect = true;
                        BearingIsCorrect = true;
                        SealIsCorrect = true;
                        Debug.Log("Made it to case 8!");
                        ReplacementAnimator.SetBool("MotorBool", false);
                        if (valuesPopulated == false)
                        {
                            ampValue = GetDoubleInRange(11.5, 12);
                            dripValue = GetDoubleInRange(5, 9);
                            infraValue = GetDoubleInRange(235, 239);
                            DripText.text += "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            InfraText.text += "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            AmpText.text += "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            valuesPopulated = true;
                        }


                        if (HitMotorCapsule && HitBearingCapsule && HitSealCapsule)
                        {
                            if (audioManager.GetSound("MonitorReadings").playing == false)
                            {
                                audioManager.PlaySound("MonitorReadings", 4.0f, true, 2);
                            }
                            if (audioManager.GetSound("MonitorReadings").hasCompleted)
                            {
                                audioManager.StopSound("MonitorReadings", true, 2);
                            }

                            StartCoroutine("ReplaceMotor");
                            StartCoroutine("ReplaceBearing");
                            StartCoroutine("ReplaceSeal");
                            ampValue = GetDoubleInRange(7, 9);
                            dripValue = GetDoubleInRange(2, 4);
                            infraValue = GetDoubleInRange(190, 225);
                            DripText.text = "Baseline: 3-4dpm" + "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            InfraText.text = "Baseline: 190F" + "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            AmpText.text = "Baseline: 10A" + "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("You finished the scenario!");
                            yield return new WaitForSeconds(5);
                            RedoButton.SetActive(true);
                            pauseMenu.SetActive(true);
                            scenarioFinished = true;
                        }

                        break;

                    case 9:
                        //Amp is right, Drip is slightly wrong, Infra is slightly wrong
                        MotorIsCorrect = false;
                        BearingIsCorrect = true;
                        SealIsCorrect = true;
                        if (valuesPopulated == false)
                        {
                            ampValue = GetDoubleInRange(1, 9);
                            dripValue = GetDoubleInRange(5, 6);
                            infraValue = GetDoubleInRange(235, 239);
                            DripText.text += "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            InfraText.text += "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            AmpText.text += "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("Made it to case 9!");
                            valuesPopulated = true;
                        }

                        if (!HitMotorCapsule && HitBearingCapsule && HitSealCapsule)
                        {
                            if (audioManager.GetSound("MonitorReadings").playing == false)
                            {
                                audioManager.PlaySound("MonitorReadings", 4.0f, true, 2);
                            }
                            if (audioManager.GetSound("MonitorReadings").hasCompleted)
                            {
                                audioManager.StopSound("MonitorReadings", true, 2);
                            }
                            StartCoroutine("ReplaceSeal");
                            StartCoroutine("ReplaceBearing");
                            dripValue = GetDoubleInRange(2, 4);
                            infraValue = GetDoubleInRange(190, 225);
                            DripText.text = "Baseline: 3-4dpm" + "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            InfraText.text = "Baseline: 190F" + "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            Debug.Log("You finished the scenario!");
                            yield return new WaitForSeconds(5);
                            RedoButton.SetActive(true);
                            pauseMenu.SetActive(true);
                            scenarioFinished = true;
                        }

                        if (HitMotorCapsule && audioManager.GetSound("ErrorMessage").playing == false)
                        {

                            audioManager.PlaySound("ErrorMessage", 4.0f, true, 2);
                            HitMotorCapsule = false;
                            HitSealCapsule = false;
                            HitBearingCapsule = false;

                        }


                        break;

                    case 10:
                        //Amp is right, Drip is right, Infra is slightly wrong
                        MotorIsCorrect = false;
                        BearingIsCorrect = true;
                        SealIsCorrect = false;
                        if (valuesPopulated == false)
                        {
                            ampValue = GetDoubleInRange(1, 9);
                            dripValue = GetDoubleInRange(2, 4);
                            infraValue = GetDoubleInRange(235, 239);
                            DripText.text += "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            InfraText.text += "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            AmpText.text += "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("Made it to case 10!");
                            valuesPopulated = true;
                        }

                        if (!HitMotorCapsule && HitBearingCapsule && !HitSealCapsule)
                        {
                            if (audioManager.GetSound("MonitorReadings").playing == false)
                            {
                                audioManager.PlaySound("MonitorReadings", 4.0f, true, 2);
                            }
                            if (audioManager.GetSound("MonitorReadings").hasCompleted)
                            {
                                audioManager.StopSound("MonitorReadings", true, 2);
                            }
                            StartCoroutine("ReplaceBearing");
                            infraValue = GetDoubleInRange(190, 225);
                            InfraText.text = "Baseline: 190F" + "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            Debug.Log("You finished the scenario!");
                            yield return new WaitForSeconds(5);
                            RedoButton.SetActive(true);
                            pauseMenu.SetActive(true);
                            scenarioFinished = true;
                        }

                        if (HitMotorCapsule == true || HitSealCapsule == true)
                        {
                            if (audioManager.GetSound("ErrorMessage").playing == false)
                            {

                                audioManager.PlaySound("ErrorMessage", 4.0f, true, 2);
                                HitMotorCapsule = false;
                                HitSealCapsule = false;
                                HitBearingCapsule = false;
                            }


                        }
                        break;

                    case 11:
                        //Amp is right, Drip is slightly wrong, Infra is right
                        MotorIsCorrect = false;
                        BearingIsCorrect = false;
                        SealIsCorrect = true;
                        if (valuesPopulated == false)
                        {
                            ampValue = GetDoubleInRange(1, 9);
                            dripValue = GetDoubleInRange(5, 6);
                            infraValue = GetDoubleInRange(190, 225);
                            DripText.text += "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            InfraText.text += "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            AmpText.text += "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("Made it to case 11!");
                            valuesPopulated = true;
                        }


                        if (!HitMotorCapsule && !HitBearingCapsule && HitSealCapsule)
                        {
                            if (audioManager.GetSound("MonitorReadings").playing == false)
                            {
                                audioManager.PlaySound("MonitorReadings", 4.0f, true, 2);
                            }
                            if (audioManager.GetSound("MonitorReadings").hasCompleted)
                            {
                                audioManager.StopSound("MonitorReadings", true, 2);
                            }
                            StartCoroutine("ReplaceSeal");
                            dripValue = GetDoubleInRange(2, 4);
                            DripText.text = "Baseline: 3-4dpm" + "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            Debug.Log("You finished the scenario!");
                            yield return new WaitForSeconds(5);
                            RedoButton.SetActive(true);
                            pauseMenu.SetActive(true);
                            scenarioFinished = true;
                        }

                        if (HitMotorCapsule == true || HitBearingCapsule == true)
                        {
                            if (audioManager.GetSound("ErrorMessage").playing == false)
                            {

                                audioManager.PlaySound("ErrorMessage", 4.0f, true, 2);
                                HitMotorCapsule = false;
                                HitSealCapsule = false;
                                HitBearingCapsule = false;
                            }

                        }
                        break;

                    case 12:
                        //Amp is slightly wrong, Drip is slightly wrong, Infra is right
                        MotorIsCorrect = true;
                        BearingIsCorrect = false;
                        SealIsCorrect = true;
                        if (valuesPopulated == false)
                        {
                            ampValue = GetDoubleInRange(11.5, 12);
                            dripValue = GetDoubleInRange(5, 6);
                            infraValue = GetDoubleInRange(190, 225);
                            DripText.text += "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            InfraText.text += "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            AmpText.text += "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("Made it to case 12!");
                            valuesPopulated = true;
                        }

                        if (HitMotorCapsule && !HitBearingCapsule && HitSealCapsule)
                        {
                            if (audioManager.GetSound("MonitorReadings").playing == false)
                            {
                                audioManager.PlaySound("MonitorReadings", 4.0f, true, 2);
                            }
                            if (audioManager.GetSound("MonitorReadings").hasCompleted)
                            {
                                audioManager.StopSound("MonitorReadings", true, 2);
                            }
                            StartCoroutine("ReplaceMotor");
                            StartCoroutine("ReplaceSeal");
                            ampValue = GetDoubleInRange(7, 9);
                            dripValue = GetDoubleInRange(2, 4);
                            DripText.text = "Baseline: 3-4dpm" + "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            AmpText.text = "Baseline: 10A" + "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("You finished the scenario!");
                            yield return new WaitForSeconds(5);
                            RedoButton.SetActive(true);
                            pauseMenu.SetActive(true);
                            scenarioFinished = true;
                        }

                        if (HitBearingCapsule)
                        {
                            if (audioManager.GetSound("ErrorMessage").playing == false)
                            {

                                audioManager.PlaySound("ErrorMessage", 4.0f, true, 2);
                                HitMotorCapsule = false;
                                HitSealCapsule = false;
                                HitBearingCapsule = false;
                            }

                        }
                        break;

                    case 13:
                        //Amp is slightly wrong, Drip is right, Infra is slightly wrong
                        MotorIsCorrect = true;
                        BearingIsCorrect = true;
                        SealIsCorrect = false;
                        if (valuesPopulated == false)
                        {
                            ampValue = GetDoubleInRange(11.5, 12);
                            dripValue = GetDoubleInRange(1, 4);
                            infraValue = GetDoubleInRange(235, 239);
                            DripText.text += "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            InfraText.text += "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            AmpText.text += "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("Made it to case 13!");
                            valuesPopulated = true;
                        }

                        if (HitMotorCapsule && HitBearingCapsule && !HitSealCapsule)
                        {
                            if (audioManager.GetSound("MonitorReadings").playing == false)
                            {
                                audioManager.PlaySound("MonitorReadings", 4.0f, true, 2);
                            }
                            if (audioManager.GetSound("MonitorReadings").hasCompleted)
                            {
                                audioManager.StopSound("MonitorReadings", true, 2);
                            }
                            StartCoroutine("ReplaceMotor");
                            StartCoroutine("ReplaceBearing");
                            ampValue = GetDoubleInRange(7, 9);
                            infraValue = GetDoubleInRange(190, 225);
                            InfraText.text = "Baseline: 190F" + "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            AmpText.text = "Baseline: 10A" + "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("You finished the scenario!");
                            yield return new WaitForSeconds(5);
                            RedoButton.SetActive(true);
                            pauseMenu.SetActive(true);
                            scenarioFinished = true;
                        }
                        if (HitSealCapsule)
                        {
                            if (audioManager.GetSound("ErrorMessage").playing == false)
                            {


                                audioManager.PlaySound("ErrorMessage", 4.0f, true, 2);
                                HitMotorCapsule = false;
                                HitSealCapsule = false;
                                HitBearingCapsule = false;

                            }
                        }
                        break;

                    case 14:
                        //Amp is slightly wrong, Drip is right, Infra is right
                        MotorIsCorrect = true;
                        BearingIsCorrect = false;
                        SealIsCorrect = false;
                        if (valuesPopulated == false)
                        {
                            ampValue = GetDoubleInRange(11.5, 12);
                            dripValue = GetDoubleInRange(2, 4);
                            infraValue = GetDoubleInRange(190, 215);
                            DripText.text += "\n" + System.Math.Round(dripValue).ToString() + " dpm";
                            InfraText.text += "\n" + System.Math.Round(infraValue, 2).ToString() + " F";
                            AmpText.text += "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("Made it to case 14!");
                            valuesPopulated = true;
                        }
                        if (HitMotorCapsule)
                        {
                            if (audioManager.GetSound("MonitorReadings").playing == false)
                            {
                                audioManager.PlaySound("MonitorReadings", 4.0f, true, 2);
                            }
                            if (audioManager.GetSound("MonitorReadings").hasCompleted)
                            {
                                audioManager.StopSound("MonitorReadings", true, 2);
                            }
                        }
                        if (HitMotorCapsule && !HitBearingCapsule && !HitSealCapsule)
                        {
                            StartCoroutine("ReplaceMotor");
                            ampValue = GetDoubleInRange(7, 9);
                            AmpText.text = "Baseline: 10A" + "\n" + System.Math.Round(ampValue, 2).ToString() + " A";
                            Debug.Log("You finished the scenario!");
                            yield return new WaitForSeconds(5);
                            RedoButton.SetActive(true);
                            pauseMenu.SetActive(true);
                            scenarioFinished = true;
                        }
                        if (HitBearingCapsule == true || HitSealCapsule == true)
                        {
                            if (audioManager.GetSound("ErrorMessage").playing == false)
                            {

                                audioManager.PlaySound("ErrorMessage", 4.0f, true, 2);
                                HitMotorCapsule = false;
                                HitSealCapsule = false;
                                HitBearingCapsule = false;

                            }
                        }
                        break;
                }

            }

            if (scenarioFinished)
            {
                Debug.Log("Made it inside scenarioFinished statement");
                RedoButton.SetActive(true);
                break;
            }

            yield return null;

        }

    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("HitMotorCapsule is: "+HitMotorCapsule);
        //Debug.Log("HitBearingCapsule is: " + HitBearingCapsule);
        //Debug.Log("HitSealCapsule is: " + HitSealCapsule);
    }

    //usual Vuforia method called when tracking state changes
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if ((newStatus == TrackableBehaviour.Status.TRACKED || newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) && previousStatus == TrackableBehaviour.Status.NO_POSE && storyHasStarted == false)
        {
            storyHasStarted = true;
            StartCoroutine(MaintenanceScenariosNarrative());
        }
    }

    //Herein follow the coroutines that execute the replacement animations and fade functions
    private IEnumerator ReplaceMotor()
    {
        //fade function
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            if (f <= 0.1f)
            {
                Motor.SetActive(false);
            }
            Color c = MotorFadeMaterial.color;
            c.a = f;
            MotorFadeMaterial.color = c;

            yield return null;
        }
        //once fade function's finished, this lerp function performs animation
        while (true)
        {
            if (notStartedLerpingYet)
            {
                ReplacementMotor.SetActive(true);
                isLerping = true;
                timeStartedLerping = Time.time;
                notStartedLerpingYet = false;
            }

            if (isLerping)
            {
                timeSinceStarted = Time.time - timeStartedLerping;
                percentageComplete = timeSinceStarted / timeTakenDuringLerp;
                ReplacementMotor.gameObject.transform.position = Vector3.Lerp(motorStartPosition, motorEndPosition, percentageComplete);
                if (percentageComplete >= 1.0f)
                {
                    isLerping = false;
                    notStartedLerpingYet = true;
                    ReplacementMotor.gameObject.transform.position = motorEndPosition;
                    Color c = MotorFadeMaterial.color;
                    c.a = 1f;
                    MotorFadeMaterial.color = c;
                    MotorIndicator.SetActive(true);
                    MotorLabel.SetActive(true);
                    MotorLabelCapsule.SetActive(true);
                    //pauseMenu.SetActive(true);
                    //Motor.SetActive(true);
                    break;
                }
            }

            yield return null;
        }



    }

    private IEnumerator ReplaceBearing()
    {
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            if (f <= 0.1f)
            {
                Bearing.SetActive(false);
            }
            Color c = BearingFadeMaterial.color;
            c.a = f;
            BearingFadeMaterial.color = c;

            yield return null;
        }
        while (true)
        {
            if (notStartedLerpingYet)
            {
                ReplacementBearing.SetActive(true);
                isLerping = true;
                timeStartedLerping = Time.time;
                notStartedLerpingYet = false;
            }

            if (isLerping)
            {
                timeSinceStarted = Time.time - timeStartedLerping;
                percentageComplete = timeSinceStarted / timeTakenDuringLerp;
                ReplacementBearing.gameObject.transform.position = Vector3.Lerp(bearingStartPosition, bearingEndPosition, percentageComplete);
                if (percentageComplete >= 1.0f)
                {
                    isLerping = false;
                    notStartedLerpingYet = true;
                    ReplacementBearing.gameObject.transform.position = bearingEndPosition;
                    Color c = BearingFadeMaterial.color;
                    c.a = 1f;
                    BearingFadeMaterial.color = c;
                    //pauseMenu.SetActive(true);
                    //BearingIndicator.SetActive(true);
                    //BearingLabel.SetActive(true);
                    //BearingLabelCapsule.SetActive(true);
                    //Motor.SetActive(true);
                    break;
                }
            }

            yield return null;
        }

    }

    private IEnumerator ReplaceSeal()
    {
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            if (f <= 0.1f)
            {
                Seal.SetActive(false);
            }
            Color c = SealFadeMaterial.color;
            c.a = f;
            SealFadeMaterial.color = c;

            yield return null;
        }
        while (true)
        {
            if (notStartedLerpingYet)
            {
                ReplacementSeal.SetActive(true);
                isLerping = true;
                timeStartedLerping = Time.time;
                notStartedLerpingYet = false;
            }

            if (isLerping)
            {
                timeSinceStarted = Time.time - timeStartedLerping;
                percentageComplete = timeSinceStarted / timeTakenDuringLerp;
                ReplacementSeal.gameObject.transform.position = Vector3.Lerp(sealStartPosition, sealEndPosition, percentageComplete);
                if (percentageComplete >= 1.0f)
                {
                    isLerping = false;
                    notStartedLerpingYet = true;
                    ReplacementSeal.gameObject.transform.position = sealEndPosition;
                    Color c = SealFadeMaterial.color;
                    c.a = 1f;
                    SealFadeMaterial.color = c;
                    //pauseMenu.SetActive(true);
                    //SealIndicator.SetActive(true);
                    //SealLabel.SetActive(true);
                    //SealLabelCapsule.SetActive(true);
                    //Motor.SetActive(true);
                    break;
                }
            }

            yield return null;
        }

    }

    //function to load new scenario--accomplished by simply re-loading scene
    public void RedoScenario()
    {
        SceneManager.LoadScene(11);
    }
}

// Start is called before the first frame update
void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
