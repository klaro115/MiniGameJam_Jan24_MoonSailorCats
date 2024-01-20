using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames
{
	public sealed class RhythmGame : Minigame
    {
		#region Types

		[Serializable]
		public class Beat
		{
			public Image uiImage = null;
			public float position = 0.0f;
			public int columnIdx = 0;
			public bool hit = false;
		}

		#endregion
		#region Fields

		public float winSuccessRate = 0.7f;
		public float scrollSpeed = 0.25f;

		public int totalBeatCount = 10;
		public int visibleBeatCount = 4;
		private int successCount = 0;
		private int errorCount = 0;

		public Image uiBeatPrefab = null;
		public RectTransform[] uiBeatTracks = new RectTransform[4];
		private readonly List<Beat> beats = new();
		private KeyCode[] trackKeys = null;
		public RectTransform uiHitLine = null;

		public Color successMarginColor = Color.yellow;
		public Color successBeatColor = Color.green;
		public Color errorBeatColor = Color.red;
		public Text uiSuccessCount = null;
		public Text uiErrorCount = null;
		public Text uiSuccessRating = null;

		#endregion
		#region Methods

		void Start()
		{
			state = MinigameState.Standby;
			outcome = MinigameOutcome.None;
			StartGame(playerID);                //TEMP
		}

		void Update()
        {
			float successMargin = 1.0f / (visibleBeatCount + 1);
			float distanceScrolled = scrollSpeed * Time.deltaTime;

			// Make beats scroll down in order:
			bool removeFirstBeat = false;
			for (int i = 0; i < beats.Count; ++i)
			{
				Beat beat = beats[i];

				beat.position -= distanceScrolled;
				beat.uiImage.rectTransform.anchorMin = new(0, beat.position);
				beat.uiImage.rectTransform.anchorMax = new(1, beat.position);

				if (beat.position < 0)
				{
					removeFirstBeat = true;
				}
				else if (beat.position < 1 && !beat.uiImage.gameObject.activeSelf)
				{
					beat.uiImage.gameObject.SetActive(true);
				}
				else if (!beat.hit && beat.position < successMargin)
				{
					beat.uiImage.color = successMarginColor;
				}
			}
			if (removeFirstBeat)
			{
				Destroy(beats[0].uiImage.gameObject);
				beats.RemoveAt(0);
			}

			// Respond to movement key strokes:
			if (beats.Count != 0)
			{
				// Identify which key was hit:
				int hitTrackIdx = -1;
				for (int i = 0; i < trackKeys.Length; ++i)
				{
					if (Input.GetKeyDown(trackKeys[i]))
					{
						hitTrackIdx = i;
						break;
					}
				}

				// Check if it was a hit, a miss, or a mistake:
				Beat curBeat = beats[0];
				if (!curBeat.hit && hitTrackIdx >= 0)
				{
					// Hit:
					if (curBeat.position >= 0 && curBeat.position <= successMargin)
					{
						// Correct key:
						if (curBeat.columnIdx == hitTrackIdx)
						{
							curBeat.uiImage.color = successBeatColor;
							successCount++;
						}
						// Wrong key:
						else
						{
							curBeat.uiImage.color = errorBeatColor;
							errorCount++;
						}
						curBeat.hit = true;
					}
					// Miss:
					else
					{
						curBeat.uiImage.color = errorBeatColor;
						errorCount++;
					}
				}
			}

			if (uiSuccessCount != null) uiSuccessCount.text = successCount.ToString();
			if (uiErrorCount != null) uiErrorCount.text = errorCount.ToString();
			if (uiSuccessRating != null) uiSuccessRating.text = $"{(float)successCount / totalBeatCount:0.#}%";
		}

		public override void StartGame(PlayerID _playerID)
		{
			base.StartGame(_playerID);

			successCount = 0;
			errorCount = 0;

			// Create a set of beats randomly across 4 tracks:
			for (int i = 0; i < totalBeatCount; ++i)
			{
				int trackIdx = UnityEngine.Random.Range(0, 100) % 4;
				CreateBeat(trackIdx, i);
			}

			trackKeys = playerID == PlayerID.PlayerOne
				? (new KeyCode[4]
				{
					KeyCode.A,
					KeyCode.W,
					KeyCode.S,
					KeyCode.D
				})
				: (new KeyCode[4]
				{
					KeyCode.LeftArrow,
					KeyCode.UpArrow,
					KeyCode.DownArrow,
					KeyCode.RightArrow
				});

			float successMargin = 1.0f / (visibleBeatCount + 1);
			if (uiHitLine != null)
			{
				uiHitLine.anchorMin = new Vector2(0, successMargin * 0.5f);
				uiHitLine.anchorMax = new Vector2(1, successMargin * 0.5f);
			}
		}

		private void CreateBeat(int _trackIdx, int _beatIdx)
		{
			RectTransform uiParent = uiBeatTracks[_trackIdx];
			Image uiImage = Instantiate(uiBeatPrefab, uiParent);
			float position = 1 + (float)_beatIdx / (visibleBeatCount + 1);

			Beat beat = new()
			{
				uiImage = uiImage,
				position = position,
				columnIdx = _trackIdx,
				hit = false,
			};
			beats.Add(beat);
			//uiImage.gameObject.SetActive(true);
		}

		protected override void EndGame()
		{
			state = MinigameState.Ended;

			foreach (Beat beat in beats)
			{
				Destroy(beat.uiImage);
			}
			beats.Clear();

			float successRate = (float)successCount / totalBeatCount;
			if (successRate > winSuccessRate)
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
