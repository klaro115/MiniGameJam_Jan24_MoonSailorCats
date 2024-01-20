using UnityEngine;

namespace GameLogic
{
	public sealed class Ship : MonoBehaviour
	{
		#region Types

		public enum ShipEventType : int
		{
			HullPointBroken,
			SteeringWheel,
			//...
		}
		private struct UiPopupInstances
		{
			public RectTransform uiPopupStart;
			public RectTransform uiPopupEnd;
		}

		#endregion
		#region Fields

		[Header("Ship Parts:")]
		public HullPoint[] hullPoints = null;
		public SteeringWheel steeringWheel = null;
		//...

		[Header("Events:")]
		public float eventInterval = 8.0f;
		private float lastEventTime = 0.0f;

		private const int EVENT_TYPE_COUNT = 2;

		[Header("UI:")]
		public RectTransform uiPopupHullPrefab_broken = null;
		public RectTransform uiPopupHullPrefab_repaired = null;
		public RectTransform uiPopupSteerPrefab_alert = null;
		public RectTransform uiPopupSteerPrefab_avoided = null;

		private Canvas canvas = null;


		#endregion
		#region Methods

		private void Start()
		{
			canvas = FindObjectOfType<Canvas>();

			hullPoints = GetComponentsInChildren<HullPoint>(false);
			steeringWheel = GetComponentInChildren<SteeringWheel>(false);

			lastEventTime = 0;
		}

		private void Update()
		{
			if (Time.time > lastEventTime + eventInterval)
			{
				lastEventTime = Time.time;

				SpawnNewEvent();
			}
		}

		[ContextMenu("Spawn new event")]
		public void SpawnNewEvent()
		{
			ShipEventType eventType = (ShipEventType)(Random.Range(0, 100) % EVENT_TYPE_COUNT);
			switch (eventType)
			{
				case ShipEventType.HullPointBroken:
					{
						// Break a random hull point: (25 tries, to prevent endless loops)
						for (int i = 0; i < 25; ++i)
						{
							int hullPtIdx = Random.Range(0, hullPoints.Length);
							HullPoint hullPt = hullPoints[hullPtIdx];
							if (!hullPt.IsEventActive)
							{
								Debug.Log($"Hull point {i} broken!");
								var popups = SpawnPopup(hullPt.transform, uiPopupHullPrefab_broken, uiPopupHullPrefab_repaired);
								hullPt.StartEvent(popups.uiPopupStart, popups.uiPopupEnd);
								break;
							}
						}
					}
					break;
				case ShipEventType.SteeringWheel:
					if (!steeringWheel.IsEventActive)
					{
						Debug.Log($"Steering wheel event!");
						var popups = SpawnPopup(steeringWheel.transform, uiPopupSteerPrefab_alert, uiPopupSteerPrefab_avoided);
						steeringWheel.StartEvent(popups.uiPopupStart, popups.uiPopupEnd);
					}
					break;
				//...
				default:
					break;
			}
		}

		private UiPopupInstances SpawnPopup(Transform _target, RectTransform _uiPopupStartPrefab, RectTransform _uiPopupEndPrefab)
		{
			Vector3 screenPos = Camera.main.WorldToScreenPoint(_target.position) + Vector3.up * 50;

			RectTransform uiPopupStart = Instantiate(_uiPopupStartPrefab, canvas.transform);
			uiPopupStart.gameObject.SetActive(true);
			uiPopupStart.position = screenPos;

			RectTransform uiPopupEnd = Instantiate(_uiPopupEndPrefab, canvas.transform);
			uiPopupEnd.gameObject.SetActive(false);
			uiPopupEnd.position = screenPos;

			return new UiPopupInstances()
			{
				uiPopupStart = uiPopupStart,
				uiPopupEnd = uiPopupEnd,
			};
		}

		#endregion
	}
}
