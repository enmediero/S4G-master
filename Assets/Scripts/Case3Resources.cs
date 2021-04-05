﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Case3Resources : MonoBehaviour
{
    private static Case3Resources instance = null;

    public static Case3Resources Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }

    public GameObject secretary;
    public GameObject nurse;
    public GameObject endoscopist2;
    public GameObject anaesthesiologist;
    public GameObject anaesthesistNurse;
    public GameObject extraNurse;

    public Animation doorAnim;
    public GameObject fakeProp;
    public GameObject tablePoint;
    public GameObject floorPoint;
    public GameObject mask;
    public GameObject animMask;

    public Transform[] waypointsExit;
    public Transform[] waypointsEnter;
    public Transform[] waypointsEnterExtra;
    public Transform[] waypointsSecretary;
    public Transform[] waypointsToTable1;
    public Transform[] waypointsToTable2;
    public Transform tableLookPoint;
    public Transform endoscopist1LookPoint;
    public Dictionary<string, Vector3> positionsDictionary; //en este diccionario guardaremos las posiciones originales de cada objeto en la mesa
    

    public GameObject[] tools; //para que se vea en el inspector :)
    public Dictionary<string, GameObject> toolsDictionary;

    public List<GameObject> ExtraTablePositions;
    

    private void Start()
    {
        InitDictionary();
    }

    public void InitDictionary()
    {
        toolsDictionary = new Dictionary<string, GameObject>();
        positionsDictionary = new Dictionary<string, Vector3>();
        foreach (var item in tools)
        {
            toolsDictionary.Add(item.name, item);
            positionsDictionary.Add(item.name, item.transform.position);
            item.GetComponent<FollowPoint>().target = item.transform;
        }
    }

    public void PutInExtraTable(GameObject prop)
    {

        //Obtener la primera posicion que este libre
        int position = 0;
        bool enc = false;
        for (int i = 0; i < ExtraTablePositions.Count && !enc; i++)
        {
            if (ExtraTablePositions[i].GetComponent<TableSlot>().isSlotFree)
            {
                enc = true;
                position = i;
            }
        }

        SpecialCases.Instance.SetProp(Case3Resources.Instance.ExtraTablePositions[position].transform, prop);

        if (enc)
        {
            ExtraTablePositions[position].GetComponent<TableSlot>().isSlotFree = false;
        }
    }

}
