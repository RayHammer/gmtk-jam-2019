﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    public static bool Paused { get; private set; } = false;

    public static bool UnPaused { get { return !Paused; } }

    [SerializeField]
    GameObject pauseCanvas = null;

    private void Start()
    {
        if (pauseCanvas != null)
        {
            var pause = Instantiate(pauseCanvas);

            Paused = false;
            myTransform = pause.transform;
            SetPause(false);
        }
    }

    public static void ChangeMenuVisibility()
    {
        for (int i = 0; i < myTransform.childCount; i++)
        {
            myTransform.GetChild(i).gameObject.SetActive(Paused);
        }
    }

    public static void SetPause(bool shouldPause, bool openMenu = true)
    {
        Paused = shouldPause;

        if (openMenu) ChangeMenuVisibility();
    }

    public void ResumeGame()
    {
        SetPause(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetPause(!Paused);
        }
    }
    public void GoToMenu()
    {
        SetPause(false);
        SceneManager.LoadScene("MainMenu");
    }

    private static Transform myTransform;
}