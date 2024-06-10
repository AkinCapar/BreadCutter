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

        public CurrencyController()
        {
            
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

        public float GetCurrency(CurrencyType currencyType)
        {
            switch (currencyType)
            {
                case CurrencyType.Money:
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
                case CurrencyType.Money:
                    _currencyModel.UpdateMoney(currencyData.CurrencyValue);
                    break;
            }
        }

        public override void Dispose()
        {

        }
    }
}
