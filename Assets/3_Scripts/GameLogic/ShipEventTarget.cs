using UnityEngine;

namespace GameLogic
{
	public abstract class ShipEventTarget : MonoBehaviour
	{
		#region Fields

		[SerializeField]
		protected bool isEventActive = false;

		private RectTransform uiPopupStart = null;
		private RectTransform uiPopupEnd = null;

		#endregion
		#region Properties

		public bool IsEventActive => isEventActive;

		#endregion
		#region Methods

		public virtual void StartEvent(RectTransform _uiPopupStart, RectTransform _uiPopupEnd)
		{
			isEventActive = true;

			if (uiPopupStart != null && uiPopupStart != _uiPopupStart)
			{
				Destroy(uiPopupStart.gameObject);
			}
			if (uiPopupEnd != null && uiPopupStart != _uiPopupEnd)
			{
				Destroy(uiPopupEnd.gameObject);
			}

			uiPopupStart = _uiPopupStart;
			uiPopupEnd = _uiPopupEnd;
		}

		public virtual void ResolveEvent(out RectTransform _outUiPopupEnd)
		{
			if (uiPopupStart != null)
			{
				Destroy(uiPopupStart.gameObject);
			}
			if (uiPopupEnd != null)
			{
				uiPopupEnd.gameObject.SetActive(true);
			}
			_outUiPopupEnd = uiPopupEnd;
			isEventActive = false;
		}

		#endregion
	}
}
