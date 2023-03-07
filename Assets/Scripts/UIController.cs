using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public float time;
	public Text txtCoins, txtTimer, txtScore, txtLives;
	public static UIController singleton;
	float timer;

	private void Awake()
	{
		singleton = this;
		timer = time;
	}

	private void Update()
	{
		timer -= Time.deltaTime;
		txtTimer.text = $"TIME\n{timer:0}";
	}
}
