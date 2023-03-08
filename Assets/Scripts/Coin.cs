using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
	bool interacted;
    GameObject ui;
    UIController uiController;

    private void Start()
	{
        ui = GameObject.FindGameObjectWithTag("UIController");
        uiController = ui.GetComponent<UIController>();
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!interacted)
		{
			if (collision.CompareTag("Player"))
			{
				collision.gameObject.GetComponent<Mario>().Coins++;
				uiController.score += 200;
				interacted = true;
				StartCoroutine(PlaySound());
			}
		}
	}

	IEnumerator PlaySound()
	{
		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();
		GetComponent<Renderer>().enabled = false;
		yield return new WaitWhile(() => audio.isPlaying);
		Destroy(gameObject);
	}
}
