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
		public sealed class GameMainConfigView : MonoBehaviour
		{
			[SerializeField]
			private CommonButton _closeButton;

			[SerializeField]
			private SettingElementListComponent _settingListComponent;

			[SerializeField]
			private GameObject _Error;

			[SerializeField]
			private Toggle _uprateToggle;

			[SerializeField]
			private TMP_InputField _ptInputField;

			[SerializeField]
			private TMP_InputField _upPtInputField;

			[SerializeField]
			private TMP_InputField _upRateInputField;

			[SerializeField]
			private TextMeshProUGUI _beforePtText;

			[SerializeField]
			private TextMeshProUGUI _beforeUpPtText;

			[SerializeField]
			private TextMeshProUGUI _beforeUpRateText;

			private GameMainConst.Config _config = new();

			/// <summary>
			/// 閉じるボタンクリック イベント
			/// </summary>
			private readonly Subject<GameMainConst.Config> _onCloseButtonClicked = new();
			public IObservable<GameMainConst.Config> OnCloseButtonClicked => _onCloseButtonClicked;

			void Awake()
			{
				_closeButton.onClick.AddListener(() => {
					_onCloseButtonClicked.OnNext(GetConfig());
				});
				SetActive(false);
			}

			/// <summary>
			/// 設定
			/// </summary>
			/// <param name="config">設定</param>
			/// <param name="settingElementExhibitList">設定情報リスト</param>
			public void SetConfig(GameMainConst.Config config, List<SettingElementComponent.SettingElementExhibit> settingElementExhibitList)
			{
				_config = config;

				SetActive(true);
				_beforePtText.text = _config.PointPerTime.ToString();
				_beforeUpPtText.text = _config.UpratePoint.ToString();
				_beforeUpRateText.text = _config.UprateRate.ToString();
				_ptInputField.text = "";
				_upPtInputField.text = "";
				_upRateInputField.text = "";

				_uprateToggle.isOn = _config.IsUprate;
				_settingListComponent.SetData(settingElementExhibitList);
			}

			/// <summary>
			/// 出し分け
			/// </summary>
			/// <param name="isActive">アクティブの時true</param>
			public void SetActive(bool isActive)
			{
				gameObject.SetActive(isActive);
			}

			/// <summary>
			/// ハズレ確率更新
			/// </summary>
			/// <param name="noneRareRate">ハズレ確率</param>
			public void UpdateNoneRareRate(float noneRareRate)
			{
				_settingListComponent.UpdateNoneRareRate(noneRareRate);
				_Error.SetActive(noneRareRate < 0);
			}

			/// <summary>
			/// 更新用の設定を作る
			/// </summary>
			public GameMainConst.Config GetConfig()
			{
				var config = new GameMainConst.Config {
					PointPerTime = _ptInputField.text == "" ? _config.PointPerTime : int.Parse(_ptInputField.text),
                    IsUprate = _uprateToggle.isOn,
					UpratePoint = _upPtInputField.text == "" ? _config.UpratePoint : int.Parse(_upPtInputField.text),
					UprateRate = _upRateInputField.text == "" ? _config.UprateRate : float.Parse(_upRateInputField.text),
                    BeforeRateSettingList = _settingListComponent.GetRateSetting(),
					AfterRateSettingList = _config.AfterRateSettingList,
                };

				return config;
			}
		}
	}
}