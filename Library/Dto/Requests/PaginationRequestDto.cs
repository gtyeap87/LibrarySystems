namespace Library.Dto.Requests
{
    public record PaginationRequestDto(int PageNumber = 1, int PageSize = 10);
}