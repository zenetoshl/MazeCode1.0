﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Scene Transition
public class MazeToCode : Interactable
{
    protected JoyButton joybutton;
    [Header("New Scene Variables")]
    public string sceneToLoad;

    [Header("Transition Variables")]
    public GameObject fadeInPanel;
    public GameObject fadeOutPanel;
    public float fadeWait;

    public void Awake()
    {
        if(fadeInPanel != null)
        {
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity) as GameObject;
            Destroy(panel, 1); 
        }
    }
    private void Start()
    {
        joybutton = FindObjectOfType<JoyButton>();
    }
    public void Update()
    {
        if(joybutton.Pressed && playerInRange)
        {
            //SceneManager.LoadScene(sceneToLoad);
            StartCoroutine(FadeControl());
        }
    }

    public IEnumerator FadeControl()
    {
        if(fadeOutPanel != null)
        {
            Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(fadeWait);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        while(!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}

