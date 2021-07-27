using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssemblyHelper : MonoBehaviour
{
    public List<GameObject> pumpObjects = new List<GameObject>();
    public Dictionary<string, string> animDictionary = new Dictionary<string, string>();
    [SerializeField]
    Text debugText;

    private Vector3 objectScale;

    // Start is called before the first frame update
    void Start()
    {
        PopulateAnimDict();
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

    public void PlayDictAnim(string pumpPart)
    {
        Animator pumpAnim = GameObject.Find(pumpPart).GetComponent<Animator>();
        pumpAnim.Play(animDictionary[pumpPart]);
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

    private void PopulateAnimDict()
    {
        animDictionary.Add("Pump@Bearing", "Bearing_Place");
        animDictionary.Add("Pump@Couplings", "Couplings_Place");
        animDictionary.Add("Pump@FrontRearSuction&Housing", "FrontRearSuction&Housing_Place");
        animDictionary.Add("Pump@Impeller_Vanes", "Impeller_Vanes_Place");
        animDictionary.Add("Pump@MechanicalSeal", "MechanicalSeal_Place");
        animDictionary.Add("Pump@Motor", "Motor_Place");
        animDictionary.Add("Pump@Shaft", "Shaft_Place");
        animDictionary.Add("Pump@ShaftHousing", "ShaftHousing_Place");
    }
}
