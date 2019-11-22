﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class RelodScene : MonoBehaviour
{
    [SerializeField]
    protected string NextSceneName = "";
    [SerializeField]
    protected int SceneNumber = 0;

    public bool isPointVictory = false;
    public int pointsToVictory;
    // How much monsters should be spawned after limit is exceeded (not exactly, waves are not cut)
    public int monsterAdditionLimit = 12;
    public static bool isVictory = false;
    public int TotalValue = 0;
    private float maxvalue = 0;

    protected static GameObject Canvas;

    protected virtual void Awake()
    {
        statSender = GetComponent<StatSender>();
        ArenaEnemySpawner spawn = GetComponent<ArenaEnemySpawner>();
        CharacterLife.isDeath = false;
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
        var arena = GetComponent<ArenaEnemySpawner>();
        maxvalue = arena.EnemyCount();

        Canvas.transform.GetChild(0).gameObject.SetActive(false);
        isVictory = false;
        ContinueButtonPressed = false;
        RestartButtonPressed = false;
        PlayerPrefs.SetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex);
    }

    public void CurrentCount(int val)
    {
        TotalValue = TotalValue + val;
    }

    private void Update()
    {
        Victory();
        Reload();
    }

    protected virtual void ProcessVictory()
    {
        CurrentEnemy.SetCurrentEnemyName(" ");
        isVictory = true;
        Canvas.transform.GetChild(0).gameObject.SetActive(true);
        if (ContinueButtonPressed || Input.GetKeyDown(KeyCode.F) && !CharacterLife.isDeath)
        {
            if (statSender == null || statSender.Finished())
            {
                Canvas.transform.GetChild(0).gameObject.SetActive(false);
                SceneManager.LoadScene(NextSceneName);
            }
            ContinueButtonPressed = true;
        }
    }

    protected virtual void Victory()
    {
        if (isPointVictory)
        {
            if (TotalValue >= pointsToVictory)
            {
                ProcessVictory();
            }
        }
        else
        {
            if (TotalValue >= maxvalue)
            {
                ProcessVictory();
            }
        }
    }

    protected virtual void Reload()
    {
        if (RestartButtonPressed || Input.GetKeyDown(KeyCode.R) && (!isVictory || CharacterLife.isDeath))
        {
            if (statSender == null || statSender.Finished())
            {
                TotalValue = 0;
                Time.timeScale = 1;
                Canvas.transform.GetChild(1).gameObject.SetActive(false);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            RestartButtonPressed = true;
        }
    }

    public static void PressR()
    {
        if (isVictory && !CharacterLife.isDeath) return;
        Canvas.transform.GetChild(1).gameObject.SetActive(true);
    }

    private NetRequestSender statSender;
    private bool ContinueButtonPressed;
    private bool RestartButtonPressed;
}

/*[CustomEditor(typeof(RelodScene))]
public class MyEditorClass : Editor
{
    public override void OnInspectorGUI()
    {
        // If we call base the default inspector will get drawn too.
        // Remove this line if you don't want that to happen.
        //base.OnInspectorGUI();

        RelodScene myReload = target as RelodScene;

        myReload.NextSceneName = EditorGUILayout.TextField("NextLevel", myReload.NextSceneName);
        myReload.SceneNumber = EditorGUILayout.IntField("Scene Number", myReload.SceneNumber);
        myReload.isPointVictory = EditorGUILayout.Toggle("isPointVictory", myReload.isPointVictory);

        if (myReload.isPointVictory)
        {
            myReload.pointsToVictory = EditorGUILayout.IntField("Points to victory:", myReload.pointsToVictory);

        }
    }
}*/
