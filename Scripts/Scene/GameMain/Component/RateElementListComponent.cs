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
	public sealed class RateElementListComponent : MonoBehaviour
	{
		[SerializeField]
		private Transform _rateElementArea;

		[SerializeField]
		private GameObject _rateElement;

		private List<RateElementComponent> _rateElementComponentList = new();

		void Awake()
		{

		}

		/// <summary>
		/// データ設定
		/// </summary>
		/// <param name="config">設定</param>
		public void SetData(GameMainConst.Config config)
		{
			foreach (var rateElementComponent in _rateElementComponentList) {
				rateElementComponent.SetActive(false);
			}

			var afterRateSettingList = config.AfterRateSettingList;

			foreach (var afterRateSetting in afterRateSettingList) {
				var rateElementComponent = _rateElementComponentList.FirstOrDefault(rateElementComponent => !rateElementComponent.gameObject.activeSelf);
				var isDiff = config.BeforeRateSettingList.FirstOrDefault(beforeRateSetting => beforeRateSetting.Rare == afterRateSetting.Rare).Rate != afterRateSetting.Rate;
				if (rateElementComponent != null) {
					rateElementComponent.SetActive(true);
					rateElementComponent.SetData(afterRateSetting, isDiff);
				} else {
					InstantiateRateElement(afterRateSetting, isDiff);
				}
			}

		}

		/// <summary>
		/// 確率生成
		/// </summary>
		/// <param name="rateSetting">確率設定</param>
		/// <param name="isDiff">調整前と差がある場合true</param>
		private void InstantiateRateElement(GameMainConst.Config.RateSetting rateSetting, bool isDiff)
		{
			var rateElementObject = Instantiate(_rateElement, _rateElementArea);
			Canvas.ForceUpdateCanvases();
			var rateElementComponent = rateElementObject.GetComponentInChildren<RateElementComponent>();
			rateElementComponent.SetData(rateSetting, isDiff);
			_rateElementComponentList.Add(rateElementComponent);
		}
	}
}