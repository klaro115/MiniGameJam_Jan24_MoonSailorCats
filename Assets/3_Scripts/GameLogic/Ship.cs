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

		#endregion
		#region Methods

		private void Start()
		{
			hullPoints = GetComponentsInChildren<HullPoint>(false);
			steeringWheel = GetComponentInChildren<SteeringWheel>(false);

			lastEventTime = eventInterval;
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
							if (!hullPt.isBroken)
							{
								Debug.Log($"Hull point {i} broken!");
								hullPt.isBroken = true;
								break;
							}
						}
					}
					break;
				case ShipEventType.SteeringWheel:
					{
						//TODO
						Debug.Log($"Steering wheel event!");
					}
					break;
				//...
				default:
					break;
			}
		}

		#endregion
	}
}
