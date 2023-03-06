using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
	public GameObject prefab;
	public float camMoveSpeed;

	void FixedUpdate()
	{
		Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + camMoveSpeed * Time.deltaTime * Input.GetAxisRaw("Horizontal"), Camera.main.transform.position.y + camMoveSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical"), Camera.main.transform.position.z);

		if (prefab != null)
		{
			if (Input.GetMouseButton(0))
			{
				Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				pos.y = Mathf.Round(pos.y);
				pos.x = Mathf.Round(pos.x);
				RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
				if (hit.collider == null)
				{
					GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
					pos.z = 0;
					obj.transform.position = pos;
					Transform parent = transform.Find(prefab.name);
					if (parent == null)
					{
						parent = new GameObject(prefab.name).transform;
						parent.parent = transform;
					}
					obj.transform.parent = parent;
					Debug.Log("Placed at " + pos);
				}
			}
			else if (Input.GetMouseButton(1))
			{
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if (hit.collider != null)
				{
					Destroy(hit.collider.gameObject);
				}
			}
		}
	}
}
