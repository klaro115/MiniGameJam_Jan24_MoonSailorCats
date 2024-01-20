using UnityEngine;

namespace Player
{
    public class PlayerCharacter : MonoBehaviour
    {
        #region Fields

        public PlayerInputType inputType = PlayerInputType.WASD;
        public float walkSpeed = 3.0f;

        private Rigidbody2D rb = null;
        private PlayerPosition playerPosition;

        #endregion
        #region Methods

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            playerPosition = GetComponent<PlayerPosition>();
            if (playerPosition == null)
            {
                Debug.LogError("PlayerPosition component not found on the player object.");
            }
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

            if (playerPosition != null)
            {
                playerPosition.SetDir(inputAxis);
            }

            Vector2 moveDir = inputAxis.normalized;
            Vector2 moveSpeed = walkSpeed * moveDir;

            rb.velocity = moveSpeed;
        }

        #endregion
    }
}
