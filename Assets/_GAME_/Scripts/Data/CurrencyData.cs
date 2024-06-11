using BreadCutter.Utils;

namespace BreadCutter.Data
{
    public struct CurrencyData
    {
        public CurrencyType CurrencyType;
        public int CurrencyValue;

        public CurrencyData(CurrencyType currencyType, int currencyValue)
        {
            CurrencyType = currencyType;
            CurrencyValue = currencyValue;
        }
    }
}