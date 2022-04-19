using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_DEBUG
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject escapeMenu;

    void Start()
    {
        escapeMenu.SetActive(false);
    }

    void OnEscape()
    {
        escapeMenu.SetActive(!escapeMenu.activeSelf);
        Time.timeScale = escapeMenu.activeSelf ? 0 : 1;
    }

    public void Exit()
    {
        Application.Quit();
#if UNITY_DEBUG
        EditorApplication.isPaused = true;
#endif
    }
}
