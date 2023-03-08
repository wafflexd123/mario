using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBlock : MonoBehaviour
{
	public Sprite interactedSprite;
	public GameObject spawnOnInteract;
	public Vector3 bounce;
	public float bounceSpeed;
	public bool allowMulipleInteractions;
	Coroutine crtBounce;
	Vector3 startPos;
	public GameObject explosionPrefab ;
	private void Start()
	{
		startPos = transform.parent.position;
        explosionPrefab = Resources.Load<GameObject>("Explosion");
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Player"))
		{
			if (crtBounce == null) crtBounce = StartCoroutine(Bounce());
		}
	}

	IEnumerator Bounce()
	{
		if (spawnOnInteract != null) Instantiate(spawnOnInteract, transform.position, Quaternion.identity);

		if (interactedSprite != null) transform.parent.GetComponent<SimpleAnimator>().SetSprites(interactedSprite);
		Vector3 bouncePos = transform.parent.position + bounce;

		Instantiate(explosionPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + Vector3.up.y, gameObject.transform.position.z), gameObject.transform.rotation);

		while (Vector2.Distance(transform.parent.position, bouncePos) > 0.001f)
		{
			transform.parent.position = Vector2.MoveTowards(transform.parent.position, bouncePos, Time.deltaTime * bounceSpeed);
			yield return null;
		}
		while (Vector2.Distance(transform.parent.position, startPos) > 0.001f)
		{
			transform.parent.position = Vector2.MoveTowards(transform.parent.position, startPos, Time.deltaTime * bounceSpeed);
			yield return null;
		}
		crtBounce = null;
		if (!allowMulipleInteractions) Destroy(this);//save memory
	}
}
