using UnityEngine;

namespace Player
{
    public class PlayerCharacter : MonoBehaviour
    {
        #region Fields

        public PlayerInputType inputType = PlayerInputType.WASD;
		public float walkSpeed = 3.0f;

        private CharacterController ctrl = null;

		#endregion
		#region Methods

		private void Start()
		{
            ctrl = GetComponent<CharacterController>();
		}

		void Update()
        {
            Vector2 inputAxis;
            switch (inputType)
            {
                case PlayerInputType.WASD:
                    inputAxis = new Vector2(Input.GetAxisRaw("Horizontal_AD"), Input.GetAxisRaw("Vertical_WS"));
					break;
                case PlayerInputType.ArrowKeys:
                    inputAxis = new Vector2(Input.GetAxisRaw("Horizontal_LeftRight"), Input.GetAxisRaw("Vertical_UpDown"));
                    break;
				default:
                    inputAxis = Vector2.zero;
					break;
            }

            Vector3 moveDir = new Vector3(inputAxis.x, inputAxis.y, 0).normalized;
            Vector3 moveSpeed = Time.deltaTime * walkSpeed * moveDir;

            ctrl.Move(moveSpeed);
        }

        #endregion
    }
}
