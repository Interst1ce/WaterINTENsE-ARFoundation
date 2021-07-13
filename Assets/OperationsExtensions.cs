using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Threading.Tasks;

public class OperationsExtensions : MonoBehaviour
{
    //you need a reference to the storymanager script to get this to work--this is just a variable with no reference, hence why it won't work
    [SerializeField]
    StoryManager sm;
    [SerializeField]
    GameObject slider;
    [SerializeField]
    Text debugText;
    [SerializeField]
    AudioClip completeAudio;

    [SerializeField]
    Target currentTarget;

    [SerializeField]
    PauseMenu pauseMenu;
    //private float sliderValue = GameObject.Find("Slider").GetComponent<Slider>().value;





    //bool hasNotStarted = true;


    /* Coroutine example
    public IEnumerable SomeCoroutine()
    {
        while (true)
        {
            if (hasNotStarted)
            {
                yield return new WaitForSeconds(3f);
                hasNotStarted = false;
                break;
            }
            yield return null;
        }

        //rest of function
        yield return null;
    }*/
    public void Awake()
    {
        slider.SetActive(false);
    }


    public void DisplaySlider()
    {


        //await Task.Delay(System.TimeSpan.FromSeconds(2));

        //AudioClip currentAudio = sm.steps[2].step.targets[0].targetAudio; **How to get a reference to a specific audio clip**


        //slider.gameObject.GetComponent<RectTransform>().localScale.Set(0, 0, 0);
        //debugText.text = "DisplaySlider()";

        debugText.text = "Audio Playing?";
        /*while (sm.currentStep == 2)
        {
            debugText.text = "While";
          
            if (!sm.audioSource.isPlaying)
            {
                debugText.text = "if";
                slider.SetActive(true);
                
            }*/

        //slider = GameObject.FindGameObjectWithTag("Slider");



        //slider.gameObject.GetComponent<RectTransform>().localScale.Set(4.48784542f, 4.48784542f, 4.48784542f);
        //debugText.text = sm.currentStep.ToString();


        //await Task.Yield();

        //}

        //StartCoroutine(DisplaySliderCoroutine());
    }

    //doing exact same as async above, just with a coroutine
    public IEnumerator DisplaySliderCoroutine()
    {

        RaycastHit topValveHit;
        //Ray ray = Camera.main.ScreenPointToRay(Input.to);
        //topValveHit.transform.gameObject.tag == "topValve";
        //not getting to this first part /////////////////////////
        slider.GetComponent<Slider>().interactable = false;

        debugText.text = "starting displayslider coroutine";
        debugText.text = "audiosource: " + sm.audioSource.isPlaying;

        while (true)
        {

            if (sm.audioSource.isPlaying)
            {

                break;

            }

            yield return null;

        }
        while (true)
        {
            if (sm.audioSource.isPlaying)
            {
                debugText.text = "should be turning off";
                slider.SetActive(false);
                slider.GetComponent<Slider>().interactable = false;

                //break;
            }
            else if (!sm.audioSource.isPlaying)
            {
                yield return new WaitForSeconds(1f);
                debugText.text = "inside isplaying check";
                slider.SetActive(true);
                slider.GetComponent<Slider>().interactable = true;
                break;
                /*if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
                {
                    Ray ray;
                    ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

                    if (Physics.Raycast(ray, out topValveHit))
                    {
                        if (topValveHit.collider.tag == "topValve")
                        {

                        }

                    }

                }*/
                
            }
            yield return null;
            
        }
        debugText.text = "done with routine";
        //not getting to above first part //////////////////////

        while (true)
        {
                //debugText.text = "slider value: " + slider.GetComponent<Slider>().value;
                if (slider.GetComponent<Slider>().value == 1f)
                {
                    debugText.text = "inside sliderValue check";
                    slider.SetActive(false);
                    sm.audioSource.clip = completeAudio;
                    sm.audioSource.Play();

                    break;

                }

                
                yield return null;

        }
        //sm.EndStory(currentTarget);
        yield return new WaitForSeconds(3f);
        pauseMenu.Pause();
        yield return null;
        
    }

    /*public IEnumerator SliderValueCheckStepTwo() 
   {

       debugText.text = "starting slider SliderValueCheckStepTwo()";
       while (true) 
       {
           if(slider.val == 1) 
           {
               debugText.text = "inside sliderValue check";
               slider.SetActive(false);
               sm.audioSource.clip = completeAudio;
               sm.audioSource.Play();
               break;

           }
           yield return null;

       }
       yield return null;

   }*/

    public void StartSliderCoroutine()
    {
        StartCoroutine(DisplaySliderCoroutine());
    }

    /*public void StartSliderValueCheckStepTwoCoroutine() 
    {
        StartCoroutine(SliderValueCheckStepTwo());
    }*/

}