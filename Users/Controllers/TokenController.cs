using Microsoft.AspNetCore.Mvc;

namespace HelloWorldService.Controllers
{
  public class TokenRequest
  {
    public string Email { get; set; }
    public string Password { get; set; }
  }

  [Route("api/[controller]")]
  [ApiController]
  public class TokenController : ControllerBase
  {
    // This should require SSL
    [HttpPost]
    public dynamic Post([FromBody] TokenRequest tokenRequest)
    {
      var token = TokenHelper.GetToken(tokenRequest.Email, tokenRequest.Password);
      return new { Token = token };
    }

    // This should require SSL
    [HttpGet("{Email}/{Password}")]
    public dynamic Get(string Email, string Password)
    {
      var token = TokenHelper.GetToken(Email, Password);
      return new { Token = token };
    }
  }
}