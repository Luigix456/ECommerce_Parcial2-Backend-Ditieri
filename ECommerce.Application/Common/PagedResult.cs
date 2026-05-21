namespace ECommerce.Application.Common;

public class PagedResult<T>
{
    public List<T> Items { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }
    public int CurrentPage { get; }
    public int PageSize { get; }

    public PagedResult(List<T> items, int totalCount, int currentPage, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}
