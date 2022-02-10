namespace API.Helpers
{
    public class Type
    {
        public const string Integer = "Integer";
        public const string String = "String";
    }

    public class PayType
    {
        public const string Wallet = "Wallet";
        public const string InternetBanking = "Internet Banking";
        public const string DebitCard = "Debit Card";
        public const string Upi = "UPI";
    }

    public class Constants
    {
        public const string OrderBy = "OrderBy";
        public const string Price = "Price";
        public const string Brand = "Brand";
        public const string Category = "Category";
        public const string ShoppingCartWallet = "ShoppingCart Wallet";
    }

    public class OrderBy
    {
        public const string HighToLow = "HighToLow";
        public const string LowToHigh = "LowToHigh";
        public const string Latest = "Latest";
        public const string Default = "Default";
    }

    public class Status
    {
        public const string Created = "Created";
        public const string Confirmed = "Confirmed";
        public const string Dispatched = "Dispatched";
        public const string Delivered = "Delivered";
        public const string Ordered = "Ordered";
        public const string Failed = "Failed";
        public const string Success = "Success";
    }
}
