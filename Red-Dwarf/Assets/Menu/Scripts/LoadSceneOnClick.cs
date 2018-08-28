using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadSceneOnClick : MonoBehaviour
{

    public void LoadNewGame(int sceneIndex)
    {
        //Enable to play tracks on level load
        //FindObjectOfType<MusicClass>().PlayMusic("Track1");
        StartCoroutine(LoadSceneDelay(sceneIndex));
    }

    IEnumerator LoadSceneDelay(int sceneIndex)
    {
        yield return new WaitForSeconds(1.5f);
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            GameControl.control.DeleteData();
        }
        GameControl.control.setDefaults();
        GameControl.control.level = sceneIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        Time.timeScale = 1.0f;
        GameControl.control.level = sceneIndex;
    }

    public void LoadSavedLevel()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            SceneManager.LoadScene(GameControl.control.level);
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
        
    }


}
