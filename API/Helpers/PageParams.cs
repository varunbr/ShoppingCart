using System.Collections.Generic;

namespace API.Helpers
{
    public class PageParams
    {
        private const int MaxSize = 25;

        private int _pageNumber = 1;
        public int PageNumber
        {
            get=>_pageNumber;
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

        public PageParams(){}

        public PageParams(Dictionary<string, string> queryParams)
        {
            PageNumber = GetValue(queryParams, nameof(PageNumber));
            PageSize = GetValue(queryParams, nameof(PageSize));
        }

        private int GetValue(Dictionary<string, string> queryParams, string key)
        {
            queryParams.TryGetValue(key, out var value);
            int.TryParse(value, out var intValue);
            return intValue;
        }
    }
}
