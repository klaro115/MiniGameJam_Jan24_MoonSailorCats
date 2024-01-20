using UnityEngine;

namespace GameLogic
{
	public sealed class HullPoint : MonoBehaviour
	{
		#region Fields

		[SerializeField]
		private bool isBroken = false;

		private RectTransform uiPopupBroken = null;
		private RectTransform uiPopupRepaired = null;

		#endregion
		#region Properties

		public bool IsBroken => isBroken;

		#endregion
		#region Methods

		public void StartEvent(RectTransform _uiPopupBroken, RectTransform _uiPopupRepaired)
		{
			isBroken = true;

			if (uiPopupBroken != null && uiPopupBroken != _uiPopupBroken)
			{
				Destroy(uiPopupBroken.gameObject);
			}
			if (uiPopupRepaired != null && uiPopupBroken != _uiPopupRepaired)
			{
				Destroy(uiPopupRepaired.gameObject);
			}

			uiPopupBroken = _uiPopupBroken;
			uiPopupRepaired = _uiPopupRepaired;
		}

		public void ResolveEvent(out RectTransform _outUiPopupRepaired)
		{
			if (uiPopupBroken != null)
			{
				Destroy(uiPopupBroken.gameObject);
			}
			if (uiPopupRepaired != null)
			{
				uiPopupRepaired.gameObject.SetActive(true);
			}
			_outUiPopupRepaired = uiPopupRepaired;
		}

		#endregion
	}
}
