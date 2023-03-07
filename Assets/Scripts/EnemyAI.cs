using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public bool dead;
    [SerializeField] Sprite deathSprite;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        GetComponent<SimpleAnimator>().sprites[0] = deathSprite;
        GetComponent<SimpleAnimator>().sprites[1] = deathSprite;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        yield return null;
    }
}
