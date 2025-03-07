namespace SurveyBasket.Api.Services;

public class RoleService(RoleManager<ApplicationRole> roleManager, ApplicationDbContext context) : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<RoleResponse>> GetAllAsync(bool includeDisabled = false, CancellationToken cancellationToken = default) =>
        await _roleManager.Roles
        .Where(x => !x.IsDefault && (!x.IsDeleted || includeDisabled))
        .ProjectToType<RoleResponse>()
        .ToListAsync(cancellationToken);

    public async Task<Result<RoleDetailResponse>> GetAsync(string id)
    {
        if (await _roleManager.FindByIdAsync(id) is not { } role)
            return Result.Failure<RoleDetailResponse>(RoleErrors.RoleNotFound);
        var permissions = await _roleManager.GetClaimsAsync(role);

        var response = new RoleDetailResponse(role.Id, role.Name!, role.IsDeleted, permissions.Select(x => x.Value));
        return Result.Success(response);
    }

    public async Task<Result<RoleDetailResponse>> AddAsync(RoleRequest request, CancellationToken cancellationToken)
    {
        var roleIsExists = await _roleManager.RoleExistsAsync(request.Name);
        if (roleIsExists)
            return Result.Failure<RoleDetailResponse>(RoleErrors.DuplicatedRole);
        var allowedPermissions = Permissions.GetAllPermissions();
        if (request.Premissions.Except(allowedPermissions).Any())
            return Result.Failure<RoleDetailResponse>(RoleErrors.InvalidPermissions);
        var role = new ApplicationRole
        {
            Name = request.Name,
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };
        var result = await _roleManager.CreateAsync(role);

        if (result.Succeeded)
        {
            var permissions = request.Premissions
                .Select(x => new IdentityRoleClaim<string>
                {
                    ClaimType = Permissions.Type,
                    ClaimValue = x,
                    RoleId = role.Id
                });
            await _context.AddRangeAsync(permissions);
            await _context.SaveChangesAsync();

            var response = new RoleDetailResponse(role.Id, role.Name, role.IsDeleted, request.Premissions);
            return Result.Success(response);
        }
        var error = result.Errors.First();
        return Result.Failure<RoleDetailResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }


    public async Task<Result> UpdateAsync(string id, RoleRequest request, CancellationToken cancellationToken)
    {
        var roleIsExists = await _roleManager.Roles.AnyAsync(x => x.Name == request.Name && x.Id != id);
        if (roleIsExists)
            return Result.Failure<RoleDetailResponse>(RoleErrors.DuplicatedRole);
        if (await _roleManager.FindByIdAsync(id) is not { } role)
            return Result.Failure<RoleDetailResponse>(RoleErrors.RoleNotFound);

        var allowedPermissions = Permissions.GetAllPermissions();
        if (request.Premissions.Except(allowedPermissions).Any())
            return Result.Failure<RoleDetailResponse>(RoleErrors.InvalidPermissions);

        role.Name = request.Name;
        var result = await _roleManager.UpdateAsync(role);

        if (result.Succeeded)
        {
            var cuurentPermissions = await _context.RoleClaims
                .Where(x => x.RoleId == id && x.ClaimType == Permissions.Type)
                .Select(x => x.ClaimValue)
                .ToListAsync();

            var newPermissions = request.Premissions.Except(cuurentPermissions)
                .Select(x => new IdentityRoleClaim<string>
                {
                    ClaimType = Permissions.Type,
                    ClaimValue = x,
                    RoleId = role.Id
                });
            var removePermissions = cuurentPermissions.Except(request.Premissions);

            await _context.RoleClaims
               .Where(x => x.RoleId == id && removePermissions.Contains(x.ClaimValue))
               .ExecuteDeleteAsync();
            await _context.AddRangeAsync(newPermissions);
            await _context.SaveChangesAsync();


            return Result.Success();
        }
        var error = result.Errors.First();
        return Result.Failure<RoleDetailResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }



    public async Task<Result> ToggleStatusAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role is null)
            return Result.Failure(RoleErrors.RoleNotFound);
        role.IsDeleted = !role.IsDeleted;
        await _roleManager.UpdateAsync(role);
        return Result.Success();



    }

}
