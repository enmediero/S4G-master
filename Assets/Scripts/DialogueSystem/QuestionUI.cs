﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionUI : MonoBehaviour
{
    public TextMeshProUGUI description;
    public List<Button> answers = new List<Button>();
    [System.NonSerialized]
    public QuestionNodeData questionData;
    private List<AnswerNodeData> answersData;
    public GameObject AnswerContainer;

    void Start()
    {

    }


    void Update()
    {

    }

    public void SetupQuestion()
    {
        description.text = questionData.Description;
        Debug.Log($"Setup description: {questionData.Description}");

        UI_Manager.Instance.lastTime = Time.time;
        answersData = UI_Manager.Instance.dialogueContainer.GetQuestionAnswers(questionData.Guid);

        UI_Manager.Instance.AddTextToRoute("To the question: " + questionData.QuestionName + " " + questionData.Description);

        //Debug.Log($"Se ha configurado la pregunta {questionData.QuestionName}, sus respuestas son {answersData[0].AnswerName}");//, {answersData[1].AnswerName}, {answersData[2].AnswerName}, {answersData[3].AnswerName}

        int i = 0;
        for (i = 0; i < answersData.Count; i++)
        {
            //Debug.Log($"Vamos a configurar el slot {i} con {answersData[i].AnswerName}, longitud de answers = {answers.Count}, longitud de answersData = {answersData.Count}");
            answers[i].GetComponent<AnswerUI>().SetupAnswer(answersData[i]);
            //Debug.Log($"El slot {i} tiene la respuesta {answersData[i].AnswerName}");
        }
        
        for (int j = i; j < answers.Count; j++)
        {
            answers[j].GetComponent<AnswerUI>().ClearAnswer();
            //Debug.Log($"El slot {j} esta clear");
        }

        StartCoroutine(UI_Manager.Instance.PlaySimpleDialogue(questionData.audioId, AnswerContainer));

    }

}
