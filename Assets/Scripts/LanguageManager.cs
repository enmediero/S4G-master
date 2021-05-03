﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{

    public static LanguageManager Instance;

    
    public string languageSelected = "EN";
    public string caseSelected = "Case3";

    public bool isThirdPerson;

    //Languages management //uTILIZAREMOS ESTAS LISTAS INDEPENDIENTEMENTE DEL IDIOMA
    public List<DialogueDataBean> case3LanguageData = new List<DialogueDataBean>(); //metemos las situaciones, questions y dialogues en el mismo saco
    public List<DialogueDataBean> case5LanguageData = new List<DialogueDataBean>();
    public List<DialogueDataBean> case6LanguageData = new List<DialogueDataBean>();
    public List<DialogueDataBean> case7LanguageData = new List<DialogueDataBean>();
    public List<DialogueDataBean> case9LanguageData = new List<DialogueDataBean>();





    void Awake()
    {
        if (Instance != null)
        {
            GameObject.Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        if (PlayerPrefs.GetString("Language")!= null)
        {
            languageSelected = PlayerPrefs.GetString("Language");
        }

        isThirdPerson = false;




        //Testing
        string[] data;
        //Audio/Case5_EN/Audio1
        //Resources.Load
        //C:\Users\Joaquin\Documents\GitHub\S4G-master\Languages\Example.csv
        //ReadCSV("C:/Users/Joaquin/Documents/GitHub/S4G-master/Assets/Resources/Languages/Example.csv", out data);


        //Real data input:
        //Esta ruta dependera del idioma seleccionado, de momento usamos siempre la misma
        ReadCSVLanguages("C:/Users/Joaquin/Documents/GitHub/S4G-master/Assets/Resources/Languages/Example.csv");

    }
    public void SelectLanguage(string languageSiglas)
    {
        languageSelected = languageSiglas;
        PlayerPrefs.SetString("Language", languageSelected);
    }


    public string GetSituationContext(SituationNodeData situation)
    {
        List<DialogueDataBean> aux = GetDatalist(); //obtenemos la lista del caso e idioma correspondiente

        //para el ingles no hace falta consultar el csv (de momento)
        if (aux != null && languageSelected != "EN") //sera null cuando el idioma sea ingles o haya habido algun error
        {
            for (int i = 0; i < aux.Count; i++)
            {
                if (aux[i].name == situation.SituationName)
                {
                    return aux[i].text;
                }
            }
        }

        return situation.Context;
    }

    public string GetDialogueSpeaker(DialogueNodeData dialogueNodeData)
    {
        List<DialogueDataBean> aux = GetDatalist(); //obtenemos la lista del caso e idioma correspondiente

        //para el ingles no hace falta consultar el csv (de momento)
        if (aux != null && languageSelected != "EN") //sera null cuando el idioma sea ingles o haya habido algun error
        {
            for (int i = 0; i < aux.Count; i++)
            {
                if (aux[i].name == dialogueNodeData.DialogueName)
                {
                    return aux[i].speaker;
                }
            }
        }

        return dialogueNodeData.Speaker;
    }

    public string GetDialogueText(DialogueNodeData dialogueNodeData)
    {
        List<DialogueDataBean> aux = GetDatalist(); //obtenemos la lista del caso e idioma correspondiente

        //para el ingles no hace falta consultar el csv (de momento)
        if (aux != null && languageSelected != "EN") //sera null cuando el idioma sea ingles o haya habido algun error
        {
            for (int i = 0; i < aux.Count; i++)
            {
                if (aux[i].name == dialogueNodeData.DialogueName)
                {
                    return aux[i].text;
                }
            }
        }

        return dialogueNodeData.DialogueText;
    }


    public string GetQuestionDescription(QuestionNodeData questionNodeData)
    {
        List<DialogueDataBean> aux = GetDatalist(); //obtenemos la lista del caso e idioma correspondiente

        //para el ingles no hace falta consultar el csv (de momento)
        if (aux != null && languageSelected != "EN") //sera null cuando el idioma sea ingles o haya habido algun error
        {
            for (int i = 0; i < aux.Count; i++)
            {
                if (aux[i].name == questionNodeData.QuestionName)
                {
                    return aux[i].text;
                }
            }
        }

        return questionNodeData.Description;
    }

    public string GetQuestionSpeaker(QuestionNodeData questionNodeData)
    {
        List<DialogueDataBean> aux = GetDatalist(); //obtenemos la lista del caso e idioma correspondiente

        //para el ingles no hace falta consultar el csv (de momento)
        if (aux != null && languageSelected != "EN") //sera null cuando el idioma sea ingles o haya habido algun error
        {
            for (int i = 0; i < aux.Count; i++)
            {
                if (aux[i].name == questionNodeData.QuestionName)
                {
                    return aux[i].speaker;
                }
            }
        }

        return questionNodeData.speaker;
    }

    void ReadCSV(string path, out string[] data) //este sera el read de dialogue y question
    {
        data = null;
        StreamReader strReader = new StreamReader(path);

        bool endOfFile = false;
        while (!endOfFile)
        {
            string data_String = strReader.ReadLine();
            if (data_String == null)
            {
                endOfFile = true;
                break;
            }

            data = data_String.Split(';');

            //Llegados a este punto, data es una linea del csv, que esta compuesta por:
            //CASO;ID(O NOMBRE)¨;SPEAKER;TEXTO

        }
    }

    void ReadCSVLanguages(string path) //este sera el read de dialogue y question
    {
        string[] data = null;
        StreamReader strReader = new StreamReader(path);

        bool endOfFile = false;
        while (!endOfFile)
        {
            string data_String = strReader.ReadLine();
            if (data_String == null)
            {
                endOfFile = true;
                break;
            }

            data = data_String.Split(';');

            //Llegados a este punto, data es una linea del csv, que esta compuesta por:
            //CASO;ID(O NOMBRE)¨;SPEAKER;TEXTO
            switch (data[0])//em este campo viene indicado el caso de uso al que hace referencia
            {
                case "Case3":
                case "CASE3":
                case "case3":
                    case3LanguageData.Add(new DialogueDataBean(data[1], data[2], data[3])); //id, speaker, text
                    break;
                case "Case5":
                case "CASE5":
                case "case5":
                    case5LanguageData.Add(new DialogueDataBean(data[1], data[2], data[3])); //id, speaker, text
                    break;
                case "Case6":
                case "CASE6":
                case "case6":
                    case6LanguageData.Add(new DialogueDataBean(data[1], data[2], data[3])); //id, speaker, text
                    break;
                case "Case7":
                case "CASE7":
                case "case7":
                    case7LanguageData.Add(new DialogueDataBean(data[1], data[2], data[3])); //id, speaker, text
                    break;
                case "Case9":
                case "CASE9":
                case "case9":
                    case9LanguageData.Add(new DialogueDataBean(data[1], data[2], data[3])); //id, speaker, text
                    break;
                default:
                    Debug.LogError($"El contenido de data[0] no se reconoce: {data[0]}");
                    break;
            }
        }
    }

    public List<DialogueDataBean> GetDatalist()
    {
        switch (UI_Manager.Instance.currentCase)
        {
            case Cases.Case3:
                return case3LanguageData;
            case Cases.Case5:
                return case5LanguageData;
            case Cases.Case6:
                return case6LanguageData;
            case Cases.Case7:
                return case7LanguageData;
            case Cases.Case9:
                return case9LanguageData;
            default:
                return null;
        }
    }

    /*
    public List<DialogueDataBean> SelectedLanguagueFilter()
    {
        switch (languageSelected)
        {
            case "EN":
                break;
            case "ES":
                break;
            case "PT":
                break;
            case "CZ":
                break;
            case "HU":
                break;
        }
    }
    */

}

