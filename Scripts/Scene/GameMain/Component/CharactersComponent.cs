using UniRx;
using UnityEngine;
using System;
using Common;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;

namespace GameMain
{
	public sealed class CharactersComponent : MonoBehaviour
	{
		[SerializeField]
		private List<GameObject> _characterList;

		void Awake()
		{

		}

		/// <summary>
		/// データ設定
		/// </summary>
		/// <param name="rateUpCount">確率アップ回数</param>
		public void SetData(int rateUpCount)
		{
			foreach(var character in _characterList) {
				if (rateUpCount > 0) {
					character.SetActive(true);
				} else {
					character.SetActive(false);
				}
				rateUpCount --;
			}
		}
	}
}