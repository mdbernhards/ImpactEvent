using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject loadingPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        mainMenuPanel.SetActive(false);
        loadingPanel.SetActive(true);

        yield return new WaitForSeconds(1f);

        AsyncOperation operation =
            SceneManager.LoadSceneAsync("Game");

        while (!operation.isDone)
        {
            yield return null;
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
		        Application.Quit();
        #endif
    }
}