namespace Library.Api.Dto;

public record MemberDto(string Name, DateOnly JoinedDate);

public record CsvMemberDto(
    Guid LibraryId,
    string Name,
    DateOnly JoinedDate
    );