using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
	public enum MarioState
	{
		Small, Super, Fiery, Star
	}
	public MarioState currentState = MarioState.Small;
	private MarioState previousState;

	public int health = 1, lives = 3;
	[SerializeField] float maxWalkSpeed = 25f, sprintSpeed = 0.01f, horizontalAcc = 15f;
	[SerializeField] float speed = 0f, maxSpeed, maxSprintSpeed, jumpSpeedMin = 4f, jumpSpeedMax = 9.5f, starTimer = 0f;
	private const float starDuration = 30f;
	bool isJumping = false, jumpCancel = false, isGrounded = true, isInvincible = false, isSprinting = false, canMove = true;

	Coroutine crtStar;
	Rigidbody2D body;

	public int Coins { get => _coins; set { _coins = value; UIController.singleton.txtCoins.text = $"COINS\n{value}"; } }
	int _coins;

	void Start()
	{
		body = GetComponent<Rigidbody2D>();
		maxSpeed = maxWalkSpeed;
		maxSprintSpeed = maxWalkSpeed * 1.6f;
	}

	void Update()
	{
		if (canMove)
		{
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				speed += sprintSpeed * Mathf.Sign(speed);
				maxSpeed = maxSprintSpeed;
				isSprinting = true;
			}

			if (Input.GetKeyUp(KeyCode.LeftShift))
			{
				maxSpeed = maxWalkSpeed;
				if (speed > maxSpeed) speed = maxSpeed;
			}

			if (Input.GetButtonDown("Jump") && Mathf.Abs(body.velocity.y) < 0.01f /*&& isGrounded*/)//isGrounded wasn't reliably working, have to fix
			{
				isJumping = true;
			}

			if (Input.GetButtonUp("Jump") && !isGrounded)
			{
				jumpCancel = true;
			}
		}
	}

	void Movement()
	{
		if (canMove)
		{
			float direction = Input.GetAxisRaw("Horizontal");
			if (direction != 0)
			{
				if (speed * direction < maxSpeed)
				{
					speed += horizontalAcc * direction * Time.fixedDeltaTime;
					if (speed * direction > maxSpeed) speed = maxSpeed * direction;
				}
			}
			else if (speed != 0)
			{
				float sign = Mathf.Sign(speed);
				speed -= horizontalAcc * sign * Time.fixedDeltaTime;
				if (speed * sign < 0) speed = 0;
			}
			body.velocity = new Vector2(speed, body.velocity.y);
		}
		//if (Input.GetKey("left") && speed > -maxSpeed)
		//{
		//	speed -= horizontalAcc;
		//}

		//else if (speed < 0)
		//{
		//	speed += horizontalAcc;

		//	if (speed < 0.002 && speed > -0.002)
		//	{
		//		speed = 0;
		//	}
		//}

		//if (Input.GetKey("right") && speed < maxSpeed)
		//{
		//	speed += horizontalAcc;
		//}

		//else if (speed > 0)
		//{
		//	speed -= horizontalAcc;

		//	if (speed < 0.002 && speed > -0.002)
		//	{
		//		speed = 0;
		//	}
		//}
		//if (speed > maxSpeed)
		//{
		//	speed = maxSpeed;
		//}

		//transform.position = new Vector2(transform.position.x + speed, transform.position.y);
	}

	//speed kept increasing if the player was walking into a wall
	//character speed varied depending on framerate
	//character would get stuck on the edges of colliders

	private void FixedUpdate()
	{
		Movement();

		if (isJumping)
		{
			body.velocity = new Vector2(body.velocity.x, jumpSpeedMax);
			isJumping = false;
		}

		if (jumpCancel)
		{
			if (body.velocity.y > jumpSpeedMin)
			{
				body.velocity = new Vector2(body.velocity.x, jumpSpeedMin);
			}
			jumpCancel = false;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.gameObject.layer == 3)//ground layer
			isGrounded = true;
		else if (collision.gameObject.CompareTag("Weak Spot")) //enemy head collider
		{
			isJumping = true;
			jumpCancel = true; //triggers a short jump

			collision.gameObject.GetComponentInParent<EnemyAI>().Death(); //activates death on collided enemy
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.gameObject.layer == 3)//ground layer
			isGrounded = false;
	}

	public void EnableMovement(bool enable)
	{
		if (enable)
		{
			body.isKinematic = false;
			canMove = true;
		}
		else
		{
			body.velocity = Vector2.zero;
			body.isKinematic = true;
			canMove = false;
		}
	}

	public bool Move(Transform pos, float speed)
	{
		if (Vector2.Distance(transform.position, pos.position) > 0.01f)
		{
			transform.position = Vector2.MoveTowards(transform.position, pos.position, Time.deltaTime * speed);
			return false;
		}
		else return true;
	}

	// Power up handling
	// Activates when Mario picks up a Super Mushroom.
	public void Grow()
	{
		currentState = MarioState.Super;
		// Temporary grow code, makes Mario size bigger, will change sprite later.
		transform.localScale = new Vector3(1, 2, 1);
		health = 2;
	}

	// Activates when Mario picks up a 1-Up Mushroom.
	public void GainLife()
	{
		lives += 1;
	}

	// Activates when Mario picks up a Fire Flower.
	public void ShootFireballs()
	{
		currentState = MarioState.Fiery;
	}

	// Activates when Mario picks up a Super Star.
	public void BecomeInvincible()
	{
		previousState = currentState;
		currentState = MarioState.Star;
		isInvincible = true;
		if (crtStar != null) StopCoroutine(crtStar);//coroutine so we don't have to check every frame
		crtStar = StartCoroutine(StarTimer());
		IEnumerator StarTimer()
		{
			yield return new WaitForSeconds(starDuration);
			isInvincible = false;
			currentState = previousState;
			crtStar = null;
		}
	}
}
