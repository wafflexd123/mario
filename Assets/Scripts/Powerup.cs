using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Powerup;

public class Powerup : MonoBehaviour
{
    // Enum defines all possible powerup types to distinguish function.
    public enum PowerupType
    {
        SuperMushroom,
        FireFlower,
        SuperStar,
        LifeMushroom
    }

    public PowerupType type;

    //Change if too fast or slow.
    public float moveSpeed = 3f;
    public float maxHeight = 10f;
    public float bounceSpeed = 5f;

    private bool isBouncing = false;

    private GameObject player;

    private void Update()
    {
        PowerupMovement();
    }

    void PowerupMovement()
    {
        switch (type)
        {
            // Super Mushroom and Life Mushroom should move to the right and reverse direction upon hitting walls.
            case PowerupType.SuperMushroom:
            case PowerupType.LifeMushroom:
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                break;
            // Super Star should move to the right and bounce.
            case PowerupType.SuperStar:
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                // If the star is not bouncing, start bouncing
                if (!isBouncing)
                {
                    isBouncing = true;
                    StartCoroutine(Bounce());
                }
                break;
            default:
                break;
        }
    }

    // Activate method will activate the code within the Mario script that handles how it changes Mario based on which powerup has been activated.
    public void Activate(Mario mario)
    {
        switch (type)
        {
            // Super Mushroom should change sprite to big Mario and give Mario an extra point of health.
            case PowerupType.SuperMushroom:
                mario.Grow();
                break;
            // Fire Flower should change sprite to fiery Mario and give Mario 2 extra points of health and the ability to shoot fireballs.
            case PowerupType.FireFlower:
                mario.ShootFireballs();
                break;
            // Super Star should make Mario flash and become invincible to all damage for 30 seconds
            case PowerupType.SuperStar:
                mario.BecomeInvincible();
                break;
            // Life Mushroom should give Mario an extra life.
            case PowerupType.LifeMushroom:
                mario.GainLife();
                break;
            default:
                break;
        }
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Debug.Log("Collided with Mario");
            Mario mario = collision.gameObject.GetComponent<Mario>();
            Activate(mario);
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            moveSpeed = -moveSpeed;
        }
    }

    IEnumerator Bounce()
    {
        // Move the star up to the maximum height
        while (transform.position.y < maxHeight)
        {
            transform.Translate(Vector3.up * bounceSpeed * Time.deltaTime);
            yield return null;
        }

        // Move the star down to the floor
        while (transform.position.y > 2.5f)
        {
            transform.Translate(Vector3.down * bounceSpeed * Time.deltaTime);
            yield return null;
        }

        // Stop bouncing
        isBouncing = false;
    }
}
