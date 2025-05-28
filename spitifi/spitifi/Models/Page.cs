using Microsoft.EntityFrameworkCore;

namespace spitifi.Models;

public class Page<T> : List<T>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }

    public Page(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageSize = pageSize;
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        this.AddRange(items);
    }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public static async Task<Page<T>> CreateAsync(
        IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return new Page<T>(items, count, pageIndex, pageSize);
    }
}