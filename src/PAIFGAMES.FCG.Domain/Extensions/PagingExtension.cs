using Microsoft.EntityFrameworkCore;

namespace PAIFGAMES.FCG.Domain.Extensions
{
    public static class PagingExtension
    {
        public static PagedList<TModel> Paging<TModel>(this IQueryable<TModel> query, int pageSize = 10, int pageNumber = 1) where TModel : class
        {
            var count = query.Count();
            var items = pageSize > 0 && pageNumber > 0 ? query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList() : query.ToList();
            return new PagedList<TModel>(items, count, pageNumber, pageSize);
        }

        public async static Task<PagedList<TModel>> PagingAsync<TModel>(this IQueryable<TModel> query, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = default) where TModel : class
        {
            var count = await query.CountAsync(cancellationToken);
            var items = pageSize > 0 && pageNumber > 0 ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken) : await query.ToListAsync(cancellationToken);
            return new PagedList<TModel>(items, count, pageNumber, pageSize);
        }
    }

    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => TotalPages > CurrentPage;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize != 0 ? pageSize : count;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling((double)count / (pageSize != 0 ? pageSize : count));
            AddRange(items);
        }
    }
    public class PageFilterModel
    {
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 25;
    }
}
