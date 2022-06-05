namespace Application.Common.Pagination;

public class PagedList<T> : List<T>
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(items);
    }

    public void AddPaginationHeaders(IHttpContextService contextService)
    {
        var path = contextService.GetPath();
        var links = new Dictionary<string, string>();

        int previous = HasPrevious ? CurrentPage - 1 : CurrentPage;
        int next = HasNext ? CurrentPage + 1 : CurrentPage;

        links.Add("First",$"{path}?pagesize={PageSize}&pagenumber=1");
        links.Add("Last", $"{path}?pagesize={PageSize}&pagenumber={TotalPages}");
        links.Add("Previous", $"{path}?pagesize={PageSize}&pagenumber={previous}");
        links.Add("Next", $"{path}?pagesize={PageSize}&pagenumber={next}");

        var metadata = new
        {
            TotalCount,
            PageSize,
            CurrentPage,
            TotalPages,
            HasNext,
            HasPrevious,
            Links = links
        };
        contextService.AddHeaderValue("X-Pagination", metadata);
    }
}
