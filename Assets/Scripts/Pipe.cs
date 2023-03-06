using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
	public float warpSpeed;
	public string inputAxis;
	public int inputAxisDirection;
	public Transform entranceInterior, exitInterior, exit, cameraPos;
	Coroutine crtMove;

	private void OnTriggerStay2D(Collider2D other)
	{
		if (crtMove == null)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				if (Input.GetAxisRaw(inputAxis) == inputAxisDirection)
				{
					crtMove = StartCoroutine(MoveMario(other.gameObject.GetComponent<Mario>()));
				}
			}
		}
	}

	IEnumerator MoveMario(Mario mario)
	{
		mario.EnableMovement(false);
		yield return new WaitUntil(() => mario.Move(entranceInterior, warpSpeed));
		Camera.main.transform.position = cameraPos.position;
		if (exitInterior != null)
		{
			mario.transform.position = exitInterior.position;
			yield return new WaitUntil(() => mario.Move(exit, warpSpeed));
		}
		else
		{
			mario.transform.position = exit.position;
		}
		mario.EnableMovement(true);
		crtMove = null;
	}
}
