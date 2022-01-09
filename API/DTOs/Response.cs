using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;

namespace API.DTOs
{
    public class Response<T, TC> where TC : BaseParams
    {
        public TC Context { get; }
        public List<T> Items { get; }

        private Response(List<T> items, TC context)
        {
            Items = items;
            Context = context;
        }

        public static Response<T, TC> Create(List<T> result, TC context)
        {
            return new Response<T, TC>(result, context);
        }

        public static async Task<Response<T, TC>> CreateAsync(IQueryable<T> query, TC context)
        {
            context.TotalCount = await query.CountAsync();
            var items = await query.Skip((context.PageNumber - 1) * context.PageSize).Take(context.PageSize).ToListAsync();
            return Create(items, context);
        }
    }
}
