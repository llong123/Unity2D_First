using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour {



	[SerializeField]
	protected Transform ballPos;

	protected bool facingRight;

	[SerializeField]
	protected float moveSpeed;

	[SerializeField]
	protected GameObject ballPrefab;

	public bool Attack { get; set;}
	public Animator MyAnimator{ get; private set; }

	// Use this for initialization
	public virtual void Start () {
		facingRight = true;
		MyAnimator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeDirection(){
		facingRight = !facingRight;
		transform.localScale = new Vector3 (transform.localScale.x * -1, 1, 1);
	}

	public virtual void ShootBall(int value){
		if (facingRight) {
			GameObject tmp = (GameObject) Instantiate (ballPrefab, ballPos.position, Quaternion.Euler(new Vector3(0,0,-90)));
			tmp.GetComponent<ShootBall> ().Intialize (Vector2.right);

		} else {
			GameObject tmp = (GameObject) Instantiate (ballPrefab, ballPos.position, Quaternion.Euler(new Vector3(0,0,90)));
			tmp.GetComponent<ShootBall> ().Intialize (Vector2.left);
		}
	}
}
