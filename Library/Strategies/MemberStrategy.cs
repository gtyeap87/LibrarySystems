using Library.Data.Identity;
using Library.Dto.Request;
using Library.Model;
using Library.Service;

namespace Library.Strategies
{
    public class NormalMember : IMemberStrategy
    {
        public bool CanHandle(string role) => role != Roles.Member;

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
        private readonly ILibraryService _libraryService = libraryService;
        private readonly ILogger<PremiumMember> _logger = logger;
        private readonly IConfiguration _configuration = configuration;

        public bool CanHandle(string role) => role == Roles.Member;

        public async Task<Guid> AddMemberAsync(RegisterUserRequest request)
        {
            try
            {
                _logger.LogInformation("Add new premium member to library");

                var memberRequest = new MemberRequest
                {
                    Member = new Member()
                    {
                        Name = $"{request.FirstName} {request.LastName}",
                        JoinedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                        LibraryId = Guid.Parse(_configuration["Library:Id"]!)
                    }
                };
                var newId = await _libraryService.CreateMemberAsync(memberRequest);

                if (_logger.IsEnabled(LogLevel.Information))
                    _logger.LogInformation("Added new premium member name {Name} with ID: {Id}", $"{request.FirstName}{request.LastName}", newId);

                return newId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding new premium member");
                return Guid.Empty;
            }
        }
    }
}