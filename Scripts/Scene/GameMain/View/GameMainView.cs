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
		public sealed class GameMainView : MonoBehaviour
		{
			[SerializeField]
			private CommonButton _gachaButton;

			[SerializeField]
			private CommonButton _configButton;

			[SerializeField]
			private TextMeshProUGUI _gachaPtParTimeText;

			[SerializeField]
			private TextMeshProUGUI _gachaCountText;

			[SerializeField]
			private TMP_InputField _ptInputField;

			[SerializeField]
			private RateElementListComponent _rateListComponent;

			[SerializeField]
			private CharactersComponent _charactersComponent;

			[SerializeField]
			private GameObject _rateUp;

			/// <summary>
			/// ガチャクリック イベント
			/// </summary>
			private readonly Subject<Unit> _onGachaButtonClicked = new();
			public IObservable<Unit> OnGachaButtonClicked => _onGachaButtonClicked;

			/// <summary>
			/// Pt入力 イベント
			/// </summary>
			private readonly Subject<int> _onInputPt = new();
			public IObservable<int> OnInputPt => _onInputPt;

			/// <summary>
			/// 設定クリック イベント
			/// </summary>
			private readonly Subject<Unit> _onConfigButtonClicked = new();
			public IObservable<Unit> OnConfigButtonClicked => _onConfigButtonClicked;

			void Awake()
			{
				_gachaButton.onClick.AddListener(() => {
					_onGachaButtonClicked.OnNext(Unit.Default);
				});

				_ptInputField.onEndEdit.AddListener((pt) => {
					if (pt == "") {
						return;
					}
					_onInputPt.OnNext(int.Parse(pt));
				});

				_configButton.onClick.AddListener(() => {
					Initialized();
					_onConfigButtonClicked.OnNext(Unit.Default);
				});
			}

			/// <summary>
			/// 初期化
			/// </summary>
			public void Initialized()
			{
				_ptInputField.text = "";
			}

			/// <summary>
			/// 確率リスト設定
			/// </summary>
			/// <param name="config">設定</param>
			public void SetRateList(GameMainConst.Config config)
			{
				_gachaPtParTimeText.text = config.PointPerTime.ToString();
				_rateListComponent.SetData(config);
			}

			/// <summary>
			/// 回数設定
			/// </summary>
			/// <param name="gachaCount">ガチャ回数</param>
			public void SetGachaCount(int gachaCount)
			{
				_gachaCountText.text = gachaCount + "連引く";
			}

			/// <summary>
			/// 確率アップ表示設定
			/// </summary>
			/// <param name="rateUpCount">確率アップ回数</param>
			public void SetRateUp(int rateUpCount)
			{
				if(rateUpCount > 0) {
					_rateUp.SetActive(true);
				} else {
					_rateUp.SetActive(false);
				}
				_charactersComponent.SetData(rateUpCount);
			}
		}
	}
}