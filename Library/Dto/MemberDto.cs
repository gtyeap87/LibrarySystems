using Library.Model;

namespace RestWebApi.Dto;

/// <summary>
/// Represents weather forecast data with support for both v1 and v2 API features
/// </summary>
public class MemberDto
{
    public required string Name { get; set; }
    public DateOnly JoinedDate { get; set; }
}