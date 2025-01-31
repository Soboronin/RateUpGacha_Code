using UniRx;
using UnityEngine;
using GameMain;
using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using System.Runtime.InteropServices;

namespace GameMain
{
	namespace MVRP.Models
	{
		public sealed class GameMainResultModel
		{
			//コピー
			[DllImport("__Internal")]
			private static extern void CopyWebGL(string str);

    		//ペースト
			[DllImport("__Internal")]
			private static extern void AsyncPasteWebGL();

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
				CopyWebGL(countText);
            }
		}
	}
}