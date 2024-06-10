using BreadCutter.Data;
using BreadCutter.Utils;

namespace BreadCutter.Models
{
    public class CurrencyModel
    {
        public float MoneyValue { get; private set; }
        
        public void UpdateMoney(float newValue)
        {
            MoneyValue = newValue;
        }
        
        public bool HasEnoughCurrency(CurrencyData currencyData)
        {
            switch (currencyData.CurrencyType)
            {
                case CurrencyType.Money:
                    return currencyData.CurrencyValue <= MoneyValue;
                default:
                    return false;
            }
        }
    }
}