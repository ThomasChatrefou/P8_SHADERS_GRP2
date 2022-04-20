using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
#if UNITY_DEBUG
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject escapeMenu;
    [SerializeField] TextMeshProUGUI textDisplayer;

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
    }

    private void Update()
    {
    }

    public void OnEscape()
    {
        Debug.Log("escape");
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

    public void DisplayMessageAndReset(string msg)
    {
        textDisplayer.text = msg;
        textDisplayer.gameObject.SetActive(true);
        StartCoroutine(HideTextAfterTwoSecondAndResetScene());
    }

    IEnumerator HideTextAfterTwoSecondAndResetScene()
    {
        yield return new WaitForSeconds(2);
        textDisplayer.gameObject.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        instance = null;
    }
}
