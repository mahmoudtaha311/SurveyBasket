namespace SurveyBasket.Api.Abstractions;

public class PaginatedList<T>(List<T> items, int pageNumber, int count, int pageSize)
{
    public List<T> Items { get; set; } = items;
    public int PageNumber { get; set; } = pageNumber;

    public int TotalPages { get; set; } = (int)Math.Ceiling(count / (double)pageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public int PageSize { get; set; }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);

        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PaginatedList<T>(items, pageNumber, count, pageSize);
    }
}
