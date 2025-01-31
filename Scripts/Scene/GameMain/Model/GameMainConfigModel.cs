using UniRx;
using UnityEngine;
using GameMain;
using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace GameMain
{
	namespace MVRP.Models
	{
		public sealed class GameMainConfigModel
		{
			// 確率設定プレビューリスト
			private List<SettingElementComponent.SettingElementExhibit> _settingElementExhibitList = new();
			public List<SettingElementComponent.SettingElementExhibit> SettingPreviewList => _settingElementExhibitList;

			/// <summary>
			/// 設定更新プレビュー イベント
			/// </summary>
			private readonly Subject<Unit> _onConfigPreview = new();
			public IObservable<Unit> OnConfigPreview => _onConfigPreview;

			public GameMainConfigModel()
			{

			}

			~GameMainConfigModel()
			{

			}

			/// <summary>
			/// 設定プレビュー用リスト更新
			/// </summary>
			/// <param name="SettingElementExhibitList">確率設定リスト</param>
			public void UpdatePreviewExhibitList(List<SettingElementComponent.SettingElementExhibit> SettingElementExhibitList)
			{
				_settingElementExhibitList = SettingElementExhibitList.ToList();
            }

			/// <summary>
			/// 設定プレビュー更新
			/// </summary>
			/// <param name="updateElement">更新したい確率設定</param>
			/// <param name="rate">確率</param>
			/// <returns>ハズレ確率</returns>
			public float SettingPreview(SettingElementComponent.SettingElementExhibit updateElement, float rate)
			{
				_settingElementExhibitList.FirstOrDefault(setting => setting.Rare == updateElement.Rare).BeforeRate = rate;

				var noneRareRate = GameMainConst.PERCENT_MAX;
				foreach(var settingElementExhibit in _settingElementExhibitList) {
					if(settingElementExhibit.Rare != GameMainConst.Rare.NONE) {
						noneRareRate -= (float)settingElementExhibit.BeforeRate;
					}
				}

				_settingElementExhibitList.FirstOrDefault(setting => setting.Rare == GameMainConst.Rare.NONE).BeforeRate = noneRareRate;

				// float演算誤差調整のため
				noneRareRate = (float)Math.Round(noneRareRate, 3);

				// 更新は閉じるタイミングでしたいのでハズレプレビュー用の確率だけ返す（異常値だった場合に変更を破棄したい為）
				return noneRareRate;
            }
		}
	}
}