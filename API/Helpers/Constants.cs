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

    public class TransactionType
    {
        public const string Order = "Order";
        public const string AmountTransfer = "Amount Transfer";
    }

    public class Constants
    {
        public const string TestUser = "test_user";
        public const string Admin = "admin";
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
        public const string Oldest = "Oldest";
        public const string Default = "Default";
    }

    public class Status
    {
        public const string Created = "Created";
        public const string Confirmed = "Confirmed";
        public const string Waiting = "Waiting";
        public const string Arrived = "Arrived";
        public const string Departed = "Departed";
        public const string OutForDelivery = "OutForDelivery";
        public const string AwaitingArrival = "AwaitingArrival";
        public const string AwaitingDeparture = "AwaitingDeparture";
        public const string AwaitingDeliveryStart = "AwaitingDeliveryStart";
        public const string AwaitingDelivery = "AwaitingDelivery";
        public const string Dispatched = "Dispatched";
        public const string Shipped = "Shipped";
        public const string Delivered = "Delivered";
        public const string Ordered = "Ordered";
        public const string Failed = "Failed";
        public const string Success = "Success";
    }
}
