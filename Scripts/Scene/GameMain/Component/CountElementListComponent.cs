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
	public sealed class CountElementListComponent : MonoBehaviour
	{
		[SerializeField]
		private Transform _countElementArea;

		[SerializeField]
		private GameObject _countElement;

		private List<CountElementComponent> _countElementComponentList = new();

		void Awake()
		{

		}

		/// <summary>
		/// データ設定
		/// </summary>
		/// <param name="countElementExhibitList">回数リスト</param>
		public void SetData(List<CountElementComponent.CountElementExhibit> countElementExhibitList)
		{
			foreach (var countElementComponent in _countElementComponentList) {
				countElementComponent.SetActive(false);
			}

			countElementExhibitList.Reverse();

			foreach (var countElementExhibit in countElementExhibitList) {
				var countElementComponent = _countElementComponentList.FirstOrDefault(rateElementComponent => !rateElementComponent.gameObject.activeSelf);
				if (countElementComponent != null) {
					countElementComponent.SetActive(true);
					countElementComponent.SetData(countElementExhibit);
				} else {
					InstantiateRateElement(countElementExhibit);
				}
			}

		}

		/// <summary>
		/// 確率生成
		/// </summary>
		/// <param name="rateSetting">確率設定</param>
		private void InstantiateRateElement(CountElementComponent.CountElementExhibit countElementExhibit)
		{
			var countElementObject = Instantiate(_countElement, _countElementArea);
			Canvas.ForceUpdateCanvases();
			var countElementComponent = countElementObject.GetComponentInChildren<CountElementComponent>();
			countElementComponent.SetData(countElementExhibit);
			_countElementComponentList.Add(countElementComponent);
		}
	}
}