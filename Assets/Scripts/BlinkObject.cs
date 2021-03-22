﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlinkObject : MonoBehaviour
{
    public bool isActive;
    public Color32 originalColor = new Color32(79, 143, 212, 217);
    public float speed;
    public float originalSpeed = 3.0f;

    public GameObject arrow;

    //Como este script solo se utilizara para la cuenta atras aprovechamos para modificar el color de la barra y de la cuenta atras
    public TextMeshProUGUI timerText;
    public Image countDownBar;

    void Start()
    {
        isActive = false;
        //originalColor = arrow.gameObject.GetComponent<Renderer>().material.GetColor("_BaseColor");
        
        speed = originalSpeed;
    }


    void Update()
    {
        if (isActive)
        {
            // Arrow
            arrow.gameObject.GetComponent<Renderer>().material.color = Color32.Lerp(originalColor, Color.red, Mathf.PingPong(Time.time * speed, 1.0f));
            //Text
            timerText.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * speed, 1.0f));
            //Bar
            countDownBar.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * speed, 1.0f));
        }
    }

    public void ResetValues()
    {
        arrow.gameObject.GetComponent<Renderer>().material.color = originalColor;
        speed = originalSpeed;
        isActive = false;
        countDownBar.color = Color.white;
        timerText.color = Color.white;
    }

    public void ResetArrowColor()
    {
        arrow.gameObject.GetComponent<Renderer>().material.color = originalColor;
    }
}
