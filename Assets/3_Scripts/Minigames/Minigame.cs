using System.Linq;
using Player;
using UnityEngine;

namespace Minigames
{
    public abstract class Minigame : MonoBehaviour
    {
		#region Fields

		[SerializeField]
		protected PlayerID playerID = PlayerID.PlayerOne;
		protected KeyCode actionKey = KeyCode.E;
		[SerializeField]
		protected MinigameState state = MinigameState.Standby;
		[SerializeField]
		protected MinigameOutcome outcome = MinigameOutcome.None;

		#endregion
		#region Properties

		public MinigameOutcome Outcome => state == MinigameState.Ended ? outcome : MinigameOutcome.None;

		#endregion
		#region Methods

		public virtual void StartGame(PlayerID _playerID)
		{
			if (state == MinigameState.Running)
			{
				EndGame();
			}

			actionKey = playerID == PlayerID.PlayerOne
				? KeyCode.E
				: KeyCode.RightShift;

			PlayerCharacter[] players = FindObjectsOfType<PlayerCharacter>(false);
			PlayerCharacter player = players?.FirstOrDefault(o => o.PlayerID == _playerID);
			if (player != null)
			{
				transform.position = Camera.main.WorldToScreenPoint(player.transform.position) + new Vector3(0, 50, 0);
			}
		}

		protected abstract void EndGame();

		#endregion
	}
}
