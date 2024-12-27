using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Models.DTO;
using StudentManagement.API.Repositories;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }


        //Post: /api/Auth/register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]AddRegisterDTO addRegisterDTO)
        {
            var IdentityUser = new IdentityUser
            {
                UserName = addRegisterDTO.UserName,
                Email = addRegisterDTO.UserName
            };

           var IdentityResult= await userManager.CreateAsync(IdentityUser, addRegisterDTO.Password);

            if (IdentityResult.Succeeded)
            {
                //Add role to user
                if(addRegisterDTO.Roles != null && addRegisterDTO.Roles.Any())
                {
                    IdentityResult= await userManager.AddToRolesAsync(IdentityUser,addRegisterDTO.Roles);

                    if (IdentityResult.Succeeded)
                    {
                        return Ok("User was registered! Please login");
                    }

                }

            }
            return BadRequest("Somthing went Wrong");
        }

        //api/Auth/LoginMethod
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginDTO loginDTO)
        {
           var User= await userManager.FindByEmailAsync(loginDTO.userName);

            if(User != null)
            {
               var CheckPasswordResult= await userManager.CheckPasswordAsync(User,loginDTO.Password);

                if (CheckPasswordResult)
                {
                    //Get Roles for this user
                   var roles= await userManager.GetRolesAsync(User);

                    if (roles != null)
                    {
                        //Create Token
                        var JwtToken= tokenRepository.CreateJWTToken(User,roles.ToList());

                        // mapping to DTO
                        var response = new LoginResponseDTO
                        {
                            JwtToken = JwtToken,
                        };
                        return Ok(response);

                    }
                   
                }
            }
            return BadRequest("UserName or Password is Incorrect!");
        }
    }
}
