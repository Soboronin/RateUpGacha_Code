using UniRx;
using UnityEngine;
using System;
using Common;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Runtime.InteropServices;

namespace GameMain
{
	public sealed class SettingElementComponent : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI _rareName;

		[SerializeField]
		private TextMeshProUGUI _beforeRateText;

		[SerializeField]
		private TextMeshProUGUI _noneRareRateText;

		[SerializeField]
		private TMP_InputField _rateInputField;

		private SettingElementExhibit _settingElementExhibit;

		/// <summary>
		/// 確率入力 イベント
		/// </summary>
		private readonly Subject<float> _onInputRate = new();

		public sealed class SettingElementExhibit
		{
			public GameMainConst.Rare Rare { get; set; }

			public float BeforeRate { get; set; }

			public IObservable<float> OnInputRate { get; set; }
		}

		void Awake()
		{
			_rateInputField.onEndEdit.AddListener((rate) => {
				if (rate == "") {
					return;
				}
				_onInputRate.OnNext(float.Parse(rate));
			});
		}

		/// <summary>
		/// データ設定
		/// </summary>
		/// <param name="countElementExhibit">回数情報</param>
		public void SetData(SettingElementExhibit settingElementExhibit)
		{
			_settingElementExhibit = settingElementExhibit;

			_rareName.text = GameMainConst.GetRareName(settingElementExhibit.Rare);

			_rateInputField.text = "";

			if (settingElementExhibit.Rare == GameMainConst.Rare.NONE) {
				_rateInputField.gameObject.SetActive(false);
				_noneRareRateText.gameObject.SetActive(true);
				_noneRareRateText.text = settingElementExhibit.BeforeRate.ToString();
			} else {
				_rateInputField.gameObject.SetActive(true);
				_noneRareRateText.gameObject.SetActive(false);
				_beforeRateText.text = settingElementExhibit.BeforeRate.ToString();
				settingElementExhibit.OnInputRate = _onInputRate;
			}
		}

		/// <summary>
		/// ハズレ確率更新
		/// </summary>
		/// <param name="noneRareRate">ハズレ確率</param>
		public void UpdateNoneRareRate(float noneRareRate)
		{
			_noneRareRateText.text = noneRareRate.ToString();
		}

		/// <summary>
		/// 出し分け
		/// </summary>
		public void SetActive(bool isActive)
		{
			gameObject.SetActive(isActive);
		}

		/// <summary>
		/// 更新用確率取得
		/// </summary>
		/// <returns>更新用確率</returns>
		public GameMainConst.Config.RateSetting GetRateSetting()
		{
			var rateSetting = new GameMainConst.Config.RateSetting();
			rateSetting.Rare = _settingElementExhibit.Rare;
			if (_settingElementExhibit.Rare == GameMainConst.Rare.NONE) {
				rateSetting.Rate = float.Parse(_noneRareRateText.text);
			} else {
				rateSetting.Rate = _rateInputField.text == "" ? _settingElementExhibit.BeforeRate : float.Parse(_rateInputField.text);
			}
			return rateSetting;
		}
	}
}