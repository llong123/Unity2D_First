using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody2D))]
public class ShootBall : MonoBehaviour {

	[SerializeField]
	private float speed;

	private Rigidbody2D myRigidbody;

	private Vector2 direction;

	// Use this for initialization
	void Start () {
	
		myRigidbody = GetComponent<Rigidbody2D> ();
		

	}
	
	// Update is called once per frame
	void Update(){
		
	}

	void FixedUpdate () {
		
		myRigidbody.velocity = direction * speed;

	}

	public void Intialize(Vector2 direction){
		this.direction = direction;
	}

	void OnBecameInvisible(){
		Destroy (gameObject);
	}
}
