using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MindHeal.Data;
using MindHeal.Interfaces.IRepositories;
using MindHeal.Models.Entities;

namespace MindHeal.Helper
{
    public class UserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
    {
        private ApplicationDbContext _appliationContext;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        public UserClaimsPrincipalFactory(
        UserManager<User> userManager,
        IOptions<IdentityOptions> optionsAccessor, ApplicationDbContext applicationContext, IUserRepository userRepository, IRoleRepository roleRepository)
            : base(userManager, optionsAccessor)
        {
            _appliationContext = applicationContext;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
    }
}
