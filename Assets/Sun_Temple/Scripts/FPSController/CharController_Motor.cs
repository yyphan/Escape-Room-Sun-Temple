using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunTemple
{


	public class CharController_Motor : MonoBehaviour
	{

		public float normalSpeed = 5.0f;
		public float sensitivity = 60.0f;
		CharacterController character;
		public GameObject cam;
		float moveFB, moveLR;
		float rotHorizontal, rotVertical;
		public bool webGLRightClickRotation = true;
		float gravity = -9.8f;

		//string debugText;

		private bool canMove = true;
		public bool hasLockPick = false;
		public PlayerHint playerHint;
		public ParticleSystem poisonGas;
		public GameObject countdownBar;
		public Animator trapAnim;


		void Start()
		{

			character = GetComponent<CharacterController>();

			webGLRightClickRotation = false;

			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				webGLRightClickRotation = true;
				sensitivity = sensitivity * 1.5f;
			}


		}




		public void EnableMovement(bool enabled)
		{
			canMove = enabled;
		}





		void FixedUpdate()
		{
			if (!canMove) return;

			float speed = normalSpeed;
			if (Input.GetKey(KeyCode.LeftShift))
            {
				speed = normalSpeed + 2;
            }

			moveFB = Input.GetAxis("Horizontal") * speed;
			moveLR = Input.GetAxis("Vertical") * speed;

			rotHorizontal = Input.GetAxisRaw("Mouse X") * sensitivity;
			rotVertical = Input.GetAxisRaw("Mouse Y") * sensitivity;


			Vector3 movement = new Vector3(moveFB, gravity, moveLR);


			if (webGLRightClickRotation)
			{
				if (Input.GetKey(KeyCode.Mouse0))
				{
					CameraRotation(cam, rotHorizontal, rotVertical);
				}
			}
			else if (!webGLRightClickRotation)
			{
				CameraRotation(cam, rotHorizontal, rotVertical);
			}

			movement = transform.rotation * movement;
			character.Move(movement * Time.fixedDeltaTime);

			// ray from our mouse position
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// if the ray hits something
			if (Physics.Raycast(ray, out hit))
            {
				GameObject gameObject = hit.transform.gameObject;
				// check if it is artifact
				if (gameObject.CompareTag("Artifact"))
                {
					// clickiing left mouse will pick it up
					if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
						Destroy(gameObject);
						SetPlayerHint("Got it! Now just need to get out of here");
						// play gas effect
						poisonGas.Play();
						// start countdown
						countdownBar.SetActive(true);
						Countdown countdownScript = countdownBar.GetComponent<Countdown>();
						countdownScript.StartCountdown();
						// drop the beam
						trapAnim.Play("Trap", 0, 0);
                    }
                }

				// check if it is a lockpick
				if (gameObject.CompareTag("LockPick"))
				{
					// clickiing left mouse will pick it up
					if (Input.GetKeyDown(KeyCode.Mouse0))
					{
						Destroy(gameObject);
						SetPlayerHint("Got it! Now the door!");
						hasLockPick = true;
					}
				}
			}
		}




		public void StopCountdown()
		{
			Countdown countdownScript = countdownBar.GetComponent<Countdown>();
			countdownScript.StopCountdown();
			countdownBar.SetActive(false);
		}





		public void SetPlayerHint(string text)
        {
			playerHint.SetPlayerHintText(text);
		}







		void CameraRotation(GameObject cam, float rotHorizontal, float rotVertical)
		{

			transform.Rotate(0, rotHorizontal * Time.fixedDeltaTime, 0);
			cam.transform.Rotate(-rotVertical * Time.fixedDeltaTime, 0, 0);



			if (Mathf.Abs(cam.transform.localRotation.x) > 0.7)
			{

				float clamped = 0.7f * Mathf.Sign(cam.transform.localRotation.x);

				Quaternion adjustedRotation = new Quaternion(clamped, cam.transform.localRotation.y, cam.transform.localRotation.z, cam.transform.localRotation.w);
				cam.transform.localRotation = adjustedRotation;
			}


		}




	}



}