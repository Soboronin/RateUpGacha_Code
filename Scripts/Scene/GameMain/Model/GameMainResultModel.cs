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
		public sealed class GameMainResultModel
		{
			public GameMainResultModel()
			{

			}

			~GameMainResultModel()
			{

			}

			/// <summary>
			/// クリップボードにコピー
			/// </summary>
			/// <param name="countElementExhibitList">回数リスト</param>
			public void Copy(List<CountElementComponent.CountElementExhibit> countElementExhibitList)
			{
				countElementExhibitList.RemoveAll(countExhibit => countExhibit.Rare == GameMainConst.Rare.NONE);
				countElementExhibitList.RemoveAll(countExhibit => countExhibit.Count == 0);

				string countText = "";
				foreach (var countElementExhibit in countElementExhibitList) {
					countText += GameMainConst.GetRareName(countElementExhibit.Rare) + " : " + countElementExhibit.Count;
					if(countElementExhibitList.Last() != countElementExhibit) {
						countText += "\n";
					}
				}
				GUIUtility.systemCopyBuffer = countText;
            }
		}
	}
}