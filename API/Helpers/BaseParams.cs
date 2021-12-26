namespace API.Helpers
{
    public class BaseParams
    {
        private const int MaxSize = 25;

        private int _pageNumber = 1;
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value <= 0 ? 1 : value;
        }

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set
            {
                _pageSize = value switch
                {
                    > MaxSize => MaxSize,
                    <= 0 => 10,
                    _ => value
                };
            }
        }
        public string OrderBy { get; set; }
    }
}
