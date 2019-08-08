using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultationManagmentAPI.Models.Entity
{
    public class ApplicationUserManger : UserManager<ApplicationUser>
    {
        public ApplicationUserManger(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            optionsAccessor.Value.Password.RequiredLength = 4;
            optionsAccessor.Value.Password.RequireNonAlphanumeric = false;
            optionsAccessor.Value.Password.RequireLowercase = false;
            optionsAccessor.Value.Password.RequireUppercase = false;
            optionsAccessor.Value.Password.RequireDigit = false;
        }
    }
}
