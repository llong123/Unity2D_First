using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

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
	public float moveSpeed = 5f;
	public float jumpHeight = 250f;
	public float currentHealth;
	public float maxHealth = 5.0f;

	[SerializeField]
	public Image healthBar;

	[SerializeField]
	public Transform[] groundPoints;

	[SerializeField]
	public bool airControl;

	public float radius = 0.01f;
	public LayerMask groundMask;
	public Animator myAnimator;

	public Rigidbody2D myRigidbody { get; set;}

	public bool Attack { get; set;}

	public bool Dash { get; set;} 

	public bool Jump { get; set;} 

	public bool OnGround { get; set;} 

	// Use this for initialization
	void Start () {
	
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

		HandleMovement (horizontal);

		Flip ();

		HandleLayers ();

		if (currentHealth <= 0 || myRigidbody.position.y <= -5) {
			Die();
		}
	}

	//Handle character's movement
	void HandleMovement(float horizontal){

		// If -y => falling is true
		if (myRigidbody.velocity.y < 0) {
			myAnimator.SetBool ("fall", true);
		}

		//
		if(!Attack && !Dash && (OnGround || airControl)){
			myRigidbody.velocity = new Vector2 (horizontal * moveSpeed, myRigidbody.velocity.y);
		}

		if (Jump && (myRigidbody.velocity.y == 0)) {
			myRigidbody.AddForce (new Vector2 (0, jumpHeight));
		}

		myAnimator.SetFloat ("speed", Mathf.Abs (horizontal));

	}

	//Handle Inputs
	void HandleInput(){

		if (Input.GetKeyDown (KeyCode.Z)) {
			myAnimator.SetTrigger ("attack");
		}

		if (Input.GetKeyDown (KeyCode.X)) {
			myAnimator.SetTrigger ("dash");
		}

		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			myAnimator.SetTrigger("jump");
		}
	}



	//Handle Animation Layers
	void HandleLayers(){
		if (!OnGround) {
			myAnimator.SetLayerWeight (1, 1);
		} else {
			myAnimator.SetLayerWeight (1, 0);
		} 
	}

	//Flip Sprites
	void Flip(){
		if (Input.GetAxisRaw ("Horizontal") == 1) {
			transform.localScale = new Vector3 (1, 1, 1);

		} else if (Input.GetAxisRaw ("Horizontal") == -1) {
			transform.localScale = new Vector3 (-1, 1, 1);
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

	//Draw Circle
	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		foreach (Transform point in groundPoints) {
			Gizmos.DrawWireSphere (point.position, radius);
		}

	}

	//GameOver
	void Die(){
		SceneManager.LoadScene("game_over");
	}
}
