namespace LyricDb.Web.Models.Dto.Responses;

public class PagedResponseBase<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPrevious => Page > 0;
    public bool HasNext => Page < TotalPages - 1;
    public string? PreviousPage => HasPrevious ? $"?page={Page - 1}&pageSize={PageSize}" : null;
    public string? NextPage => HasNext ? $"?page={Page + 1}&pageSize={PageSize}" : null;
    public IEnumerable<T> Items { get; set; } = Array.Empty<T>();
}