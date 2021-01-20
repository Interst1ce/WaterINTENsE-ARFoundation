using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class MultimeterUI : MonoBehaviour {
    public TextMeshProUGUI multimeterReading;
    public TextMeshProUGUI tempIndicator;
    public TextMeshProUGUI phIndicator;
    public TextMeshProUGUI doIndicator;

    public Color screenColor;

    public GameObject multimeterCamera;

    public async void ReadingUpdate(int reading) {
        switch (reading) {
            case 1:
                multimeterReading.text = "9";
                tempIndicator.color = Color.black;
                phIndicator.color = screenColor;
                doIndicator.color = Color.black;
                multimeterCamera.SetActive(true);
                break;
            case 2:
                multimeterReading.text = "52";
                tempIndicator.color = screenColor;
                phIndicator.color = Color.black;
                doIndicator.color = Color.black;
                break;
            case 3:
                multimeterReading.text = "0";
                tempIndicator.color = Color.black;
                phIndicator.color = Color.black;
                doIndicator.color = screenColor;
                await Task.Delay(TimeSpan.FromSeconds(4));
                break;
            default:
                break;
        }
    }
}
