using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextGetPercentage : MonoBehaviour
{
    public GameObject agent;

    private Text timerText;

    private void Start()
    {
        timerText = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = (int)(agent.GetComponent<MowerAgent>().cumsumReward * 100f) + "% completed";
    }
}
