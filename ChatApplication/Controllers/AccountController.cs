using ChatApplication.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChatApplication
{
    [Route("/account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController
            (UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Route("getCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user =  await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            var newUser = new ApplicationUser
            {
                Email = userRegisterDto.Email,
                UserName = userRegisterDto.Email,
                FirstName = userRegisterDto.FirstName,
                LastName = userRegisterDto.LastName
            };

            var result = await _userManager.CreateAsync(newUser, userRegisterDto.Password);
            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                await _userManager.ConfirmEmailAsync(newUser, token);

                return Ok();
            }

            return NotFound();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var foundUser = await _userManager.FindByEmailAsync(userLoginDto.Email);
            if (foundUser == null) 
            { 
                return NotFound(); 
            }



            var result = await _signInManager.PasswordSignInAsync(foundUser, userLoginDto.Password, true, false);
            if (result.Succeeded)
            {
                return Ok();
            }

            return NotFound();
        }
    }
}
