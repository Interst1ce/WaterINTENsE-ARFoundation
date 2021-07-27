using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalysisHelper : MonoBehaviour
{
    [SerializeField]
    StoryManager sManager;
    [SerializeField]
    Text debugText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //debugText.text = "current step: " + sManager.currentStep;
    }
}
