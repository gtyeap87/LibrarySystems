using Library.Dto.Request;
using Library.Model;
using Library.Service;

namespace Library.Strategies
{
    public class NormalMember : IMemberStrategy
    {
        public async Task<Guid> AddMemberAsync(RegisterUserRequest request)
        {
            // normal member logic
            var emptyGuid = Guid.Empty;
            return await Task.FromResult(emptyGuid);
        }
    }

    public class PremiumMember(
        ILibraryService libraryService,
        ILogger<PremiumMember> logger,
        IConfiguration configuration
        ) : IMemberStrategy
    {
        public async Task<Guid> AddMemberAsync(RegisterUserRequest request)
        {
            try
            {
                logger.LogInformation("Add new premium member to library");

                var memberRequest = new MemberRequest(
                  new Member()
                  {
                      Name = $"{request.FirstName} {request.LastName}",
                      JoinedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                      LibraryId = Guid.Parse(configuration["Library:Id"]!)
                  }
                );
                var newId = await libraryService.CreateMemberAsync(memberRequest);

                if (logger.IsEnabled(LogLevel.Information))
                {
                    var name = $"{request.FirstName}{request.LastName}".Replace(Environment.NewLine, string.Empty);
                    logger.LogInformation("Added new premium member name {Name} with ID: {Id}", name, newId);
                }

                return newId;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding new premium member");
                return Guid.Empty;
            }
        }
    }
}