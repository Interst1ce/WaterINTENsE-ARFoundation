using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmpButtonTest : MonoBehaviour
{

    [SerializeField]
    Text debugText;
    // Start is called before the first frame update
    void Start()
    {

        //Button btn = gameObject.GetComponent<Button>();
        //btn.onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick()
    {
        debugText.text = "You have clicked the button!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
