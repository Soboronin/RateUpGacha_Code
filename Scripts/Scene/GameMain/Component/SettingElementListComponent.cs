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
	public sealed class SettingElementListComponent : MonoBehaviour
	{
		[SerializeField]
		private Transform _settingElementArea;

		[SerializeField]
		private GameObject _countElement;

		private List<SettingElementComponent> _settingElementComponentList = new();

		private SettingElementComponent _noneRareComponent;

		void Awake()
		{

		}

		/// <summary>
		/// データ設定
		/// </summary>
		/// <param name="settingElementExhibitList">確率リスト</param>
		public void SetData(List<SettingElementComponent.SettingElementExhibit> settingElementExhibitList)
		{
			foreach (var settingElementComponent in _settingElementComponentList) {
				settingElementComponent.SetActive(false);
			}

			foreach (var settingElementExhibit in settingElementExhibitList) {
				var settingElementComponent = _settingElementComponentList.FirstOrDefault(rateElementComponent => !rateElementComponent.gameObject.activeSelf);
				if (settingElementComponent != null) {
					settingElementComponent.SetActive(true);
					settingElementComponent.SetData(settingElementExhibit);

					if(settingElementExhibit.Rare == GameMainConst.Rare.NONE) {
						_noneRareComponent = settingElementComponent;
					}
				} else {
					InstantiateRateElement(settingElementExhibit);
				}
			}
		}

		/// <summary>
		/// 確率生成
		/// </summary>
		/// <param name="settingElementExhibit">確率設定</param>
		private void InstantiateRateElement(SettingElementComponent.SettingElementExhibit settingElementExhibit)
		{
			var settingElementObject = Instantiate(_countElement, _settingElementArea);
			Canvas.ForceUpdateCanvases();
			var settingElementComponent = settingElementObject.GetComponentInChildren<SettingElementComponent>();
			settingElementComponent.SetData(settingElementExhibit);
			_settingElementComponentList.Add(settingElementComponent);

			if(settingElementExhibit.Rare == GameMainConst.Rare.NONE) {
				_noneRareComponent = settingElementComponent;
			}
		}

		/// <summary>
		/// ハズレ確率更新
		/// </summary>
		/// <param name="noneRareRate">ハズレ確率</param>
		public void UpdateNoneRareRate(float noneRareRate)
		{
			_noneRareComponent.UpdateNoneRareRate(noneRareRate);
		}

		/// <summary>
		/// 更新用確率リスト取得
		/// </summary>
		/// <returns>更新用確率リスト</returns>
		public List<GameMainConst.Config.RateSetting> GetRateSetting()
		{
			var rateSettingList = new List<GameMainConst.Config.RateSetting>();
			foreach (var settingElementComponent in _settingElementComponentList) {
				rateSettingList.Add(settingElementComponent.GetRateSetting());
			}
			return rateSettingList;
		}
	}
}