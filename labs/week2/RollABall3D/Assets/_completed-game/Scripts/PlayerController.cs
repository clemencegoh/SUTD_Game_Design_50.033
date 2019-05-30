using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;

public class PlayerController : MonoBehaviour {
	
	// Create public variables for player speed, and for the Text UI game objects
	public float speed;
	public Text countText;
	public Text winText;

	// Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
	private Rigidbody rb;
	private int count;
	private Camera mainCam;

	public float zoomSpeed = 20f;
	public float minZoomFOV = 10f;


	// At the start of the game..
	void Start ()
	{
		// Assign the Rigidbody component to our private rb variable
		rb = GetComponent<Rigidbody>();

		// Set the count to zero 
		count = 0;

		// Run the SetCountText function to update the UI (see below)
		SetCountText ();

		// Set the text property of our Win Text UI to an empty string, making the 'You Win' (game over message) blank
		winText.text = "";

		mainCam = Camera.main; // Grab a reference to the camera

	}

	// Each physics step..
	// Controls player movement
	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");

		//Store the current vertical input in the float moveVertical.
		float moveVertical = Input.GetAxis ("Vertical");

		//Use the two store floats to create a new Vector2 variable movement.
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		//Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
		rb.AddForce (movement * speed);
	}

	// When this game object intersects a collider with 'is trigger' checked, 
	// store a reference to that collider in a variable named 'other'..
	void OnTriggerEnter(Collider other) 
	{
		// TODO: PICKUP THE BOXES
		//Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
		if (other.gameObject.CompareTag("Pick Up")){
			other.gameObject.SetActive(false);
			count = count + 1;
			SetCountText();
			
			// Additional: Add reaction force with particle system
			float moveHorizontal = Input.GetAxis ("Horizontal");

			//Store the current vertical input in the float moveVertical.
			float moveVertical = Input.GetAxis ("Vertical");

			Vector3 movement = new Vector3 (- moveHorizontal, 0.0f, - moveVertical);

			// Reaction impulse force
			rb.AddForce(movement * speed, ForceMode.Impulse);

			// Make camera move closer
			mainCam.fieldOfView -= zoomSpeed/8;
			if (mainCam.fieldOfView < minZoomFOV)
			{
				mainCam.fieldOfView = minZoomFOV;
			}
		}
	}

	// Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
	void SetCountText()
	{
		// Update the text field of our 'countText' variable
		countText.text = "Count: " + count.ToString ();

		// Check if our 'count' is equal to or exceeded 12
		if (count >= 12) 
		{
			// Set the text value of our 'winText'
			winText.text = "You Win!";
		}
	}
}