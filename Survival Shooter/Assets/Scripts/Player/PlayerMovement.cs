using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public float speed = 6f;

	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidbody;
	int floorMask;
	float camRayLength = 100f;

	private void Awake()
	{
		floorMask = LayerMask.GetMask( "Floor" );
		anim = GetComponent<Animator>();
		playerRigidbody = GetComponent<Rigidbody>();

	}

	private void FixedUpdate()
	{
		float h = Input.GetAxisRaw( "Horizontal" );
		float v = Input.GetAxisRaw( "Vertical" );

		Move( h, v );
		Turning();
		Animating( h, v );
	}

	void Move( float h, float v )
	{
		// Set the movement vector based on the axis input.
		movement.Set( h, 0, v );
		// Normalise the movement vector and make it proportional to the speed per second.
		movement = movement.normalized * speed * Time.deltaTime;

		// Move the player to it's current position plus the movement.
		playerRigidbody.MovePosition( transform.position + movement);
	}

	void Turning()
	{
		Ray camRay = Camera.main.ScreenPointToRay( Input.mousePosition );
		RaycastHit floorHit;

		if(Physics.Raycast(camRay, out floorHit, camRayLength, floorMask ) )
		{
			// Create a vector from the player to the point on the floor the raycast from the mouse hit.
			Vector3 playerToMouse = floorHit.point - transform.position;

			// Ensure the vector is entirely along the floor plane.
			playerToMouse.y = 0f;

			// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
			Quaternion newRotation = Quaternion.LookRotation( playerToMouse );

			// Set the player's rotation to this new rotation.
			playerRigidbody.MoveRotation( newRotation );
		}
		
	}

	void Animating( float h, float v )
	{
		bool walking = h != 0f || v != 0f;

		// Tell the animator whether or not the player is walking.
		anim.SetBool( "IsWalking", walking );
	}
}
