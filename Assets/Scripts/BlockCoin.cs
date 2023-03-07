using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
	public float jumpHeight, jumpSpeed;

    IEnumerator Start()
	{
		Mario.singleton.Coins++;
		Vector2 bouncePos = transform.position + new Vector3(0, jumpHeight, 0);
		Vector2 origin = transform.position;
		while (Vector2.Distance(transform.position, bouncePos) > 0.001f)
		{
			transform.position = Vector2.MoveTowards(transform.position, bouncePos, Time.deltaTime * jumpSpeed);
			yield return null;
		}
		while (Vector2.Distance(transform.position, origin) > 0.001f)
		{
			transform.position = Vector2.MoveTowards(transform.position, origin, Time.deltaTime * jumpSpeed);
			yield return null;
		}
		Destroy(gameObject);
	}
}
