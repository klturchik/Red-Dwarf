using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;
using System;

public class GlassUIController : MonoBehaviour {

    private GameObject AlertPanel;
    private GameObject MissionPanel;
    private GameObject DeathPanel;
    public PlayerEvents playerEvents;

    private bool missionAccepted;
    

	// Use this for initialization
	void Awake () {
        AlertPanel = transform.Find("AlertPanel").gameObject;
        MissionPanel = transform.Find("MissionPanel").gameObject;
        DeathPanel = transform.Find("DeathPanel").gameObject;
        ShrinkPanel(AlertPanel);
        ShrinkPanel(MissionPanel);
    }

    private void Update()
    {
        if (MissionPanel.activeInHierarchy && Input.GetButtonDown("Submit") && !missionAccepted)
        {
            AcceptMission();
        }

        if (DeathPanel.activeInHierarchy && Input.GetButtonDown("Submit"))
        {
            DeathPanel.transform.GetComponentInChildren<Button>().onClick.Invoke();
        }
    }

    void OnEnable()
    {
        initListeners(true);
    }

    void OnDisable()
    {
        initListeners(false);
    }


    private void initListeners(bool state)
    {
        if (state)
        {
            playerEvents.OnDie += OnDie;
        }
        else
        {
            playerEvents.OnDie -= OnDie;
        }
    }

    public void GiveMission(string description)
    {
        StartCoroutine(ShowAlert("Incoming Mission!", 5f));
        StartCoroutine(ShowMission(description, 5f));
    }

    private IEnumerator ShowAlert(string alertText, float displayTime)
    {
        AlertPanel.SetActive(true);
        Animator anim = AlertPanel.GetComponent<Animator>();
        Text text = AlertPanel.transform.Find("AlertText").GetComponent<Text>();

        text.text = alertText;
        anim.Play("Expand");
        yield return new WaitForSeconds(displayTime);
        anim.Play("Collapse");
    }

    private IEnumerator ShowMission(string description, float delayTime)
    {
        Animator anim = MissionPanel.GetComponent<Animator>();
        Text text = MissionPanel.transform.Find("MissionDescription").GetComponent<Text>();
        text.text = description;

        yield return new WaitForSeconds(delayTime);
        MissionPanel.SetActive(true);
        GetComponent<VRTK_UICanvas>().enabled = true;
        anim.Play("Expand");
    }

    public void AcceptMission()
    {
        missionAccepted = true;
        //GetComponent<VRTK_UICanvas>().enabled = false;
        Animator anim = MissionPanel.GetComponent<Animator>();
        playerEvents.Spawn();
        anim.Play("Collapse");
    }

    private void ShrinkPanel(GameObject panel)
    {
        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.localScale = Vector3.zero;
    }

    private void OnDie()
    {
        Time.timeScale = 0f;
        DeathPanel.SetActive(true);
    }

}
