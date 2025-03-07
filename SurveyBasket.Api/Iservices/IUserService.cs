namespace SurveyBasket.Api.Iservices;

public interface IUserService
{
    Task<Result<UserProfileResponse>> GetProfilrAsync(string userId);
    Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    Task<Result> ChangePassword(string userId, changePasswordRequest request);
    Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<UserResponse>> GetAsync(string id, CancellationToken cancellationToken);
    Task<Result<UserResponse>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatus(string id);
    Task<Result> UnLock(string id);
}
