namespace Identity.Configurations;

using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using Identity.Models;
using Microsoft.AspNetCore.Identity;

public class UserValidator : IResourceOwnerPasswordValidator
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UserValidator(SignInManager<User> signInManager,
        UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var user = await _userManager.FindByNameAsync(context.UserName);

        var signIn = await _signInManager.PasswordSignInAsync(
            user,
            context.Password,
            isPersistent: true,
            lockoutOnFailure: true);

        if (signIn.Succeeded)
        {
            var userId = user!.Id.ToString();

            // context set to success
            context.Result = new GrantValidationResult(
                subject: userId,
                authenticationMethod: "custom",
                claims: new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, user.UserName)
                }
            );

            return;
        }

        // context set to Failure
        context.Result = new GrantValidationResult(
            TokenRequestErrors.UnauthorizedClient, "Invalid Credentials");
    }
}
