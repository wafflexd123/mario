using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimator : MonoBehaviour
{
	public float frameDelay, startDelay, loopDelay;
	public bool loop, randomiseDelays;
	public Sprite[] sprites;

	IEnumerator Start()
	{
		if (sprites != null && sprites.Length > 0)
		{
			if (randomiseDelays)
			{
				startDelay = Random.Range(0, .5f);
				loopDelay = Random.Range(0, 2f);
			}

			SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
			Vector2 drawSize = spriteRenderer.size;
			if (startDelay > 0) yield return new WaitForSeconds(startDelay);
			do
			{
				for (int i = 0; i < sprites.Length; i++)
				{
					spriteRenderer.sprite = sprites[i];
					spriteRenderer.size = drawSize;
					yield return new WaitForSeconds(frameDelay);
				}
				if (loopDelay > 0) yield return new WaitForSeconds(loopDelay);
			} while (loop);
		}
	}
}
