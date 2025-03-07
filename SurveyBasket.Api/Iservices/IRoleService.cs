namespace SurveyBasket.Api.Iservices;

public interface IRoleService
{
    Task<IEnumerable<RoleResponse>> GetAllAsync(bool includeDisabled = false, CancellationToken cancellationToken = default);
    Task<Result<RoleDetailResponse>> GetAsync(string id);
    Task<Result<RoleDetailResponse>> AddAsync(RoleRequest request, CancellationToken cancellationToken);
    Task<Result> UpdateAsync(string id, RoleRequest request, CancellationToken cancellationToken);
    Task<Result> ToggleStatusAsync(string id);
}
