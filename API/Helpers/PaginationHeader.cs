namespace API.Helpers
{
    public class PaginationHeader
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        public PaginationHeader(int pageNumber, int pageSize, int totalPages, int totalCount)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
            TotalCount = totalCount;
        }
    }
}
