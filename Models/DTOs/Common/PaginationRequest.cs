namespace ApiApplication.Models.DTOs.Common
{
    public class PaginationRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; } = "desc"; // asc, desc
        public string? Search { get; set; }
    }

    public class PaginationResponse<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount => TotalItems;
        public int CurrentPage => Page;
        public bool HasNextPage => HasNext;
        public bool HasPreviousPage => HasPrevious;
    }
}
