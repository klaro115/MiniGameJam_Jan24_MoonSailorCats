using Player;
using UnityEngine;

namespace Minigames
{
	public sealed class SwingingReactionGame : Minigame
	{
		#region Fields

		public float hitZoneWidth = 0.333f;
		private float hitZoneStart = 0.0f;
		private float hitZoneEnd = 0.0f;
		public float sliderSpeed = 0.1f;
		private float sliderPosition = 0.0f;

		public RectTransform uiHitZone = null;
		public RectTransform uiSlider = null;

		#endregion
		#region Methods

		void Start()
		{
			state = MinigameState.Standby;
			outcome = MinigameOutcome.None;
			StartGame(playerID);				//TEMP
		}

		void Update()
		{
			if (state == MinigameState.Running)
			{
				// Make slider move from left (0%) to right (100%) following a sine curve:
				sliderPosition = Mathf.Sin(Time.time * sliderSpeed);
				sliderPosition += 1;
				sliderPosition *= 0.5f;

				uiSlider.anchorMin = new Vector2(sliderPosition, 0);
				uiSlider.anchorMax = new Vector2(sliderPosition, 1);

				// Check player inputs:
				if (Input.GetKeyDown(actionKey))
				{
					EndGame();
				}
			}
		}

		public override void StartGame(PlayerID _playerID)
		{
			base.StartGame(_playerID);

			playerID = _playerID;
			outcome = MinigameOutcome.None;
			state = MinigameState.Running;

			float hitZonePosition = Random.Range(0.0f, 1.0f);
			hitZoneStart = Mathf.Max(hitZonePosition - 0.5f * hitZoneWidth, 0);
			hitZoneEnd = Mathf.Min(hitZonePosition + 0.5f * hitZoneWidth, 1);

			uiHitZone.anchorMin = new Vector2(hitZoneStart, 0);
			uiHitZone.anchorMax = new Vector2(hitZoneEnd, 1);
		}

		protected override void EndGame()
		{
			state = MinigameState.Ended;

			if (sliderPosition >= hitZoneStart && sliderPosition <= hitZoneEnd)
			{
				outcome = MinigameOutcome.Success;
			}
			else
			{
				outcome = MinigameOutcome.Failure;
			}
		}

		#endregion
	}
}
