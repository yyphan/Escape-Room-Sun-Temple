using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunTemple
{


	public class Door : MonoBehaviour
	{
		public bool IsLocked = false;
		public bool DoorClosed = true;
		public float OpenRotationAmount = 90;
		public float RotationSpeed = 1f;
		public float MaxDistance = 3.0f;
		public string playerTag = "Player";
		private Collider DoorCollider;

		private GameObject Player;
		private Camera Cam;
		private CursorManager cursor;
		public GameObject lockPuzzle;

		Vector3 StartRotation;
		float StartAngle = 0;
		float EndAngle = 0;
		float LerpTime = 1f;
		float CurrentLerpTime = 0;
		bool Rotating;


		private bool scriptIsEnabled = true;
		private bool unlockingLock = false;



		void Start()
		{
			StartRotation = transform.localEulerAngles;
			DoorCollider = GetComponent<BoxCollider>();

			if (!DoorCollider)
			{
				Debug.LogWarning(this.GetType().Name + ".cs on " + gameObject.name + "door has no collider", gameObject);
				scriptIsEnabled = false;
				return;
			}

			Player = GameObject.FindGameObjectWithTag(playerTag);

			if (!Player)
			{
				Debug.LogWarning(this.GetType().Name + ".cs on " + this.name + ", No object tagged with " + playerTag + " found in Scene", gameObject);
				scriptIsEnabled = false;
				return;
			}

			Cam = Camera.main;
			if (!Cam)
			{
				Debug.LogWarning(this.GetType().Name + ", No objects tagged with MainCamera in Scene", gameObject);
				scriptIsEnabled = false;
			}

			cursor = CursorManager.instance;

			if (cursor != null)
			{
				cursor.SetCursorToDefault();
			}


		}



		void Update()
		{
			if (scriptIsEnabled && !unlockingLock)
			{
				if (Rotating)
				{
					Rotate();
				}

				if (Input.GetKeyDown(KeyCode.Mouse0))
				{
					TryToOpen();
				}


				if (cursor != null)
				{
					CursorHint();
				}
			}

		}




		void TryToOpen()
		{
			if (Mathf.Abs(Vector3.Distance(transform.position, Player.transform.position)) <= MaxDistance)
			{

				Ray ray = Cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
				RaycastHit hit;

				if (DoorCollider.Raycast(ray, out hit, MaxDistance))
				{
					if (IsLocked == false)
					{
						Activate();
					}
					else
					{
						if (lockPuzzle != null)
						{
							TryOpenLock();
						}
						else
						{
							CharController_Motor playerController = Player.GetComponent<CharController_Motor>();
							playerController.SetPlayerHint("Can't pick this... Got to try somewhere else!");
						}
					}
				}
			}
		}



		void TryOpenLock()
		{
			CharController_Motor playerController = Player.GetComponent<CharController_Motor>();
			if (!playerController.hasLockPick)
            {
				playerController.SetPlayerHint("Wait... I can pick this. Just need to find a lock picker!");
				return;
			}

			unlockingLock = true;

			// lock player movement
			playerController.EnableMovement(false);

			// Set Puzzle
			lockPuzzle.SetActive(true);
			LockPick lockPick = lockPuzzle.GetComponentInChildren<LockPick>();
			lockPick.Initialize();
			lockPick.unlockEvent.AddListener(DoorUnlocked);
		}

		void DoorUnlocked()
		{
			unlockingLock = false;

			// unlock player movement
			CharController_Motor playerController = Player.GetComponent<CharController_Motor>();
			playerController.EnableMovement(true);

			// unset puzzle
			lockPuzzle.SetActive(false);
			LockPick lockPick = lockPuzzle.GetComponentInChildren<LockPick>();
			lockPick.unlockEvent.RemoveListener(DoorUnlocked);

			// set the door as unlocked
			IsLocked = false;
		}




		void CursorHint()
		{
			if (Mathf.Abs(Vector3.Distance(transform.position, Player.transform.position)) <= MaxDistance)
			{
				Ray ray = Cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
				RaycastHit hit;

				if (DoorCollider.Raycast(ray, out hit, MaxDistance))
				{
					if (IsLocked == false)
					{
						cursor.SetCursorToDoor();
					}
					else if (IsLocked == true)
					{
						cursor.SetCursorToLocked();
					}
				}
				else
				{
					cursor.SetCursorToDefault();
				}
			}
		}




		public void Activate()
		{
			if (DoorClosed)
				Open();
			else
				Close();
		}







		void Rotate()
		{
			CurrentLerpTime += Time.deltaTime * RotationSpeed;
			if (CurrentLerpTime > LerpTime)
			{
				CurrentLerpTime = LerpTime;
			}

			float _Perc = CurrentLerpTime / LerpTime;

			float _Angle = CircularLerp.Clerp(StartAngle, EndAngle, _Perc);
			transform.localEulerAngles = new Vector3(transform.eulerAngles.x, _Angle, transform.eulerAngles.z);

			if (CurrentLerpTime == LerpTime)
			{
				Rotating = false;
				DoorCollider.enabled = true;
			}


		}



		void Open()
		{
			DoorCollider.enabled = false;
			DoorClosed = false;
			StartAngle = transform.localEulerAngles.y;
			EndAngle = StartRotation.y + OpenRotationAmount;
			CurrentLerpTime = 0;
			Rotating = true;
		}



		void Close()
		{
			DoorCollider.enabled = false;
			DoorClosed = true;
			StartAngle = transform.localEulerAngles.y;
			EndAngle = transform.localEulerAngles.y - OpenRotationAmount;
			CurrentLerpTime = 0;
			Rotating = true;
		}

	}
}