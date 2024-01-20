using UnityEngine;

namespace Player
{
	public class PlayerCharacter : MonoBehaviour
	{
		#region Fields

		[SerializeField]
		private PlayerID playerID = PlayerID.PlayerOne;

		public float walkSpeed = 3.0f;

		private CharacterController ctrl = null;

		#endregion
		#region Properties

		public PlayerID PlayerID => playerID;

		#endregion
		#region Methods

		private void Start()
		{
			ctrl = GetComponent<CharacterController>();
		}

		void Update()
		{
			var inputAxis = playerID switch
			{
				PlayerID.PlayerOne => new Vector2(Input.GetAxisRaw("Horizontal_AD"), Input.GetAxisRaw("Vertical_WS")),
				PlayerID.PlayerTwo => new Vector2(Input.GetAxisRaw("Horizontal_LeftRight"), Input.GetAxisRaw("Vertical_UpDown")),
				_ => Vector2.zero,
			};
			Vector3 moveDir = new Vector3(inputAxis.x, inputAxis.y, 0).normalized;
			Vector3 moveSpeed = Time.deltaTime * walkSpeed * moveDir;

			ctrl.Move(moveSpeed);
		}

		#endregion
	}
}
