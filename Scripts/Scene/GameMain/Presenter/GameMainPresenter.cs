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
        public class GameMainPresenter : MonoBehaviour
        {
            private GameMainModel _model;
            private GameMainView _view;

            public void Initialize(GameMainModel model, GameMainView view)
            {
                _model = model;
                _view = view;

                // view → model
                _view.OnInputPt
                .Subscribe(pt => SetGacha(pt))
                .AddTo(this);

                _view.OnGachaButtonClicked
                .Where(_ => _model.GachaCount.Value != 0)
                .Subscribe(_ => Gacha())
                .AddTo(this);

                _view.OnConfigButtonClicked
                .Subscribe(_ => _model.InitConfig())
                .AddTo(this);

                // model → view
                _model.RateUpCount
                .Subscribe(rateUpCount => _view.SetRateUp(rateUpCount))
                .AddTo(this);

                _model.GachaCount
                .Subscribe(gachaCount => _view.SetGachaCount(gachaCount))
                .AddTo(this);

                _model.OnRateSetting
                .Subscribe(_ => _view.SetRateList(_model.Config))
                .AddTo(this);
            }

            /// <summary>
            /// ガチャ設定
            /// </summary>
            /// <param name="pt">ポイント数</param>
            private void SetGacha(int pt) {
                _model.SetGachaCount(pt);
                _model.SetRate(pt);
            }

            /// <summary>
            /// ガチャ
            /// </summary>
            private void Gacha() {
                _model.Gacha();
                _view.Initialized();
            }
        }
    }
}