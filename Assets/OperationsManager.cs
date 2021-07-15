using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperationsManager : MonoBehaviour
{
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
    AudioClip lowsucExploreAudio;
    [SerializeField]
    AudioClip highDisExploreAudio;
    [SerializeField]
    AudioClip recircExploreAudio;
    [SerializeField]
    AudioClip cavitationClip;
    [SerializeField]
    AudioClip recirculationClip;

    [SerializeField]
    Slider sliderValveTop;
    [SerializeField]
    Slider sliderValveBot;

    [SerializeField]
    Material glowMat;

    Material valveTopOriginalMat;
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

    int currentScenario;

    bool topCorrect = false;
    bool botCorrect = false;

    bool restartSelected = false;

    RaycastHit operationsHit;
    // Start is called before the first frame update
    void Start()
    {
        valveTopOriginalMat = valveTop.gameObject.GetComponent<MeshRenderer>().material;
        valveBotOriginalMat = valveBot.gameObject.GetComponent<MeshRenderer>().material;

        StartCoroutine(TouchInput());
        
    }

    // Update is called once per frame
    void Update()
    {

        sliderValue.text = sliderValveTop.value.ToString();
        sliderValue.text = sliderValveBot.value.ToString();


    }

    public IEnumerator TouchInput() 
    {

        while (true) 
        {

            if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) 
            {

                Ray ray;
                ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

                if (Physics.Raycast(ray, out operationsHit)) 
                {
                    
                        StartCoroutine(ShowValveSliders());
                    //debugText.text = "collider:" + operationsHit.collider.tag;
                    
                }
            
            }

            yield return null;
        }
        
        yield return null;
    
    }

    //call coroutines in cases
    public void OperationsExploreManager() 
    {
        restartButton.SetActive(false);
        valveBot.GetComponent<MeshRenderer>().material = valveBotOriginalMat;
        valveTop.GetComponent<MeshRenderer>().material = valveBotOriginalMat;


        //breaking the sliders
        /*while (true) 
        {

            if (audioSource.isPlaying == true)
            {
                sliderValveTop.gameObject.SetActive(false);
                sliderValveBot.gameObject.SetActive(false);
                //sliderValveTop.GetComponent<Slider>().interactable = false;
                //sliderValveBot.GetComponent<Slider>().interactable = false;

            }
            else
            {
                sliderValveTop.gameObject.SetActive(true);
                sliderValveBot.gameObject.SetActive(true);
                //sliderValveTop.GetComponent<Slider>().interactable = true;
                //sliderValveBot.GetComponent<Slider>().interactable = true;
                break;
            }*/


    


            System.Random random = new System.Random();
            currentScenario = random.Next(0, 2);

            switch (currentScenario) 
            {
                case 0:

                StartCoroutine(LowSuctionScenario());
                    
                break;

                case 1:

                StartCoroutine(HighDischargeScenario());


                break;

                case 2:

                StartCoroutine(RecirculationScenario());


                break;
                
                default:
                debugText.text = "Not In Case";
                break;

            }          
    }

    public IEnumerator LowSuctionScenario()
    {

        audioSource.clip = introClip;
        audioSource.Play();

        if (restartSelected == true)
        {
            restartSelected = false;
            sliderValveTop.value = 0f;
            sliderValveBot.value = 0f;
            valveBot.GetComponent<MeshRenderer>().material = valveBotOriginalMat;
            valveTop.GetComponent<MeshRenderer>().material = valveBotOriginalMat;
            sliderValveBot.GetComponent<Slider>().interactable = true;
            sliderValveTop.GetComponent<Slider>().interactable = true;
            sliderValveBot.gameObject.SetActive(false);
            sliderValveTop.gameObject.SetActive(false);
            sliderName.gameObject.SetActive(false);
            sliderPanel.gameObject.SetActive(false);

        }

        while (true)
        {
            if (!audioSource.isPlaying)
            {

                break;

            }

            yield return null;
        }

        
        


        audioSource.clip = lowsucExploreAudio;
        audioSource.Play();

        yield return new WaitForSeconds(3f);        
        valveBot.GetComponent<MeshRenderer>().material = glowMat;
        

        audioSource2.clip = cavitationClip;
        audioSource2.Play();
        audioSource2.volume = 1f;

        while (true)
        {

            if (sliderValveBot.value > .69f && sliderValveBot.value < .8f)
            {
                botCorrect = true;
                audioSource.clip = success;
                audioSource.Play();
                topCorrect = false;
                botCorrect = false;
                break;
            }


            yield return null;
        }

        sliderValveTop.GetComponent<Slider>().interactable = false;
        sliderValveBot.GetComponent<Slider>().interactable = false;

        while (true)
        {
            if (!audioSource.isPlaying)
            {

                pauseMenu.Pause();
                restartButton.SetActive(true);
                break;

            }

            yield return null;

        }

        yield return null;
    }

    public IEnumerator HighDischargeScenario()
    {
        audioSource.clip = introClip;
        audioSource.Play();
        //debugText.text = "HighDischarge";
        if (restartSelected == true)
        {
            restartSelected = false;
            sliderValveTop.value = 1f;
            sliderValveBot.value = 0f;
            valveBot.GetComponent<MeshRenderer>().material = valveBotOriginalMat;
            valveTop.GetComponent<MeshRenderer>().material = valveBotOriginalMat;
            sliderValveBot.GetComponent<Slider>().interactable = true;
            sliderValveTop.GetComponent<Slider>().interactable = true;
            sliderValveBot.gameObject.SetActive(false);
            sliderValveTop.gameObject.SetActive(false);
            sliderName.gameObject.SetActive(false);
            sliderPanel.gameObject.SetActive(false);

        }

        while (true)
        {
            if (!audioSource.isPlaying)
            {

                break;
            }
            yield return null;
        }

        audioSource.clip = highDisExploreAudio;
        audioSource.Play();
        sliderValveTop.value = 1f;
        yield return new WaitForSeconds(3f);
        valveTop.GetComponent<MeshRenderer>().material = glowMat;
        

        audioSource2.clip = cavitationClip;
        audioSource2.Play();
        audioSource2.volume = 1f;

        while (true)
        {
            if (sliderValveTop.value > .39f && sliderValveTop.value < .49f)
            {
                audioSource.clip = success;
                audioSource.Play();

                break;

            }

            yield return null;
        }
        sliderValveTop.GetComponent<Slider>().interactable = false;
        sliderValveBot.GetComponent<Slider>().interactable = false;

        while (true)
        {
            if (!audioSource.isPlaying)
            {

                pauseMenu.Pause();
                restartButton.SetActive(true);
                break;

            }

            yield return null;
        }
    }

    public IEnumerator RecirculationScenario()
    {
        audioSource.clip = introClip;
        audioSource.Play();

        debugText.text = "Recirculation";
        if (restartSelected == true)
        {
            restartSelected = false;
            sliderValveTop.value = 0f;
            sliderValveBot.value = 0f;
            valveBot.GetComponent<MeshRenderer>().material = valveBotOriginalMat;
            valveTop.GetComponent<MeshRenderer>().material = valveBotOriginalMat;
            sliderValveBot.GetComponent<Slider>().interactable = true;
            sliderValveTop.GetComponent<Slider>().interactable = true;
            sliderValveBot.gameObject.SetActive(false);
            sliderValveTop.gameObject.SetActive(false);
            sliderName.gameObject.SetActive(false);
            sliderPanel.gameObject.SetActive(false);

        }

        while (true)
        {
            if (!audioSource.isPlaying)
            {

                break;
            }

            yield return null;
        }

        audioSource.clip = recircExploreAudio;
        audioSource.Play();
        yield return new WaitForSeconds(3f);
        valveTop.GetComponent<MeshRenderer>().material = glowMat;
        

        audioSource2.clip = recirculationClip;
        audioSource2.Play();
        audioSource2.volume = 1f;

        while (true)
        {
            if (sliderValveTop.value > .49f && sliderValveTop.value < .6f)
            {

                audioSource.clip = success;
                audioSource.Play();

                break;
            }


            yield return null;
        }

        sliderValveTop.GetComponent<Slider>().interactable = false;
        sliderValveBot.GetComponent<Slider>().interactable = false;

        while (true)
        {
            if (!audioSource.isPlaying)
            {

                pauseMenu.Pause();
                restartButton.SetActive(true);
                break;

            }

            yield return null;
        }

        yield return null;
    }

    public IEnumerator ShowValveSliders()
    {

        while (true)
        {
            if (audioSource.isPlaying)
            {
                debugText.text = "should be turning off";
                valveBot.GetComponent<Collider>().enabled = false;
                valveTop.GetComponent<Collider>().enabled = false;

                //break;
            }
            else if (!audioSource.isPlaying)
            {
                if (currentScenario == 0)
                {
                    //low suction
                    valveTop.GetComponent<Collider>().enabled = false;
                    valveBot.GetComponent<Collider>().enabled = true;
                    break;
                }
                else if (currentScenario == 1)
                {
                    //high discharge
                    valveTop.GetComponent<Collider>().enabled = true;
                    valveBot.GetComponent<Collider>().enabled = false;
                    break;
                }
                else if (currentScenario == 2)
                {
                    //recirculation
                    valveTop.GetComponent<Collider>().enabled = true;
                    valveBot.GetComponent<Collider>().enabled = false;
                    break;
                }
            }
            yield return null;
        }

        while (true)
        {

            if (operationsHit.collider.tag == "topValve")
            {
                valveBot.GetComponent<MeshRenderer>().material = valveBotOriginalMat;
                valveTop.GetComponent<MeshRenderer>().material = glowMat;

                sliderPanel.gameObject.SetActive(true);
                sliderName.gameObject.SetActive(true);
                sliderValveTop.gameObject.SetActive(true);
                sliderValveBot.gameObject.SetActive(false);

                //sliderValue.text = sliderValveTop.value.ToString();


                sliderName.text = "Top Intake Valve";
                break;

            }
            else if (operationsHit.collider.tag == "botValve")
            {
                valveTop.GetComponent<MeshRenderer>().material = valveTopOriginalMat;
                valveBot.GetComponent<MeshRenderer>().material = glowMat;

                sliderPanel.gameObject.SetActive(true);
                sliderName.gameObject.SetActive(true);
                sliderValveTop.gameObject.SetActive(false);
                sliderValveBot.gameObject.SetActive(true);

                //sliderValue.text = sliderValveBot.value.ToString();

                sliderName.text = "Side Intake Valve";
                break;
            }
            else
            {
                /*sliderPanel.gameObject.SetActive(false);
                sliderValveTop.gameObject.SetActive(false);
                sliderValveBot.gameObject.SetActive(false);*/
                break;
            }

            yield return null;

        }

        yield return null;
    }
    public void RestartScene()
    {
        //yield return new WaitForSeconds(2f);
        /*audioSource.clip = introClip;
        audioSource.Play();*/

        //SceneManager.LoadScene("OperationsScenarios_LowSuc_PumpApp");
        restartSelected = true;
        pauseMenu.Pause();
        OperationsExploreManager();



    }

    public void CavitationVolume()
    {


        if (audioSource2.clip == cavitationClip && sliderValveBot.value >= .7f)
        {

            audioSource2.volume = 0f;

        }
        else if (sliderValveBot.value <= .7f)
        {

            audioSource2.volume = .5f;

        }
        else
        {

            audioSource2.volume = .25f;

        }

    }

    public void RecirculationVolume()
    {

        if (audioSource2.clip == recirculationClip && sliderValveTop.value >= .5f)
        {

            audioSource2.volume = 0f;

        }
        else if (sliderValveTop.value <= .5f)
        {

            audioSource2.volume = .5f;

        }
        else
        {

            audioSource2.volume = .25f;

        }


    }

    public void HighDischargeVolume()
    {


        if (audioSource2.clip == cavitationClip && sliderValveBot.value >= .4f)
        {

            audioSource2.volume = 0f;

        }
        else if (sliderValveBot.value <= .39f)
        {

            audioSource2.volume = .5f;

        }
        else
        {

            audioSource2.volume = .25f;

        }

    }
}
