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
	public sealed class CountElementComponent : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI _rareName;

		[SerializeField]
		private TextMeshProUGUI _countText;

		public sealed class CountElementExhibit
		{
			public GameMainConst.Rare Rare { get; set; }

			public int Count { get; set; }
		}

		void Awake()
		{

		}

		/// <summary>
		/// データ設定
		/// </summary>
		/// <param name="countElementExhibit">回数情報</param>
		public void SetData(CountElementExhibit countElementExhibit)
		{
			_rareName.text = GameMainConst.GetRareName(countElementExhibit.Rare);
			_countText.text = countElementExhibit.Count + "回";
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