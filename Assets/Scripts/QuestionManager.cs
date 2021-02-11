using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionManager : MonoBehaviour {
    public List<Question> questions;
    Dictionary<Vector2Int,Question> questionDict = new Dictionary<Vector2Int,Question>();
    Question currentQuest;
    int currentQuestIndexOffset = 0;

    [SerializeField]
    GameObject qAPanel;
    GameObject questionPanel;
    GameObject answerLayout;
    AudioSource audioSource;

    [SerializeField]
    AudioClip buttonSound;
    [SerializeField]
    AudioClip correctSound;
    [SerializeField]
    AudioClip incorrectSound;

    List<TextMeshProUGUI> textFade = new List<TextMeshProUGUI>();
    List<Image> imageFade = new List<Image>();

    int numInput;

    public float audioDelay;
    public bool inQuestion = false;

    private void Awake() {
        int indexOffset = 0;
        int prevIndex = -1;
        foreach (Question question in questions) {
            if (question.step == prevIndex) indexOffset++; else indexOffset = 0;
            questionDict.Add(new Vector2Int(question.step,indexOffset),question);
            prevIndex = question.step;
        }
        if (qAPanel != null) {
            questionPanel = qAPanel.transform.GetChild(0).gameObject;
            if (buttonSound != null) {
                audioSource = qAPanel.AddComponent<AudioSource>();
            }
        }
        qAPanel.SetActive(false);
    }

    int[] numGrid = new int[] { 7,8,9,4,5,6,1,2,3 };

    public void StartQuest(int step) {
        inQuestion = true;
        qAPanel.SetActive(true);
        currentQuest = questionDict[new Vector2Int(step,currentQuestIndexOffset)];
        if (answerLayout == null) answerLayout = Instantiate(currentQuest.answerLayout,qAPanel.transform);
        if (currentQuestIndexOffset == 0) {
            if (currentQuest.qType == QuestionType.Numpad) {
                Transform panel = answerLayout.transform.Find("Panel");
                panel.Find("Enter").GetComponent<Button>().onClick.AddListener(delegate { CheckNumpad(numInput); });
                int i = 0;
                foreach (Transform button in panel.Find("NumGrid")) {
                    int v = numGrid[i];
                    //Debug.Log("Index: " + i + " Value: " + v);
                    button.GetComponent<Button>().onClick.AddListener(delegate { UpdateNumInput(v); });
                    i++;
                }
                panel.Find("Zero").GetComponent<Button>().onClick.AddListener(delegate { UpdateNumInput(0); });
                //Debug.Log("Numpad Listeners Added");
            } else {
                int i = 0;
                foreach (Transform ansBut in answerLayout.transform) {
                    ansBut.GetComponent<Button>().onClick.AddListener(delegate { CheckAnswer(i); });
                    i++;
                }
                //Debug.Log("Multiple Choice Listeners Added");
            }
        }
        //Debug.Log("Updating UI");
        StartCoroutine(UpdateUI(1,currentQuest,audioDelay));
        //qAPanel.SetActive(true);
        //Debug.Log("UI Finished Updating");
    }

    void CheckAnswer(int choice) {
        PlayButtonSound();
        if (choice == currentQuest.answer) {
            if (correctSound != null) {
                audioSource.clip = correctSound;
                audioSource.Play();
            }
            if (questionDict.ContainsKey(new Vector2Int(currentQuest.step,currentQuestIndexOffset + 1))) {
                currentQuestIndexOffset++;
                StartQuest(currentQuest.step);
            } else {
                currentQuestIndexOffset = 0;
                Destroy(answerLayout);
                Invoke("DisableUI",1);
                inQuestion = false;
            }
        } else {
            if (incorrectSound != null) {
                audioSource.clip = incorrectSound;
                audioSource.Play();
            }
        }
    }

    void CheckNumpad(int input) {
        //PlayButtonSound();
        if (input == currentQuest.answer) {
            if (correctSound != null) {
                audioSource.clip = correctSound;
                audioSource.Play();
            }
            if(questionDict.ContainsKey(new Vector2Int(currentQuest.step,currentQuestIndexOffset + 1))) {
                currentQuestIndexOffset++;
                StartQuest(currentQuest.step);
            } else {
                currentQuestIndexOffset = 0;
                Destroy(answerLayout);
                Invoke("DisableUI",1);
                inQuestion = false;
            }
        } else {
            if (incorrectSound != null) {
                audioSource.clip = incorrectSound;
                audioSource.Play();
            }  
        }
        UpdateNumInput(-1);
    }

    public void UpdateNumInput(int i) {
        PlayButtonSound();
        string numString = $"{numInput}" + i;
        if (i == -1) numString = "00000";
        if (numString.Length > 5) numString = numString.Substring(1);
        numInput = int.Parse(numString);
        answerLayout.transform.Find("Panel").Find("NumDisplay").GetComponentInChildren<TextMeshProUGUI>().text = numInput.ToString();
    }

    public void PlayButtonSound() {
        audioSource.PlayOneShot(buttonSound);
    }

    void UpdateFadeLists(QuestionType type) {
        textFade.Add(questionPanel.GetComponentInChildren<TextMeshProUGUI>());
        imageFade.Add(qAPanel.GetComponent<Image>());
        foreach (Transform obj in qAPanel.transform) {
            if (obj.GetComponent<Image>() != null) imageFade.Add(obj.GetComponent<Image>());
        }
        foreach (Transform obj in qAPanel.transform) {
            if (obj.GetComponent<TextMeshProUGUI>() != null) textFade.Add(obj.GetComponent<TextMeshProUGUI>());
        }
    }

    IEnumerator UpdateUI(float fadeDur,Question quest = null,float delay = 0,bool fadeIn = true) {
        if (fadeIn && quest != null) {
            UpdateFadeLists(quest.qType);
            textFade[0].text = quest.question;
            if (quest.qType == QuestionType.Numpad) {

            } else {
                for (int i = 1; i < textFade.Count; i++) {
                    textFade[i].text = quest.choices[i - 1];
                }
            }
        }

        float elapsedTime = 0;

        if (delay > 0) {
            while (elapsedTime < delay) {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        qAPanel.SetActive(true);

        //alpha fading to be done at a later date

        yield return null;
    }

    public void DisableUI() {
        qAPanel.SetActive(false);
    }
}
