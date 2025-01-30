using UniRx;
using UnityEngine;
using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine.UI;
using TMPro;

namespace GameMain
{
	namespace MVRP.Views
	{
		public sealed class GameMainResultView : MonoBehaviour
		{
			[SerializeField]
			private CommonButton _resetButton;

			[SerializeField]
			private CountElementListComponent _countListComponent;

			/// <summary>
			/// リセットボタンクリック イベント
			/// </summary>
			private readonly Subject<Unit> _onResetButtonClicked = new();
			public IObservable<Unit> OnResetButtonClicked => _onResetButtonClicked;

			void Awake()
			{
				_resetButton.onClick.AddListener(() => {
					_onResetButtonClicked.OnNext(Unit.Default);
				});

				SetActive(false);
			}

			/// <summary>
			/// リザルト設定
			/// </summary>
			/// <param name="countElementExhibitList">回数設定リスト</param>
			public void SetResult(List<CountElementComponent.CountElementExhibit> countElementExhibitList)
			{
				SetActive(true);
				_countListComponent.SetData(countElementExhibitList);
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
}