using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssemblyHelper : MonoBehaviour
{
    public List<GameObject> pumpObjects = new List<GameObject>();
    public Dictionary<string, string> animDictionary = new Dictionary<string, string>();
   

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
        
    }

    public void ManuallyPlayAnim(string pumpPart)
    {
        Animator partAnim = GameObject.Find(pumpPart).GetComponent<Animator>();
    }

    public void GetScale(string pumpPart)
    {
        objectScale = GameObject.Find(pumpPart).transform.localScale;
       
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

    public void DisableCollider(string pumpPart)
    {
        GameObject.Find(pumpPart).GetComponent<Collider>().enabled = false;
    }
    public void EnableCollider(string pumpPart)
    {
        GameObject.Find(pumpPart).GetComponent<Collider>().enabled = true;
    }
    public IEnumerator TempDisableCollider(string pumpPart)
    {
        GameObject.Find(pumpPart).GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        GameObject.Find(pumpPart).GetComponent<Collider>().enabled = false;
        
    }

    public void StartTempDisableCollider(string pumpPart)
    {
        StartCoroutine(TempDisableCollider(pumpPart));
    }

    public IEnumerator WaitAndResizeToNorm(string pumpPart)
    {
        yield return new WaitForSeconds(1f);
        GameObject.Find(pumpPart).transform.localScale = GameObject.Find("Pump@Base").transform.localScale;
    }

    public void StartWaitAndResize(string pumpPart)
    {
        StartCoroutine(WaitAndResizeToNorm(pumpPart));
    }
}
