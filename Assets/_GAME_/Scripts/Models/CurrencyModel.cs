using BreadCutter.Data;
using BreadCutter.Utils;

namespace BreadCutter.Models
{
    public class CurrencyModel
    {
        public int MoneyValue { get; private set; }
        
        public void UpdateMoney(int newValue)
        {
            MoneyValue = newValue;
        }
        
        public bool HasEnoughCurrency(CurrencyData currencyData)
        {
            switch (currencyData.CurrencyType)
            {
                case CurrencyType.Coin:
                    return currencyData.CurrencyValue <= MoneyValue;
                default:
                    return false;
            }
        }
    }
}