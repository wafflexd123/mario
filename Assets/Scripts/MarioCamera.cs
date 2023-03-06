using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioCamera : MonoBehaviour
{
	public float finalXPos;
	public Mario mario;

	private void Update()
	{
		if (mario.transform.position.x > transform.position.x && transform.position.x < finalXPos)
		{
			transform.position = new Vector3(mario.transform.position.x > finalXPos ? finalXPos : mario.transform.position.x, transform.position.y, transform.position.z);
		}
	}
}
