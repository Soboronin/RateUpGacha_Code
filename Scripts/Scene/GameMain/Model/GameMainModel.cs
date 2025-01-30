using UniRx;
using UnityEngine;
using GameMain;
using System;
using UnityEditor.SearchService;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace GameMain
{
	namespace MVRP.Models
	{
		public sealed class GameMainModel
		{
			// 確率アップ回数
			private ReactiveProperty<int> _rateUpCount = new();
			public IReadOnlyReactiveProperty<int> RateUpCount => _rateUpCount;

			// ガチャ回数
			private ReactiveProperty<int> _gachaCount = new();
			public IReadOnlyReactiveProperty<int> GachaCount => _gachaCount;

			// 設定
			private GameMainConst.Config _config = new();
			public GameMainConst.Config Config => _config;

			// 回数リスト
			private List<CountElementComponent.CountElementExhibit> _countList = new();
			public List<CountElementComponent.CountElementExhibit> CountList => _countList;

			// 確率設定リスト
			private List<SettingElementComponent.SettingElementExhibit> _settingList = new();
			public List<SettingElementComponent.SettingElementExhibit> SettingList => _settingList;

			/// <summary>
			/// 確率設定 イベント
			/// </summary>
			private readonly Subject<Unit> _onRateSetting = new();
			public IObservable<Unit> OnRateSetting => _onRateSetting;

			/// <summary>
			/// ガチャ イベント
			/// </summary>
			private readonly Subject<Unit> _onGacha = new();
			public IObservable<Unit> OnGacha => _onGacha;

			/// <summary>
			/// 設定 イベント
			/// </summary>
			private readonly Subject<Unit> _onConfig = new();
			public IObservable<Unit> OnConfig => _onConfig;

			public GameMainModel()
			{
				_config = GameMainConst.Config.GetInitConfig();
			}

			~GameMainModel()
			{

			}

            /// <summary>
            /// 初期化後処理
            /// </summary>
            public void Initialized()
            {
				InitCount();
				_rateUpCount.Value = 0;
				_gachaCount.Value = 0;
				SetRateSettingList();
				_onRateSetting.OnNext(Unit.Default);
            }

			/// <summary>
			/// 確率設定
			/// </summary>
			/// <param name="pt">ポイント数</param>
			public void SetRate(int pt)
			{
				if (_config.IsUprate) {
					_rateUpCount.Value = pt / _config.UpratePoint;
				}

				SetRateSettingList();
				_onRateSetting.OnNext(Unit.Default);
            }

			/// <summary>
			/// ガチャ回数設定
			/// </summary>
			/// <param name="pt">ポイント数</param>
			public void SetGachaCount(int pt)
			{
				_gachaCount.Value = pt / _config.PointPerTime;
            }

			/// <summary>
			/// ガチャ確率設定
			/// </summary>
			private void SetRateSettingList()
			{
				var beforeRateSettingList = _config.BeforeRateSettingList;
				var afterRateSettingList = _config.AfterRateSettingList.ToList();
				afterRateSettingList.RemoveAll(afterRateSetting => afterRateSetting.Rare == GameMainConst.Rare.NONE);

				float totalRareRate = 0.0f;
				foreach(var afterRateSetting in afterRateSettingList) {
					var beforeRate = beforeRateSettingList.FirstOrDefault(rateSetting => rateSetting.Rare == afterRateSetting.Rare).Rate;
					afterRateSetting.Rate = beforeRate + _config.UprateRate * _rateUpCount.Value;
					totalRareRate += afterRateSetting.Rate;
				}

				if(totalRareRate > GameMainConst.PERCENT_MAX) {
					totalRareRate = GameMainConst.PERCENT_MAX;
				}
				_config.AfterRateSettingList.FirstOrDefault(afterRateSetting => afterRateSetting.Rare == GameMainConst.Rare.NONE).Rate = GameMainConst.PERCENT_MAX - totalRareRate;
            }

			/// <summary>
			/// 回数初期化
			/// </summary>
			private void InitCount()
			{
				_countList = new();

				foreach (var rare in GameMainConst.GetRareList()) {
					var countElementExhibit = new CountElementComponent.CountElementExhibit {
						Rare = rare,
						Count = 0,
					};
					_countList.Add(countElementExhibit);
				}
            }

			/// <summary>
			/// ガチャ
			/// </summary>
			public void Gacha()
			{
				var rateSettingList = _config.AfterRateSettingList;

				var totalRate = rateSettingList.Select(rateSetting => rateSetting.Rate).Sum();

				Debug.Log(_gachaCount.Value);
				for (int i = 0; i < _gachaCount.Value; i ++) {
					float num = UnityEngine.Random.Range(0.0f, totalRate);
					num *= 100;
					num = (float)Math.Round(num) / 100;
					if (num >= totalRate) {
						// 四捨五入ではみ出したヤツを調整
						num = totalRate - 0.01f;
					}
					var tempRate = 0.0f;
					foreach (var rateSetting in rateSettingList) {
						tempRate += rateSetting.Rate;
						// 低確率から確認する前提
						if(tempRate > num) {
							var countElementExhibit = _countList.FirstOrDefault(countExhibit => countExhibit.Rare == rateSetting.Rare);
							countElementExhibit.Count ++;
							break;
						}
					}
				}

				_onGacha.OnNext(Unit.Default);
            }

			/// <summary>
			/// 設定項目初期設定
			/// </summary>
			public void InitConfig()
			{
				_settingList = new();
				var beforeRateSettingList = _config.BeforeRateSettingList;

				foreach (var beforeRateSetting in beforeRateSettingList) {
					var settingElementExhibit = new SettingElementComponent.SettingElementExhibit {
						Rare = beforeRateSetting.Rare,
						BeforeRate = beforeRateSetting.Rate,
					};
					_settingList.Add(settingElementExhibit);
				}

				_onConfig.OnNext(Unit.Default);
            }

			/// <summary>
			/// 設定項目更新
			/// </summary>
			/// <param name="config">設定</param>
			public void UpdateConfig(GameMainConst.Config config)
			{
				_config.PointPerTime = config.PointPerTime;
				_config.IsUprate = config.IsUprate;
				_config.UprateRate = config.UprateRate;
				_config.UpratePoint = config.UpratePoint;
				_config.BeforeRateSettingList = config.BeforeRateSettingList;
            }
		}
	}
}