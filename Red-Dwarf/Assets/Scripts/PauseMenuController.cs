using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;

public class PauseMenuController : MonoBehaviour {

    public GameObject PauseMenuCanvas;

    private GameObject PauseMenuPanel;

    // Use this for initialization
    void Start () {
        PauseMenuPanel = PauseMenuCanvas.transform.Find("PauseMenuPanel").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        // if (Vive menu button is pressed and controller is pointed at shipStatusCanvas)
        GameObject leftController = VRTK_DeviceFinder.GetControllerLeftHand();
        GameObject rightController = VRTK_DeviceFinder.GetControllerRightHand();

        if ((leftController != null && CheckPointAndMenu(leftController)) || (rightController != null && CheckPointAndMenu(rightController)))
        {
            GamePause();
        }
	}

    public void GamePause()
    {
        PauseMenuPanel.SetActive(true);
          //  Animator anim = PauseMenuPanel.GetComponent<Animator>();
          //  anim.Play("Expand");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public bool CheckPointAndMenu(GameObject controller)
    {
        VRTK_ControllerEvents events = controller.GetComponent<VRTK_ControllerEvents>();
        VRTK_BasePointerRenderer pointer = controller.GetComponent<VRTK_BasePointerRenderer>();

        if (events.menuPressed && pointer.GetDestinationHit().collider != null && pointer.GetDestinationHit().collider.gameObject == gameObject)
        {
            return true;
        }

        return false;
        
    }

    public void ReturnButtonPress()
    {
        Animator anim = PauseMenuPanel.GetComponent<Animator>();
        Time.timeScale = 1;
        //anim.Play("Collapse");
        PauseMenuPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RestartButtonPress()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenuButtonPressed()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void QuitButtonPressed()
    {
        Time.timeScale = 1;
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
