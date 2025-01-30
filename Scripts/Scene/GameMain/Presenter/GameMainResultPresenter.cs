using UniRx;
using GameMain.MVRP.Models;
using GameMain.MVRP.Views;
using UnityEngine;
using System.Collections.Generic;
using Common;
using Cysharp.Threading.Tasks;
using System.Linq;
using System.Runtime.InteropServices;

namespace GameMain
{
    namespace MVRP.Presenters
    {
        public class GameMainResultPresenter : MonoBehaviour
        {
            private GameMainModel _mainModel;
            private GameMainResultModel _model;
            private GameMainResultView _view;

            public void Initialize(GameMainModel mainModel, GameMainResultModel model, GameMainResultView view)
            {
                _mainModel = mainModel;
                _model = model;
                _view = view;

                // view → model
                _view.OnResetButtonClicked
                .Subscribe(_ => Close())
                .AddTo(this);

                // mainModel → view
                _mainModel.OnGacha
                .Subscribe(_ => _view.SetResult(_mainModel.CountList))
                .AddTo(this);
            }

            /// <summary>
            /// 閉じる
            /// </summary>
            private void Close() {
                _model.Copy(_mainModel.CountList);
                _mainModel.Initialized();
                _view.SetActive(false);
            }
        }
    }
}