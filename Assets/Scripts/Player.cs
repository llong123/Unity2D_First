using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : Character {

	private static Player instance;

	public static Player Instance{
		get{ 

			if (instance == null) {
				instance = GameObject.FindObjectOfType<Player> ();
			}

			return instance;
		}
	}


	[SerializeField]
	private float jumpHeight = 250f;


	[SerializeField]
	public Image healthBar;
	private float currentHealth;
	private float maxHealth = 5.0f;

	[SerializeField]
	public Transform[] groundPoints;

	[SerializeField]
	public Transform[] headPoints;

	[SerializeField]
	public bool airControl;

	public float radius = 0.01f;
	public LayerMask groundMask;

	public Rigidbody2D myRigidbody { get; set;}



	public bool Dash { get; set;} 

	public bool Jump { get; set;} 

	public bool OnGround { get; set;} 

	public bool HeadTouch { get; set;} 

	// Use this for initialization
	public override void Start () {
	
		base.Start ();
		myRigidbody = GetComponent<Rigidbody2D> ();
		currentHealth = maxHealth;
	}

	void Update(){
		
		HandleInput ();
	}

	// Update is called once per frame
	void FixedUpdate () {

		float horizontal = Input.GetAxis ("Horizontal");

		healthBar.fillAmount = currentHealth / maxHealth;

		OnGround = IsGrounded();

		HeadTouch = CollidedHead ();

		HandleMovement (horizontal);

		Flip (horizontal);

		HandleLayers ();

		if (currentHealth <= 0 || myRigidbody.position.y <= -5) {
			Die();
		}
	}

	//Handle character's movement
	void HandleMovement(float horizontal){

		// If -y => falling is true
		if (myRigidbody.velocity.y < 0) {
			MyAnimator.SetBool ("fall", true);
		}

		//
		if (!Attack && !Dash && (OnGround || airControl)) {
			myRigidbody.velocity = new Vector2 (horizontal * moveSpeed, myRigidbody.velocity.y);
		}
		//added
		else if (!Attack && Dash && (OnGround || airControl)) {
			myRigidbody.velocity = new Vector2 (horizontal * moveSpeed * 1.5f, myRigidbody.velocity.y);
		}

		if (Jump && (myRigidbody.velocity.y == 0)) {
			myRigidbody.AddForce (new Vector2 (0, jumpHeight));
		}

		MyAnimator.SetFloat ("speed", Mathf.Abs (horizontal));

	}

	//Handle Inputs
	void HandleInput(){

		if (Input.GetKeyDown (KeyCode.Z)) {
			MyAnimator.SetTrigger ("attack");
		}

		//added
		if (myRigidbody.velocity.x != 0 && OnGround && Input.GetKeyDown (KeyCode.X)) {
			MyAnimator.SetTrigger ("dash");
		}

		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			MyAnimator.SetTrigger("jump");
		}

		if(Input.GetKeyDown(KeyCode.C)){
			MyAnimator.SetTrigger ("shoot");
		}
	}



	//Handle Animation Layers
	void HandleLayers(){
		if (!OnGround) {
			MyAnimator.SetLayerWeight (1, 1);
		} else {
			MyAnimator.SetLayerWeight (1, 0);
		} 
	}

	//Flip Sprites
	void Flip(float horizontal){
		if(horizontal > 0 && !facingRight || horizontal < 0 && facingRight){
			ChangeDirection ();
		}
	}

	bool IsGrounded(){
		if (myRigidbody.velocity.y <= 0) {
			foreach (Transform point in groundPoints) {

				Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, radius, groundMask);

				for (int i = 0; i < colliders.Length; i++) {
					if (colliders [i].gameObject != gameObject) {
						return true;
					}
				}
			}
		}
		return false;
	}

	bool CollidedHead(){
		if(myRigidbody.velocity.y > 0){
			foreach (Transform point in headPoints) {
			
				Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, radius, groundMask);

				for (int i = 0; i < colliders.Length; i++) {
					if (colliders [i].gameObject != gameObject) {
						return true;
					}
				}

			}
		}

		return false;
	}

	//Draw Circle
	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		foreach (Transform point in groundPoints) {
			Gizmos.DrawWireSphere (point.position, radius);
		}

		foreach (Transform piste in headPoints) {
			Gizmos.DrawWireSphere (piste.position, radius);
		}
	}

	public override void ShootBall(int value){

		if(!OnGround && value == 1 || OnGround && value == 0){
			base.ShootBall (value);
		}
	}

	//GameOver
	void Die(){
		SceneManager.LoadScene("game_over");
	}
}
