using GameLogic;
using Minigames;
using UnityEngine;

namespace Player
{
	public class PlayerCharacter : MonoBehaviour
	{
		#region Types

		public enum ControlMode
		{
			Ship,
			Minigame,
		}

		#endregion
		#region Fields

		[SerializeField]
		private PlayerID playerID = PlayerID.PlayerOne;
		[SerializeField]
		private ControlMode controlMode = ControlMode.Ship;

		public float walkSpeed = 3.0f;
		private Vector3 lastMoveDir = Vector3.down;

		private CharacterController ctrl = null;
		private Canvas canvas = null;

		private ShipEventTarget activeTarget = null;
		private Minigame activeMinigame = null;
		public Minigame[] uiMinigamePrefabs = new Minigame[2];
		private RectTransform uiPendingPopup = null;
		private float pendingPopupTimeout = 0.0f;

		#endregion
		#region Properties

		public PlayerID PlayerID => playerID;

		public KeyCode ActionKey => playerID == PlayerID.PlayerOne ? KeyCode.E : KeyCode.RightShift;

		#endregion
		#region Methods

		private void Start()
		{
			ctrl = GetComponent<CharacterController>();
			canvas = FindObjectOfType<Canvas>();
		}

		void Update()
		{
			if (controlMode == ControlMode.Minigame && activeMinigame != null)
			{
				UpdateMinigameControls();
			}
			else
			{
				UpdateShipControls();
			}

			if (uiPendingPopup != null && Time.time > pendingPopupTimeout)
			{
				Destroy(uiPendingPopup.gameObject);
				uiPendingPopup = null;
			}
		}

		private void UpdateShipControls()
		{
			var inputAxis = playerID switch
			{
				PlayerID.PlayerOne => new Vector2(Input.GetAxisRaw("Horizontal_AD"), Input.GetAxisRaw("Vertical_WS")),
				PlayerID.PlayerTwo => new Vector2(Input.GetAxisRaw("Horizontal_LeftRight"), Input.GetAxisRaw("Vertical_UpDown")),
				_ => Vector2.zero,
			};
			Vector3 moveDir = new Vector3(inputAxis.x, inputAxis.y, 0).normalized;
			if (moveDir.sqrMagnitude > 0.01f)
			{
				lastMoveDir = moveDir;
			}
			Vector3 moveSpeed = Time.deltaTime * walkSpeed * moveDir;

			ctrl.Move(moveSpeed);

			CheckInteraction();
		}

		private void UpdateMinigameControls()
		{
			if (activeMinigame == null)
			{
				controlMode = ControlMode.Ship;
				return;
			}

			if (activeMinigame.Outcome == MinigameOutcome.Success)
			{
				if (uiPendingPopup != null)
				{
					Destroy(uiPendingPopup.gameObject);
					uiPendingPopup = null;
				}
				activeTarget.ResolveEvent(out uiPendingPopup);
				pendingPopupTimeout = Time.time + 1.0f;
			}
			if (activeMinigame.Outcome != MinigameOutcome.None)
			{
				Destroy(activeMinigame.gameObject);
				activeTarget = null;
				activeMinigame = null;
			}
		}

		private void CheckInteraction()
		{
			if (Input.GetKeyDown(ActionKey))
			{
				Vector3 lookDir = lastMoveDir.normalized;
				if (Physics.Raycast(transform.position + lookDir * ctrl.radius, lookDir, out RaycastHit hit, 1.0f) &&
					hit.transform.TryGetComponent(out ShipEventTarget target) &&
					target.IsEventActive)
				{
					int minigameIdx = Random.Range(0, 100) % uiMinigamePrefabs.Length;
					activeMinigame = Instantiate(uiMinigamePrefabs[minigameIdx], canvas.transform);
					activeMinigame.StartGame(playerID);
					activeTarget = target;
					controlMode = ControlMode.Minigame;
				}
			}
		}

		#endregion
	}
}
