using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimator : MonoBehaviour
{
	public float frameDelay, startDelay, loopDelay;
	public bool loop, randomiseDelays;
	public Sprite[] sprites;
	Vector2 drawSize;
	SpriteRenderer spriteRenderer;

	IEnumerator Start()
	{
		if (sprites != null && sprites.Length > 0)
		{
			if (randomiseDelays)
			{
				startDelay = Random.Range(0, .5f);
				loopDelay = Random.Range(0, 2f);
			}

			spriteRenderer = GetComponent<SpriteRenderer>();
			drawSize = spriteRenderer.size;
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

	public void SetSprites(params Sprite[] sprites)
	{
		this.sprites = sprites;
		spriteRenderer.sprite = sprites.Length > 0 ? sprites[0] : null;
		spriteRenderer.size = drawSize;
		if (sprites.Length == 1) loop = false;
	}
}
