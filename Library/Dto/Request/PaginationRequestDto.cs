namespace Library.Dto.Request
{
    public record PaginationRequestDto(int PageNumber = 1, int PageSize = 10);
}