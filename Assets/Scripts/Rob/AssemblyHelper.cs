using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssemblyHelper : MonoBehaviour
{
    public List<GameObject> pumpObjects = new List<GameObject>();
    [SerializeField]
    Text debugText;

    private Vector3 objectScale;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayImpellerAnim()
    {
        Animator pumpAnim = GameObject.Find("Pump@Impeller_Vanes").GetComponent<Animator>();
        pumpAnim.Play("Impeller_Vanes_Place");
        

    }
    //Vector3(0.0297999997,0.00499999989,0.0179999992)

    public void ResizeToNormal(string pumpPart)
    {
        GameObject.Find(pumpPart).transform.localScale = GameObject.Find("Pump@Base").transform.localScale;
        debugText.text += "resizing, scale is "+ GameObject.Find(pumpPart).transform.localScale.x;
    }

    public void ManuallyPlayAnim(string pumpPart)
    {
        Animator partAnim = GameObject.Find(pumpPart).GetComponent<Animator>();
    }

    public void GetScale(string pumpPart)
    {
        objectScale = GameObject.Find(pumpPart).transform.localScale;
        debugText.text += "getting scale";
        foreach(GameObject go in pumpObjects)
        {
            go.transform.localScale = new Vector3(0f, 0f, 0f);
        }
    }
}
