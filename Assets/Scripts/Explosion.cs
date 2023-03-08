using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("1");
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyAI>().Death();
        }


    }
}
