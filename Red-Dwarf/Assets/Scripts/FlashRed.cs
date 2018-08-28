using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashRed : MonoBehaviour {

    public PlayerEvents playerEvents;
    public GameObject flashRedPanel;
    

    // Use this for initialization
    void Awake () {
        initListeners(true);
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void initListeners(bool state)
    {
        if (state)
        {
            playerEvents.OnTakeDamage += OnTakeDamage;
        }
        else
        {
            playerEvents.OnTakeDamage -= OnTakeDamage;
        }
    }

    private void OnTakeDamage(float hp)
    {
        StartCoroutine(Flash());
        
    }

    private IEnumerator Flash ()
    {
        flashRedPanel.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        flashRedPanel.SetActive(false);
    }
}
