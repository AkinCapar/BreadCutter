using System.Collections;
using System.Collections.Generic;
using BreadCutter.Data;
using BreadCutter.Models;
using BreadCutter.Utils;
using UnityEngine;

namespace BreadCutter.Controllers
{
    public class CurrencyController : BaseController
    {
        #region Injection

        private CurrencyModel _currencyModel;

        public CurrencyController(CurrencyModel currencyModel)
        {
            _currencyModel = currencyModel;
        }
        #endregion
        
        public override void Initialize()
        {

        }
        
        public void AddCurrency(CurrencyData currencyData)
        {
            var newValue = GetCurrency(currencyData.CurrencyType) + currencyData.CurrencyValue;
            UpdateCurrency(new CurrencyData(currencyData.CurrencyType, newValue));
        }

        public int GetCurrency(CurrencyType currencyType)
        {
            switch (currencyType)
            {
                case CurrencyType.Coin:
                    return _currencyModel.MoneyValue;
            }

            return 0;
        }

        public bool TryConsume(CurrencyData currencyData)
        {
            if (!_currencyModel.HasEnoughCurrency(currencyData)) return false;
            var newValue = GetCurrency(currencyData.CurrencyType) - currencyData.CurrencyValue;
            UpdateCurrency(new CurrencyData(currencyData.CurrencyType, newValue));

            return true;
        }

        private void UpdateCurrency(CurrencyData currencyData)
        {
            switch (currencyData.CurrencyType)
            {
                case CurrencyType.Coin:
                    _currencyModel.UpdateMoney(currencyData.CurrencyValue);
                    _signalBus.Fire(new TotalCoinChangedSignal(_currencyModel.MoneyValue));
                    break;
            }
        }

        public override void Dispose()
        {

        }
    }
}
