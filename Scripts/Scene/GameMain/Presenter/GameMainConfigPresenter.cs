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
        public class GameMainConfigPresenter : MonoBehaviour
        {
            private GameMainModel _mainModel;
            private GameMainConfigModel _model;
            private GameMainConfigView _view;

            private bool _initConfig = false;

            public void Initialize(GameMainModel mainModel, GameMainConfigModel model, GameMainConfigView view)
            {
                _mainModel = mainModel;
                _model = model;
                _view = view;

                // view → model
                _view.OnCloseButtonClicked
                .Subscribe(config => Close(config))
                .AddTo(this);

                // mainModel → view
                _mainModel.OnConfig
                .Subscribe(_ => OpenConfig())
                .AddTo(this);
            }

            /// <summary>
            /// 閉じる
            /// </summary>
            /// <param name="config">設定</param>
            private void Close(GameMainConst.Config config) {
                _mainModel.UpdateConfig(config);
                _mainModel.Initialized();
                _view.SetActive(false);
            }

            /// <summary>
            /// 設定
            /// </summary>
            private void OpenConfig() {
                _view.SetConfig(_mainModel.Config, _mainModel.SettingList);

                if(!_initConfig) {
                    foreach(var setting in _mainModel.SettingList) {
                        if(setting.Rare == GameMainConst.Rare.NONE) {
                            continue;
                        }
                        setting.OnInputRate
                        .Subscribe(rate => _view.UpdateNoneRareRate(_model.SettingPreview(setting, rate)))
                        .AddTo(this);
                    }
                    _initConfig = true;
                }

                _model.UpdatePreviewExhibitList(_mainModel.SettingList);
            }
        }
    }
}