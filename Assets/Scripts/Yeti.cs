using UnityEngine;
using System.Collections;

public class Yeti : MonoBehaviour {

	public float JumpSpeed;
	private Rigidbody2D RigidBody;
	private bool OnGround;
	private BoxCollider2D BoxCollider;
	public float maxSpeed = 10f;
	private Animator Animation;
	public Camera Camera;
	private bool Stopped;
	private bool JumpSnow;
	private ParticleSystem SnowTrail;

	public float CameraFollowUpperPercentage;
	public float CameraFollowLowerPercentage; 

	// Use this for initialization
	void Start () {

		Animation = GetComponent<Animator> ();

		RigidBody = GetComponent<Rigidbody2D>();

		OnGround = false;
		Stopped = false;

		BoxCollider = GetComponent<BoxCollider2D>();

		SnowTrail = GetComponentInChildren <ParticleSystem> ();

	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Stop") {
			RigidBody.velocity = Vector2.zero;
			Stopped = true;
		}

	}

	void MoveCamera()
	{
		Vector3 PlayerScreenPosition = Camera.WorldToScreenPoint (gameObject.transform.position);
		Vector3 CameraScreenPosition = Camera.WorldToScreenPoint (Camera.transform.position);

		float CameraTop = Camera.pixelRect.yMax;

		float UpperBoundary = CameraFollowUpperPercentage * CameraTop;
		float LowerBoundary = CameraFollowLowerPercentage * CameraTop;


		if (PlayerScreenPosition.y < LowerBoundary) 
		{
			CameraScreenPosition.y -= (LowerBoundary -PlayerScreenPosition.y);
		} 
		else if (PlayerScreenPosition.y > UpperBoundary) 
		{
			CameraScreenPosition.y += (PlayerScreenPosition.y - UpperBoundary);
		}

		Vector3 WorldPosition = Camera.ScreenToWorldPoint (CameraScreenPosition);
		
		Vector3 CameraPosition = new Vector3 (gameObject.transform.position.x, WorldPosition.y, Camera.transform.position.z);

		Camera.transform.position = CameraPosition;
	}
	
	// Update is called once per frame
	void FixedUpdate () {


		if (Stopped) {
			Stopped = !Input.GetKeyDown (KeyCode.LeftShift);
		} else {
			RigidBody.velocity = new Vector2 (maxSpeed, RigidBody.velocity.y);
		}

		Animation.SetBool ("Stopped", Stopped);

		MoveCamera ();

		//Step 1: Find bottom of player
		//Step 2: Cast a ray down from the bottom of the character
		//Step 3: Look at where the ray hit
		//Step 4: If the distance travelled by the ray is "small" then we must be on the ground
		//Step 5: Only jump if we are on the ground
		Vector2 center = BoxCollider.bounds.center;

		float halfHeight = BoxCollider.bounds.extents.y;

		Vector2 bottom = new Vector2 (center.x,center.y - halfHeight);

		//Casts a ray from a point, in a direction
		RaycastHit2D hit = Physics2D.Linecast(bottom + new Vector2(0.0f, -0.001f), bottom + new Vector2 (0.0f, -0.02f));
		OnGround =  hit.collider != null && hit.collider.gameObject.tag == "Ground";

		Animation.SetBool ("OnGround", OnGround);

		int NumberOfTouches = Input.touchCount;
		// || means OR
		if ((NumberOfTouches > 0 || Input.GetKeyDown(KeyCode.Space)) && OnGround)
		{
			RigidBody.AddForce (new Vector2(0.0f, JumpSpeed));
		}
			
	}

	void Update (){
		
		if (OnGround) {
			if (SnowTrail.isStopped) {
				SnowTrail.Simulate (SnowTrail.main.duration);
				SnowTrail.Play ();
			}
		} else {
			SnowTrail.Stop ();
			SnowTrail.Clear ();
		}
	}
}
