using UniRx;
using UnityEngine;
using System;
using Common;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace GameMain
{
	public sealed class RateElementComponent : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI _rareName;

		[SerializeField]
		private TextMeshProUGUI _rateText;

		private Color _defaultColor;

		void Awake()
		{
			_defaultColor = _rateText.color;
		}

		/// <summary>
		/// データ設定
		/// </summary>
		/// <param name="rateSetting">確率設定</param>
		/// <param name="isDiff">調整前と差がある場合true</param>
		public void SetData(GameMainConst.Config.RateSetting rateSetting, bool isDiff)
		{
			if (isDiff) {
				_rateText.color = new Color32 (249, 85, 129, 255);
			} else {
				_rateText.color = _defaultColor;
			}

			_rareName.text = GameMainConst.GetRareName(rateSetting.Rare);
			_rateText.text = rateSetting.Rate.ToString("0.00") + "%";
		}

		/// <summary>
		/// 出し分け
		/// </summary>
		public void SetActive(bool isActive)
		{
			gameObject.SetActive(isActive);
		}
	}
}