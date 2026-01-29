namespace Library.Dto;

public class MemberDto
{
    public required string Name { get; set; }
    public DateOnly JoinedDate { get; set; }
}

public record CsvMemberDto(
    Guid LibraryId,
    string Name,
    DateOnly JoinedDate
    );