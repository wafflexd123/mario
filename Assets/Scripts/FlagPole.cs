using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
	public Transform bottom, flag, drop, exitDoor, fireworks;
	public float flagSlideSpeed, marioSlideSpeed;
	bool finishedSliding = false, hasStarted = false;

	IEnumerator FlagSlide()
	{
		fireworks.gameObject.SetActive(true);
		while (flag.position != bottom.position)
		{
			flag.position = Vector2.MoveTowards(flag.position, bottom.position, Time.deltaTime * flagSlideSpeed);
			yield return null;
		}
		finishedSliding = true;
	}

	IEnumerator MarioSlide(Mario mario, float speed)
	{
		yield return new WaitUntil(() => mario.Move(bottom, speed));
		yield return new WaitUntil(() => finishedSliding);
		yield return new WaitUntil(() => mario.Move(drop, speed));
		yield return new WaitUntil(() => mario.Move(exitDoor, speed));
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!hasStarted)
		{
			hasStarted = true;
			if (collision.gameObject.CompareTag("Player"))
			{
				Mario mario = collision.gameObject.GetComponent<Mario>();
				mario.EnableMovement(false);
				StartCoroutine(MarioSlide(mario, marioSlideSpeed));
				StartCoroutine(FlagSlide());
			}
		}
	}
}
