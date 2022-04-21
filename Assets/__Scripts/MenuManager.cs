using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
#if UNITY_DEBUG
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject escapeMenu;
    [SerializeField] TextMeshProUGUI textDisplayer;
    [SerializeField] TextMeshProUGUI counterDisplayer;

    public static MenuManager instance;

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        escapeMenu.SetActive(false);
        textDisplayer.gameObject.SetActive(false);
        Cursor.visible = false;
    }

    public void OnEscape()
    {
        escapeMenu.SetActive(!escapeMenu.activeSelf);
        Time.timeScale = escapeMenu.activeSelf ? 0 : 1;
        //Cursor.lockState = escapeMenu.activeSelf ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = escapeMenu.activeSelf;
    }

    public void Exit()
    {
        Application.Quit();
#if UNITY_DEBUG
        EditorApplication.isPaused = true;
#endif
    }

    public void DisplayMessageAndReset(string msg)
    {

        textDisplayer.text = msg;
        textDisplayer.gameObject.SetActive(true);
        GameManager.instance.Pause();
        StartCoroutine(HideTextAfterTwoSecondAndResetScene());
    }

    public void UpdateCounter(string str)
    {
        counterDisplayer.text = str;
    }

    IEnumerator HideTextAfterTwoSecondAndResetScene()
    {
        yield return new WaitForSeconds(2);
        textDisplayer.gameObject.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.instance.Resume();
    }

    void OnDestroy()
    {
        instance = null;
    }
}
