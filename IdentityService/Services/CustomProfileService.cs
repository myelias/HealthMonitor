using System.Runtime.InteropServices;
using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService;

public class CustomProfileService : IProfileService
{
    // We need the user Manager to get a hold of the user's subjectId
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomProfileService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    // Adding name and username claims
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        var existingClaims = await _userManager.GetClaimsAsync(user);

        var claims = new List<Claim>
        {
            new Claim("username", user.UserName),
        };

        context.IssuedClaims.AddRange(claims);
        context.IssuedClaims.Add(existingClaims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name));
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}