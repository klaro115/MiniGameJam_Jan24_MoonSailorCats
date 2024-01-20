using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames
{
    public class RhythmGame : Minigame
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
		private int succcessHitCounter = 0;

		public Image uiBeatPrefab = null;
		public RectTransform[] uiBeatTracks = new RectTransform[4];
		private List<Beat> beats = new();

		#endregion
		#region Methods

		void Update()
        {
			float successMargin = 1.0f / visibleBeatCount;
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
				else if (beat.position < 1 && beat.uiImage.gameObject.activeSelf)
				{
					beat.uiImage.gameObject.SetActive(true);
				}
			}
			if (removeFirstBeat)
			{
				Destroy(beats[0].uiImage.gameObject);
				beats.RemoveAt(0);
			}

			
			if (Input.GetKeyDown(actionKey))
			{
			}

			//TODO
        }

		public override void StartGame(PlayerID _playerID)
		{
			base.StartGame(_playerID);

			// Create a set of beats randomly across 4 tracks:
			for (int i = 0; i < totalBeatCount; ++i)
			{
				int trackIdx = UnityEngine.Random.Range(0, 100) % 4;
				CreateBeat(trackIdx);
			}
		}

		private void CreateBeat(int _trackIdx)
		{
			RectTransform uiParent = uiBeatTracks[_trackIdx];
			Image uiImage = Instantiate(uiBeatPrefab, uiParent);
			float position = 1 + (float)totalBeatCount / visibleBeatCount;

			Beat beat = new()
			{
				uiImage = uiImage,
				position = position,
				columnIdx = _trackIdx,
				hit = false,
			};
			beats.Add(beat);
			uiImage.gameObject.SetActive(false);
		}

		protected override void EndGame()
		{
			state = MinigameState.Ended;

			foreach (Beat beat in beats)
			{
				Destroy(beat.uiImage);
			}
			beats.Clear();

			float successRate = (float)succcessHitCounter / totalBeatCount;
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
