using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour {

    public float maxHealth = 100f;
    public float score = 0f;

    public Slider healthSlider;
    public Text scoreText;

    public GameObject deathPanel;

    private PlayerEvents playerEvents;
    private float health;

    private void Start()
    {
        health = maxHealth;
    }

    void OnEnable()
    {
        playerEvents = GetComponentInParent<PlayerEvents>();
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
            playerEvents.OnTakeDamage += OnTakeDamage;
            playerEvents.OnAddHealth += OnAddHealth;
            playerEvents.OnDie += OnDie;
            playerEvents.OnAddScore += OnAddScore;
        }
        else
        {
            playerEvents.OnTakeDamage -= OnTakeDamage;
            playerEvents.OnAddHealth -= OnAddHealth;
            playerEvents.OnDie -= OnDie;
            playerEvents.OnAddScore -= OnAddScore;
        }
    }

    private void OnTakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            playerEvents.Die();
        }
        healthSlider.value = health;
    }

    private void OnDie()
    {
        
    }

    private void OnAddHealth(float amount)
    {
        health += amount;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        healthSlider.value = health;
    }

    private void OnAddScore(float amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HealthPack")
        {
            playerEvents.AddHealth(other.gameObject.GetComponent<Healthpack>().healthAmount);
            Destroy(other.gameObject);
        }
    }

}
