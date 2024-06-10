using BreadCutter.Utils;

namespace BreadCutter.Data
{
    public struct CurrencyData
    {
        public CurrencyType CurrencyType;
        public float CurrencyValue;

        public CurrencyData(CurrencyType currencyType, float currencyValue)
        {
            CurrencyType = currencyType;
            CurrencyValue = currencyValue;
        }
    }
}