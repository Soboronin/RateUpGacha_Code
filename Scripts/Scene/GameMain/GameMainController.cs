using UniRx;
using UnityEngine;
using System;
using UnityEditor.SearchService;
using GameMain.MVRP.Models;
using GameMain.MVRP.Views;
using GameMain.MVRP.Presenters;
using Common;

namespace GameMain
{
	public sealed class GameMainController : MonoBehaviour
	{
		[SerializeField]
		private GameMainView _GameMainView;

		[SerializeField]
		private GameMainResultView _GameMainResultView;

		[SerializeField]
		private GameMainConfigView _GameMainConfigView;

		void Awake()
		{
			var GameMainModel = new GameMainModel();
			var GameMainResultModel = new GameMainResultModel();
			var GameMainConfigModel = new GameMainConfigModel();

			_GameMainView.gameObject.AddComponent<GameMainPresenter>().Initialize(GameMainModel, _GameMainView);
			_GameMainResultView.gameObject.AddComponent<GameMainResultPresenter>().Initialize(GameMainModel, GameMainResultModel, _GameMainResultView);
			_GameMainConfigView.gameObject.AddComponent<GameMainConfigPresenter>().Initialize(GameMainModel, GameMainConfigModel, _GameMainConfigView);

			GameMainModel.Initialized();
		}
	}
}