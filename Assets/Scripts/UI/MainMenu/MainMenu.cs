using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour {

    TextMeshProUGUI[] chapterTitles = new TextMeshProUGUI[3];
    //TextMeshProUGUI[] chapterSummaries = new TextMeshProUGUI[3];

    //Image[] chapterIcons = new Image[3];
    Image[] moduleIcons = new Image[3];

    GameObject moreChapters;
    GameObject prevChapters;
    GameObject eventManager;

    Button[] buttons = new Button[3];

    int lowIndex = 0;
    int upIndex = 2;
    int modIndex = 0;

    [SerializeField]
    [Header("DON'T SET SIZE HIGHER THAN 3")]
    Module[] modules;

    //Init global variables from objects in scene
    public void Awake() {

        try 
        {

            chapterTitles[0] = GameObject.Find("Chapter1/ChapterTitle").GetComponent<TextMeshProUGUI>();
            chapterTitles[1] = GameObject.Find("Chapter2/ChapterTitle").GetComponent<TextMeshProUGUI>();
            chapterTitles[2] = GameObject.Find("Chapter3/ChapterTitle").GetComponent<TextMeshProUGUI>();
            //chapterSummaries[0] = GameObject.Find("Chapter1/ChapterSummary").GetComponent<TextMeshProUGUI>();
            //chapterSummaries[1] = GameObject.Find("Chapter2/ChapterSummary").GetComponent<TextMeshProUGUI>();
            //chapterSummaries[2] = GameObject.Find("Chapter3/ChapterSummary").GetComponent<TextMeshProUGUI>();
            moreChapters = GameObject.Find("MoreChapters");
            prevChapters = GameObject.Find("BackChapters");
            eventManager = GameObject.Find("EventSystem");
            //chapterIcons[0] = GameObject.Find("Chapter1/ChapterIcon").GetComponent<Image>();
            //chapterIcons[1] = GameObject.Find("Chapter2/ChapterIcon").GetComponent<Image>();
            //chapterIcons[2] = GameObject.Find("Chapter3/ChapterIcon").GetComponent<Image>();
            moduleIcons[0] = GameObject.Find("TopModule/ModuleIcon").GetComponent<Image>();
            moduleIcons[1] = GameObject.Find("MiddleModule/ModuleIcon").GetComponent<Image>();
            moduleIcons[2] = GameObject.Find("BottomModule/ModuleIcon").GetComponent<Image>();
            buttons[0] = GameObject.Find("Chapter1").GetComponent<Button>();
            buttons[1] = GameObject.Find("Chapter2").GetComponent<Button>();
            buttons[2] = GameObject.Find("Chapter3").GetComponent<Button>();

        }
        catch 
        {

            Debug.Log("setup not working");
        
        }
        
    }

    public void HighlightModuleButton(int buttonPressed) {
        switch (buttonPressed) {
            case 0:
                moduleIcons[0].color = new Color(0.9411765f,0.9019608f,0.1960784f);
                moduleIcons[1].color = Color.white;
                moduleIcons[2].color = Color.white;
                modIndex = 0;
                break;
            case 1:
                moduleIcons[0].color = Color.white;
                moduleIcons[1].color = new Color(0.9411765f,0.9019608f,0.1960784f);
                moduleIcons[2].color = Color.white;
                modIndex = 1;
                break;
            case 2:
                moduleIcons[0].color = Color.white;
                moduleIcons[1].color = Color.white;
                moduleIcons[2].color = new Color(0.9411765f,0.9019608f,0.1960784f);
                modIndex = 2;
                break;
        }
    }

    public void UpdateIndexBounds(int buttonPressed) {
        int chapLen = modules[modIndex].chapters.Capacity;
        if (buttonPressed == 1) {
            lowIndex += 3;
            if (upIndex + 4 > chapLen) {
                upIndex += 3 - ((chapLen - upIndex) % 3);
            } else upIndex += 3;
        } else if (buttonPressed == -1) {
            if ((upIndex + 1) % 3 != 0) {
                if (upIndex == lowIndex) upIndex -= 1; else upIndex -= 3 - upIndex % 3;
            } else upIndex -= 3;
            lowIndex -= 3;
        } else {
            lowIndex = 0;
            if (chapLen < 3) {
                upIndex = chapLen - 1;
            } else upIndex = 2;
        }

        if (lowIndex == 0) {
            prevChapters.GetComponent<Button>().interactable = false;
        } else prevChapters.GetComponent<Button>().interactable = true;

        if (chapLen > 3 && upIndex + 1 < chapLen) {
            moreChapters.GetComponent<Button>().interactable = true;
        } else moreChapters.GetComponent<Button>().interactable = false;
        StartCoroutine(UpdateText(new Color(1,1,1,1),0.5f));
    }

    public void LoadScene(int buttonPressed) {
        SceneManager.LoadScene(modules[modIndex].chapters[lowIndex + buttonPressed].chapterScene);
    }

    IEnumerator UpdateText(Color color, float time) {
        float dTime = 0;

        #region FadeOut
        while(dTime < time) {
            for(int i = 0; i < chapterTitles.Length; i++) {
                chapterTitles[i].color = new Color(color.r,color.g,color.b,Mathf.Lerp(chapterTitles[i].color.a,0,(dTime / time)));
                //chapterSummaries[i].color = new Color(color.r,color.g,color.b,Mathf.Lerp(chapterSummaries[i].color.a,0,(dTime / time)));
                //chapterIcons[i].color = new Color(0.3176471f,0.3176471f,0.3176471f,Mathf.Lerp(chapterIcons[i].color.a,0,(dTime / time)));
            }

            //update elapsed time
            dTime += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < chapterTitles.Length; i++) {
            chapterTitles[i].color = new Color(color.r,color.g,color.b,0);
            //chapterSummaries[i].color = new Color(color.r,color.g,color.b,0);
            //chapterIcons[i].color = new Color(0.3176471f,0.3176471f,0.3176471f,0);
        }
        #endregion

        dTime = 0;
        color = new Color(color.r,color.g,color.b,0);

        #region UpdateButtons
        for (int i = 0; i < 2; i++) if (i > upIndex) buttons[i].interactable = false;

        var modChaps = modules[modIndex].chapters;

        if(lowIndex == upIndex) {
            int transIndex = lowIndex - upIndex;
            buttons[transIndex].interactable = true;
            chapterTitles[transIndex].text = modChaps[lowIndex].chapterTitle;
            //chapterSummaries[transIndex].text = modChaps[lowIndex].chapterSummary;
            //chapterIcons[transIndex].sprite = modChaps[lowIndex].chapterIcon;
            for(int i = transIndex + 1; i < transIndex + 3; i++) {
                Debug.Log("" + i);
                chapterTitles[i].text = "";
                //chapterSummaries[i].text = "";
                //chapterIcons[i].sprite = null;
            }
        } else {
            for (int i = lowIndex; i < upIndex + 1; i++) {
                buttons[i - lowIndex].interactable = true;
                if(i != upIndex + 1) {
                    chapterTitles[i - lowIndex].text = modChaps[i].chapterTitle;
                    //chapterSummaries[i - lowIndex].text = modChaps[i].chapterSummary;
                    //chapterIcons[i - lowIndex].sprite = modChaps[i].chapterIcon;
                } else {
                    chapterTitles[i - lowIndex].text = modChaps[i - 1].chapterTitle;
                    //chapterSummaries[i - lowIndex].text = modChaps[i - 1].chapterSummary;
                    //chapterIcons[i - lowIndex].sprite = modChaps[i - 1].chapterIcon;
                } 
            }
        }
        if(upIndex < 2) {
            for(int i = upIndex + 1; i < 3; i++) {
                chapterTitles[i].text = "";
                //chapterSummaries[i].text = "";
                //chapterIcons[i].sprite = null;
            }
        } else if(upIndex > modChaps.Capacity) {        //I don't know if it ever actually gets to this block
            for(int i = upIndex; i > modChaps.Capacity - 1; i--) {
                Debug.Log("" + (i - modChaps.Capacity - 1));
                chapterTitles[i - modChaps.Capacity - 1].text = "";
                //chapterSummaries[i - modChaps.Capacity - 1].text = "";
                //chapterIcons[i - modChaps.Capacity - 1].sprite = null;
            }
        }
        #endregion

        #region FadeIn
        while (dTime < time) {
            //fade each text element from list back in from the given color value to an alpha of 1
            foreach (TextMeshProUGUI elem in chapterTitles) {
                elem.color = new Color(color.r,color.g,color.b,Mathf.Lerp(color.a,1,dTime / time));
            }
            /*foreach (TextMeshProUGUI elem in chapterSummaries) {
                elem.color = new Color(color.r,color.g,color.b,Mathf.Lerp(color.a,1,dTime / time));
            }*/
            //fade each sprite element from list back in from the hard coded value to an alpha of 1
            /*foreach (Image elem in chapterIcons) {
                elem.color = new Color(0.3176471f,0.3176471f,0.3176471f,Mathf.Lerp(color.a,1,dTime / time));
            }*/
            dTime += Time.deltaTime;
            yield return null;
        }
        foreach (TextMeshProUGUI elem in chapterTitles) {
            elem.color = new Color(color.r,color.g,color.b,1);
        }
        /*foreach (TextMeshProUGUI elem in chapterSummaries) {
            elem.color = new Color(color.r,color.g,color.b,1);
        }*/
        /*foreach (Image elem in chapterIcons) {
            elem.color = new Color(0.3176471f,0.3176471f,0.3176471f,1);
        }*/
        #endregion
    }
}
