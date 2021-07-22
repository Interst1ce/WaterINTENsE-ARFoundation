using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OperationsScenarios : MonoBehaviour {

    [SerializeField]
    Text debugText;
    [SerializeField]
    Text sliderName;
    [SerializeField]
    GameObject sliderPanel;
    [SerializeField]
    Text sliderValue;

    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioSource audioSource2;
    [SerializeField]
    AudioClip introClip;
    [SerializeField]
    AudioClip success;


    [SerializeField]
    Slider sliderValveTop;
    [SerializeField]
    Slider sliderValveBot;

    [SerializeField]
    Material glowMat;

    [SerializeField]
    Material valveTopOriginalMat;
    [SerializeField]
    Material valveBotOriginalMat;

    [SerializeField]
    GameObject valveTop;
    [SerializeField]
    GameObject valveBot;

    [SerializeField]
    GameObject restartButton;

    [SerializeField]
    PauseMenu pauseMenu;
    [SerializeField]
    ARObjectPlacement arPlacer;

    [SerializeField]
    int scenarioSelect;

    [SerializeField]
    AudioClip cavitationClip;
    [SerializeField]
    AudioClip recirculationClip;

    bool topCorrect = false;
    bool botCorrect = false;

    bool recircVolFinished = false;
    bool highDisVolFinished = false;
    bool lowsucVolFinished = false;

    bool restartSelected = false;


    RaycastHit operationsScenarioHit;
    // Start is called before the first frame update
    void Start() {
        //arPlacer.sceneSpawned = false;

        valveTopOriginalMat = valveTop.gameObject.GetComponent<MeshRenderer>().material;
        valveBotOriginalMat = valveBot.gameObject.GetComponent<MeshRenderer>().material;

        StartCoroutine(TouchInput());

    }

    // Update is called once per frame
    void Update() {
        //debugText.text = "volume: " + audioSource2.volume;
        sliderValue.text = sliderValveTop.value.ToString();
        sliderValue.text = sliderValveBot.value.ToString();

    }

    public IEnumerator TouchInput() {

        while (true) {

            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) {

                Ray ray;
                ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

                if (Physics.Raycast(ray,out operationsScenarioHit)) {


                    StartCoroutine(ShowValveSliders());
                    //debugText.text = "collider:" + operationsScenarioHit.collider.tag;


                }

            }

            yield return null;
        }

        yield return null;

    }

    public void OperationsScenariosManager() {


        restartButton.SetActive(false);
        audioSource.clip = introClip;
        audioSource.Play();

        //debugText.text = "scenario select is" + scenarioSelect;


        switch (scenarioSelect) {

            case 0:
                audioSource2.clip = cavitationClip;
                StartCoroutine(LowSuctionScenario());

                break;

            case 1:
                audioSource2.clip = cavitationClip;
                StartCoroutine(HighDischargeScenario());


                break;

            case 2:
                audioSource2.clip = recirculationClip;
                StartCoroutine(RecirculationScenario());
                break;

            default:
                debugText.text = "Not In Case";
                break;

        }

    }

    public IEnumerator LowSuctionScenario() {
        if (restartSelected == true) {
            valveBot.GetComponent<Collider>().enabled = true;
            restartSelected = false;
            sliderValveTop.value = 0f;
            sliderValveBot.value = 0f;
            valveBot.GetComponent<MeshRenderer>().material = valveBotOriginalMat;
            sliderValveBot.GetComponent<Slider>().interactable = true;
            sliderValveBot.gameObject.SetActive(false);
            sliderName.gameObject.SetActive(false);
            sliderPanel.gameObject.SetActive(false);


        }
        yield return new WaitForSeconds(23f);
        valveBot.GetComponent<MeshRenderer>().material = glowMat;


        audioSource2.Play();
        audioSource2.volume = 1f;

        StartCoroutine(LowSuctionVolumeCoroutine());

        while (true) {

            if (sliderValveBot.value > .69f && sliderValveBot.value < .8f) {
                lowsucVolFinished = true;
                botCorrect = true;
                audioSource.clip = success;
                audioSource.Play();
                topCorrect = false;
                botCorrect = false;
                audioSource2.volume = 0f;
                break;
            }


            yield return null;
        }

        sliderValveTop.GetComponent<Slider>().interactable = false;
        sliderValveBot.GetComponent<Slider>().interactable = false;

        while (true) {
            if (!audioSource.isPlaying) {

                pauseMenu.Pause();
                restartButton.SetActive(true);
                break;

            }

            yield return null;

        }

        yield return null;
    }

    public IEnumerator HighDischargeScenario() {

        //debugText.text = "HighDischarge";
        if (restartSelected == true) {
            valveTop.GetComponent<Collider>().enabled = true;
            restartSelected = false;
            sliderValveTop.value = 1f;
            sliderValveBot.value = 0f;
            valveTop.GetComponent<MeshRenderer>().material = valveTopOriginalMat;
            valveBot.GetComponent<MeshRenderer>().material = valveBotOriginalMat;
            sliderValveTop.GetComponent<Slider>().interactable = true;
            sliderValveTop.gameObject.SetActive(false);
            sliderName.gameObject.SetActive(false);
            sliderPanel.gameObject.SetActive(false);


        }
        sliderValveTop.value = 1f;
        yield return new WaitForSeconds(8f);
        valveTop.GetComponent<MeshRenderer>().material = glowMat;

        audioSource2.Play();
        audioSource2.volume = 1f;

        StartCoroutine(RecirculationVolumeCoroutine());




        while (true) {
            if (sliderValveTop.value > .39f && sliderValveTop.value < .49f) {
                highDisVolFinished = true;
                audioSource.clip = success;
                audioSource.Play();
                audioSource2.volume = 0f;
                break;

            }

            yield return null;
        }
        sliderValveTop.GetComponent<Slider>().interactable = false;
        sliderValveBot.GetComponent<Slider>().interactable = false;

        while (true) {
            if (!audioSource.isPlaying) {

                pauseMenu.Pause();
                restartButton.SetActive(true);
                break;

            }

            yield return null;
        }
    }

    public IEnumerator RecirculationScenario() {
        //debugText.text = "Recirculation";
        if (restartSelected == true) {
            valveTop.GetComponent<Collider>().enabled = true;
            restartSelected = false;
            sliderValveTop.value = 0f;
            sliderValveBot.value = 0f;
            valveTop.GetComponent<MeshRenderer>().material = valveTopOriginalMat;
            valveBot.GetComponent<MeshRenderer>().material = valveBotOriginalMat;
            sliderValveTop.GetComponent<Slider>().interactable = true;
            sliderValveBot.GetComponent<Slider>().interactable = true;
            sliderValveTop.gameObject.SetActive(false);
            sliderValveBot.gameObject.SetActive(false);
            sliderName.gameObject.SetActive(false);
            sliderPanel.gameObject.SetActive(false);

        }

        yield return new WaitForSeconds(14f);
        valveTop.GetComponent<MeshRenderer>().material = glowMat;



        audioSource2.Play();
        audioSource2.volume = 1f;

        StartCoroutine(HighDischargeVolume());

        while (true) {
            if (sliderValveTop.value > .49f && sliderValveTop.value < .6f) {
                recircVolFinished = true;
                audioSource.clip = success;
                audioSource.Play();
                audioSource2.volume = 0f;
                break;
            }


            yield return null;
        }

        sliderValveTop.GetComponent<Slider>().interactable = false;
        sliderValveBot.GetComponent<Slider>().interactable = false;

        while (true) {
            if (!audioSource.isPlaying) {

                pauseMenu.Pause();
                restartButton.SetActive(true);
                break;

            }

            yield return null;
        }

        yield return null;
    }

    public IEnumerator ShowValveSliders() {

        while (true) {
            if (audioSource.isPlaying) {
                //debugText.text = "should be turning off";
                valveBot.GetComponent<Collider>().enabled = false;
                valveTop.GetComponent<Collider>().enabled = false;

                //break;
            } else if (!audioSource.isPlaying) {
                if (scenarioSelect == 0) {
                    //low suction
                    valveTop.GetComponent<Collider>().enabled = false;
                    valveBot.GetComponent<Collider>().enabled = true;
                    break;
                } else if (scenarioSelect == 1) {
                    //high discharge
                    valveTop.GetComponent<Collider>().enabled = true;
                    valveBot.GetComponent<Collider>().enabled = false;
                    break;
                } else if (scenarioSelect == 2) {
                    //recirculation
                    valveTop.GetComponent<Collider>().enabled = true;
                    valveBot.GetComponent<Collider>().enabled = false;
                    break;
                }
                yield return new WaitForSeconds(1f);
                //debugText.text = "inside isplaying check";

                break;
            }
            yield return null;
        }

        while (true) {

            if (operationsScenarioHit.collider.tag == "topValve") {
                valveBot.GetComponent<MeshRenderer>().material = valveBotOriginalMat;
                valveTop.GetComponent<MeshRenderer>().material = glowMat;

                sliderPanel.gameObject.SetActive(true);
                sliderName.gameObject.SetActive(true);
                sliderValveTop.gameObject.SetActive(true);
                sliderValveBot.gameObject.SetActive(false);

                //sliderValue.text = sliderValveTop.value.ToString();


                sliderName.text = "Top Intake Valve";
                operationsScenarioHit.collider.tag = "null";
                break;

            } else if (operationsScenarioHit.collider.tag == "botValve") {
                valveTop.GetComponent<MeshRenderer>().material = valveTopOriginalMat;
                valveBot.GetComponent<MeshRenderer>().material = glowMat;

                sliderPanel.gameObject.SetActive(true);
                sliderName.gameObject.SetActive(true);
                sliderValveTop.gameObject.SetActive(false);
                sliderValveBot.gameObject.SetActive(true);

                //sliderValue.text = sliderValveBot.value.ToString();

                sliderName.text = "Side Intake Valve";
                operationsScenarioHit.collider.tag = "null";
                break;
            } else {
                /*sliderPanel.gameObject.SetActive(false);
                sliderValveTop.gameObject.SetActive(false);
                sliderValveBot.gameObject.SetActive(false);*/
                break;
            }

            yield return null;

        }

        yield return null;
    }

    public void RestartScene() {
        //yield return new WaitForSeconds(2f);
        /*audioSource.clip = introClip;
        audioSource.Play();*/

        //SceneManager.LoadScene("OperationsScenarios_LowSuc_PumpApp");
        restartSelected = true;
        pauseMenu.Pause();
        OperationsScenariosManager();



    }

    public IEnumerator LowSuctionVolumeCoroutine() {

        while (true) {
            if (lowsucVolFinished) {
                break;
            } else if (audioSource2.isActiveAndEnabled) {

                if (audioSource2.clip == cavitationClip && sliderValveBot.value >= .7f && audioSource2.isPlaying) {
                    //debugText.text += "Volume off";
                    audioSource2.volume = 0f;
                    break;
                } else if (sliderValveBot.value < .7f && sliderValveBot.value >= .3f && audioSource2.isPlaying) {
                    //debugText.text += "Volume on, volume: " + audioSource2.volume;
                    audioSource2.volume = .5f;

                } else {
                    //debugText.text += "in else, volume:" + audioSource2.volume;
                    audioSource2.volume = .25f;

                }
            }


            yield return null;
        }


    }

    public IEnumerator RecirculationVolumeCoroutine() {
        while (true) {
            if (recircVolFinished) {
                break;
            } else if (audioSource2.isActiveAndEnabled) {

                if (audioSource2.clip == recirculationClip && sliderValveTop.value >= .5f && audioSource2.isPlaying) {
                    //debugText.text += "Volume off";
                    audioSource2.volume = 0f;
                    break;
                } else if (sliderValveTop.value < .5f && sliderValveBot.value >= .2f && audioSource2.isPlaying) {
                    //debugText.text += "Volume on, volume: " + audioSource2.volume;
                    audioSource2.volume = .5f;

                } else {
                    //debugText.text += "in else, volume:" + audioSource2.volume;
                    audioSource2.volume = .25f;

                }
            }
            yield return null;
        }
    }

    public IEnumerator HighDischargeVolume() {

        while (true) {
            if (highDisVolFinished) {

                break;
            } else if (audioSource2.isActiveAndEnabled) {

                if (audioSource2.clip == cavitationClip && sliderValveBot.value >= .4f && audioSource2.isPlaying) {
                    //debugText.text += "Volume off";
                    audioSource2.volume = 0f;
                    break;
                } else if (sliderValveBot.value < .4f && sliderValveBot.value >= .2f && audioSource2.isPlaying) {
                    //debugText.text += "Volume on, volume: " + audioSource2.volume;
                    audioSource2.volume = .5f;

                } else {
                    //debugText.text += "in else, volume:" + audioSource2.volume;
                    audioSource2.volume = .25f;

                }
            }
            yield return null;
        }


    }
}
