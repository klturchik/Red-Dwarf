using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private GameObject player;
    private GlassUIController playerUI;
    private PlayerEvents playerEvents;


    public bool skipIntro = false;
    public Transform spawnPoint;
    public Transform hyperspacePoint;

	// Use this for initialization
	void Start () {
        if (skipIntro)
        {
            playerEvents.Spawn();
        }
        else
        {
            Intro();
        }
    }

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerUI = player.GetComponentInChildren<GlassUIController>();
        playerEvents = player.GetComponent<PlayerEvents>();
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
            playerEvents.OnSpawn += SpawnPlayer;
        }
        else
        {
            playerEvents.OnSpawn -= SpawnPlayer;
        }
    }

    private void Intro()
    {
        ToggleHyperspace(true);
        TogglePlayerControls(false);
        playerUI.GiveMission("Defeat all enemies in this quadrant");
    }

    public void TogglePlayerControls(bool state)
    {
        player.GetComponent<PlayerShipController>().enabled = state;
    }

    public void ToggleHyperspace(bool state)
    {
        ParticleSystem hyperspaceEffect = player.transform.Find("PlayerShip").Find("HyperspaceEffect").GetComponent<ParticleSystem>();
        Animator playerAnim = player.GetComponent<Animator>();

        if (state)
        {
            player.transform.position = hyperspacePoint.position;
            hyperspaceEffect.Play();
        }
        else
        {
            playerAnim.Play("WarpIn");
            hyperspaceEffect.Stop();
        }
    }

    private void SpawnPlayer()
    {
        player.transform.position = spawnPoint.position;
        ToggleHyperspace(false);
        TogglePlayerControls(true);
    }

}
