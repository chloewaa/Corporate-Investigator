using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuHelpManager : MonoBehaviour
{
    [Header("Help Canvas")]
    public GameObject helpCanvas;

    [Header("Menu Canvas")]
    public GameObject menuCanvas;

    [Header("Menu Scene")]
        [Header("Menu Scene Settings")]
    [Tooltip("Name of the main menu scene to load")] 
    public string menuSceneName = "MainMenu";

    [Header("Fade Transition Settings")]
    [Tooltip("CanvasGroup covering the screen for fade transitions")]
    public CanvasGroup fadeCanvasGroup;
    [Tooltip("Duration of the fade in seconds")]
    public float fadeDuration = 0.5f;

    public void GoToMenu() {
        if (fadeCanvasGroup != null) {
            StartCoroutine(TransitionAndLoad(menuSceneName));
        } else {
            //Fallback to immediate load if no fade
            SceneManager.LoadScene(menuSceneName);
        }
    }

    public void ExitGame() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    private IEnumerator TransitionAndLoad(string sceneName) {
        //Fade to black
        yield return StartCoroutine(Fade(1));
        //Load the new scene
        yield return SceneManager.LoadSceneAsync(sceneName);
        //Fade back in
        yield return StartCoroutine(Fade(0));
    }

    private IEnumerator Fade(float targetAlpha) {
        float startAlpha = fadeCanvasGroup.alpha;
        float time = 0f;
        while (time < fadeDuration) {
            time += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = targetAlpha;
    }


    public void OnMenuButton() {
        menuCanvas.SetActive(true);
        //if help canvas is open at the same time, close it
        helpCanvas.SetActive(false);
    }

    public void OnHelpButton() {
        helpCanvas.SetActive(true);
        //if menu canvas is open at the same time, close it
        menuCanvas.SetActive(false);
    }

    public void OnExit() {
        helpCanvas.SetActive(false);
        menuCanvas.SetActive(false);
    }

}
