using System.Collections;
using System.Collections.Generic;
using BreadCutter.Data;
using BreadCutter.Models;
using BreadCutter.Settings;
using BreadCutter.Utils;
using UnityEngine;

namespace BreadCutter.Controllers
{
    public class UpgradeController : BaseController
    {
        #region Injection

        private CurrencyController _currencyController;
        private UpgradesModel _upgradesModel;
        private UpgradesPricesSettings _upgradesPricesSettings;

        public UpgradeController(CurrencyController currencyController
            , UpgradesModel upgradesModel
            , UpgradesPricesSettings upgradesPricesSettings)
        {
            _currencyController = currencyController;
            _upgradesModel = upgradesModel;
            _upgradesPricesSettings = upgradesPricesSettings;
        }

        #endregion

        public override void Initialize()
        {
        }

        public bool SpendCoinForUpgrade(UpgradeTypes upgradeType)
        {
            bool result = false;
            switch (upgradeType)
            {
                case UpgradeTypes.AddBread:
                    
                    result = _currencyController.TryConsume(new CurrencyData(CurrencyType.Coin,
                        _upgradesModel.AddBreadValue));

                    if (result && _upgradesPricesSettings.AddBreadPrices.Length > _upgradesModel.AddBreadLevel + 1)
                    {
                        _upgradesModel.UpdateAddBreadValue(
                            _upgradesPricesSettings.AddBreadPrices[_upgradesModel.AddBreadLevel + 1]);
                    }
                    break;
                
                case UpgradeTypes.BladeUpgrade:
                    
                    result = _currencyController.TryConsume(new CurrencyData(CurrencyType.Coin,
                        _upgradesModel.UpgradeBladeValue));
                    
                    if (result && _upgradesPricesSettings.UpgradeBladePrices.Length > _upgradesModel.UpgradeBladeLevel + 1)
                    {
                        _upgradesModel.UpdateUpgradeBladeValue(
                            _upgradesPricesSettings.UpgradeBladePrices[_upgradesModel.UpgradeBladeLevel + 1]);
                    }
                    break;
                
                case UpgradeTypes.Merge:
                    result = _currencyController.TryConsume(new CurrencyData(CurrencyType.Coin,
                        _upgradesModel.MergeValue));

                    if (result && _upgradesPricesSettings.MergePrices.Length > _upgradesModel.MergeLevel + 1)
                    {
                        _upgradesModel.UpdateMergeValue(_upgradesPricesSettings.MergePrices[_upgradesModel.MergeLevel + 1]);
                    }
                    break;
                
                case UpgradeTypes.Expand:
                    result = _currencyController.TryConsume(new CurrencyData(CurrencyType.Coin,
                        _upgradesModel.ExpandValue));

                    if (result && _upgradesPricesSettings.ExpandPrices.Length > _upgradesModel.ExpandLevel + 1)
                    {
                        _upgradesModel.UpdateExpandValue(_upgradesPricesSettings.ExpandPrices[_upgradesModel.ExpandLevel + 1]);
                    }
                    break;
            }

            return result;
        }

        public override void Dispose()
        {
        }
    }
}