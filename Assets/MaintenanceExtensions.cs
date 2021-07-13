using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaintenanceExtensions : MonoBehaviour
{
    [SerializeField]
    GameObject ampMeter;
    [SerializeField]
    GameObject thermoMeter;
    [SerializeField]
    GameObject dripCounter;

    [SerializeField]
    GameObject pump;

    [SerializeField]
    PauseMenu pauseMenu;

    [SerializeField]
    Text debugText;

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip finalAudio;



    bool restartSelected = false;
    bool ampMeterActive = false;
    bool thermoMeterActive = false;
    bool dripMeterActive = false;



    // Start is called before the first frame update
    void Start()
    {
        restartSelected = false;
        ampMeter.SetActive(false);
        thermoMeter.SetActive(false);
        dripCounter.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (ampMeter.gameObject.activeSelf)
        {
            thermoMeter.gameObject.transform.localScale.Set(0f, 0f, 0f);
            dripCounter.gameObject.transform.localScale.Set(0f, 0f, 0f);

            thermoMeter.SetActive(false);
            dripCounter.SetActive(false);

        }
        else if (thermoMeter.gameObject.activeSelf)
        {

            ampMeter.gameObject.transform.localScale.Set(0f, 0f, 0f);
            dripCounter.gameObject.transform.localScale.Set(0f, 0f, 0f);

            ampMeter.SetActive(false);
            dripCounter.SetActive(false);

        }
        else if (dripCounter.gameObject.activeSelf) 
        {

            thermoMeter.gameObject.transform.localScale.Set(0f, 0f, 0f);
            ampMeter.gameObject.transform.localScale.Set(0f, 0f, 0f);

            thermoMeter.SetActive(false);
            ampMeter.SetActive(false);

        }*/

    }

    public void EnableAmpMeter()
    {
        //StartCoroutine(Wait(2f));

        ampMeter.SetActive(true);




    }

    public void EnableThermoMeter()
    {

        //lerp the alpha to 0 here
        /*StartCoroutine(Wait(2f));
        StartCoroutine(PumpOpacity(1f, 1f));*/

        StartCoroutine(EnableThermometerCheck());
        



    }

    public void EnableDripCounter()
    {
        //lerp alpha back up
        /*StartCoroutine(Wait(2f));
        StartCoroutine(PumpOpacity(0f, 1f));*/

        StartCoroutine(EnableDripmeterCheck());


    }
    public IEnumerator Wait(float waitTime) 
    {

        yield return new WaitForSeconds(waitTime);
    }

    public IEnumerator PumpOpacity(float aValue, float aTime)
    {
        float alpha = pump.GetComponent<MeshRenderer>().material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            pump.GetComponent<MeshRenderer>().material.color = newColor;
            yield return null;
        }
    }
    public void LastStepAudio() 
    {


        StartCoroutine(PlayFinalAudio());
    
    
    }

    public IEnumerator PlayFinalAudio() 
    {
        debugText.text = "reached play final audio";
        yield return new WaitForSeconds(3f);
        /*
        while (true)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = finalAudio;
                audioSource.Play();
                break;

            }
            yield return null;

        }*/
        
        while (true)
        {
            if (!audioSource.isPlaying)
            {
                pauseMenu.Pause();
                break;

            }
            yield return null;

        }
        debugText.text = "before pause";
        
        yield return null;

    }
    public void EnableRestart()
    {
        //yield return new WaitForSeconds(2f);
        /*audioSource.clip = introClip;
        audioSource.Play();*/

        //SceneManager.LoadScene("OperationsScenarios_LowSuc_PumpApp");
        StartCoroutine(RestartScene(1f));
        

    }

    public IEnumerator RestartScene(float waitTime) 
    {

        yield return new WaitForSeconds(waitTime);

        try
        {

            restartSelected = true;
            pauseMenu.Pause();

        }
        catch 
        {

            debugText.text = "Restart wrong";
        
        
        }


        yield return null;
    }

    public IEnumerator EnableThermometerCheck() 
    {

        while (true) 
        {
            if(!audioSource.isPlaying) 
            {

                thermoMeter.SetActive(true);
                ampMeter.SetActive(false);
                dripCounter.SetActive(false);
                break;
            }

            yield return null;
        }
        yield return null;
    
    }

    public IEnumerator EnableDripmeterCheck() 
    {

        while (true)
        {
            if (!audioSource.isPlaying)
            {
                
                dripCounter.SetActive(true);
                thermoMeter.SetActive(false);
                ampMeter.SetActive(false);
                break;
            }

            
            yield return null;
        }
        yield return null;

    }


}


