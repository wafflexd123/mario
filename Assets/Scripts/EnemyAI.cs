using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
   // public bool dead;
    [SerializeField] Sprite deathSprite;

    public void Death() //changes animation sprites to death sprite, then disappears
    {
        StartCoroutine(Routine());//dont need to check every frame in Update(), can just call this immediately
        IEnumerator Routine()
        {
            GetComponent<SimpleAnimator>().sprites = new Sprite[] { deathSprite };
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }
}
