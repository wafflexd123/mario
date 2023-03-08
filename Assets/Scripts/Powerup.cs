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

    public float maxVelocity = 5f;

    private Rigidbody2D rb;

    private bool isBouncing = false;

    private GameObject player;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        PowerupMovement();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y > maxVelocity)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxVelocity);
        }
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
                StartCoroutine(PlayPowerUp());
                break;
            // Fire Flower should change sprite to fiery Mario and give Mario 2 extra points of health and the ability to shoot fireballs.
            case PowerupType.FireFlower:
                mario.ShootFireballs();
                StartCoroutine(PlayPowerUp());
                break;
            // Super Star should make Mario flash and become invincible to all damage for 30 seconds
            case PowerupType.SuperStar:
                mario.BecomeInvincible();
                StartCoroutine(PlayPowerStar());
                break;
            // Life Mushroom should give Mario an extra life.
            case PowerupType.LifeMushroom:
                mario.GainLife();
                StartCoroutine(PlayLifeUp());
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

    IEnumerator PlayPowerUp()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        GetComponent<Renderer>().enabled = false;
        yield return new WaitWhile(() => audio.isPlaying);
        Destroy(gameObject);
    }

    IEnumerator PlayPowerStar()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        GetComponent<Renderer>().enabled = false;
        yield return new WaitWhile(() => audio.isPlaying);
        Destroy(gameObject);
    }

    IEnumerator PlayLifeUp()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        GetComponent<Renderer>().enabled = false;
        yield return new WaitWhile(() => audio.isPlaying);
        Destroy(gameObject);
    }
}
