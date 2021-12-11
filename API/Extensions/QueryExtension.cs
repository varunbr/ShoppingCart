using System.Linq;

namespace API.Extensions
{
    public static class QueryExtension
    {
        public static IQueryable<T> AddPagination<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            return query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
