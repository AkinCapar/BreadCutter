using System.Collections;
using System.Collections.Generic;
using BreadCutter.Models;
using BreadCutter.Settings;
using BreadCutter.Utils;
using BreadCutter.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BreadCutter.Controllers
{
    public class MergeController : BaseController
    {
        private bool _merging;
        
        #region Injection

        private BreadModel _breadModel;
        private LevelSettings _levelSettings;
        private UpgradeController _upgradeController;

        public MergeController(BreadModel breadModel
            , LevelSettings levelSettings
            , UpgradeController upgradeController)
        {
            _breadModel = breadModel;
            _levelSettings = levelSettings;
            _upgradeController = upgradeController;
        }

        #endregion

        public override void Initialize()
        {
            _signalBus.Subscribe<MergeButtonPressedSignal>(OnMergeButtonPressedSignal);
        }

        private void OnMergeButtonPressedSignal()
        {
            if (_merging)
            {
                return;
            }
            
            for (int i = 0; i < _breadModel.breadLines.Count - 1; i++)
            {
                for (int j = i + 1; j < _breadModel.breadLines.Count; j++)
                {
                    if (_breadModel.breadLines[i].Count > 0 && _breadModel.breadLines[j].Count > 0)
                    {
                        if (_breadModel.breadLines[i][0].BreadLevel == _breadModel.breadLines[j][0].BreadLevel && _upgradeController.SpendCoinForUpgrade(UpgradeTypes.Merge))
                        {
                            _signalBus.Fire<MergeOperationStartedSignal>();
                            _merging = true;
                            Merge(_breadModel.breadLines[i], _breadModel.breadLines[j]).Forget();
                            return;
                        }
                    }
                }
            }
        }


        private async UniTask Merge(List<BreadView> list1, List<BreadView> list2)
        {
            for (int i = 0; i < list1.Count; i++)
            {
                list2[i].Merge(list1[i].transform.position);
            }

            await UniTask.WaitForSeconds(_levelSettings.MergeAnimationTime);

            _signalBus.Fire(new MergeAnimationIsDoneSignal(list1, list2));
            _merging = false;
        }

        public override void Dispose()
        {
            _signalBus.Unsubscribe<MergeButtonPressedSignal>(OnMergeButtonPressedSignal);
        }
    }
}