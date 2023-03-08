using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	// public bool dead;
	[SerializeField] Sprite deathSprite;
    GameObject ui;
    UIController uiController;

	private void Start()
	{
        ui = GameObject.FindGameObjectWithTag("UIController");
        uiController = ui.GetComponent<UIController>();
    }

	public void Death() //changes animation sprites to death sprite, then disappears
	{
		StartCoroutine(Routine());//dont need to check every frame in Update(), can just call this immediately
		IEnumerator Routine()
		{
			GetComponent<SimpleAnimator>().SetSprites(deathSprite);
			uiController.score += 100;
			yield return new WaitForSeconds(0.5f);
			Destroy(gameObject);
		}
	}
}
