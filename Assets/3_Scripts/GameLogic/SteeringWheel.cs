using UnityEngine;

namespace GameLogic
{
	public class SteeringWheel : MonoBehaviour
	{
		#region Fields

		[SerializeField]
		private bool onCollisionCourse = false;

		private RectTransform uiPopupAlert = null;
		private RectTransform uiPopupAvoided = null;

		#endregion
		#region Properties

		public bool OnCollisionCourse => onCollisionCourse;

		#endregion
		#region Methods

		public void StartEvent(RectTransform _uiPopupAlert, RectTransform _uiPopupAvoided)
		{
			onCollisionCourse = true;

			if (uiPopupAlert != null && uiPopupAlert != _uiPopupAlert)
			{
				Destroy(uiPopupAlert.gameObject);
			}
			if (uiPopupAvoided != null && uiPopupAlert != _uiPopupAvoided)
			{
				Destroy(uiPopupAvoided.gameObject);
			}

			uiPopupAlert = _uiPopupAlert;
			uiPopupAvoided = _uiPopupAvoided;
		}

		public void ResolveEvent(out RectTransform _outUiPopupAvoided)
		{
			if (uiPopupAlert != null)
			{
				Destroy(uiPopupAlert.gameObject);
			}
			if (uiPopupAvoided != null)
			{
				uiPopupAvoided.gameObject.SetActive(true);
			}
			_outUiPopupAvoided = uiPopupAvoided;
		}

		#endregion
	}
}
