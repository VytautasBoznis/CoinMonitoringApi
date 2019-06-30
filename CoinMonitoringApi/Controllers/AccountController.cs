using System.Web.Http;
using CoinMonitoringApi.Interfaces.Account;
using CoinMonitoringPortalApi.Data.Messages;
using CoinMonitoringPortalApi.Data.Messages.Account;

namespace CoinMonitoringApi.Controllers
{
    public class AccountController : ApiController
    {
	    private readonly IAccountFacade _accountFacade;

	    public AccountController(IAccountFacade accountFacade)
	    {
		    _accountFacade = accountFacade;
	    }
		
		[Route("Account/Login")]
		[HttpPost]
	    public LoginResponse Login([FromBody] LoginRequest request)
		{
			LoginResponse response = _accountFacade.Login(request);
			return response;
	    }

		[Route("Account/Register")]
	    [HttpPost]
	    public RegisterResponse Register([FromBody] RegisterRequest request)
	    {
		    RegisterResponse response = _accountFacade.Register(request);
		    return response;
	    }

	    [Route("Account/UpdateUser")]
	    [HttpPost]
	    public UpdateUserResponse UpdateUser([FromBody] UpdateUserRequest request)
	    {
		    UpdateUserResponse response = _accountFacade.UpdateUser(request);
		    return response;
	    }

	    [Route("Account/ChangePassword")]
	    [HttpPost]
	    public ChangeUserPasswordResponse ChangePassword([FromBody] ChangeUserPasswordRequest request)
	    {
		    ChangeUserPasswordResponse response = _accountFacade.ChangePassword(request);
		    return response;
	    }

	    [Route("Account/GetAuthKeys")]
	    [HttpPost]
	    public AuthKeysResponse GetAuthKeys([FromBody] AuthKeysRequest request)
	    {
		    AuthKeysResponse response = _accountFacade.GetAuthKeys(request);
		    return response;
	    }

	    [Route("Account/CreateAuthKey")]
	    [HttpPost]
	    public CreateAuthKeyResponse CreateAuthKey([FromBody] CreateAuthKeyRequest request)
	    {
		    CreateAuthKeyResponse response = _accountFacade.CreateAuthKey(request);
		    return response;
	    }

	    [Route("Account/DeleteAuthKey")]
	    [HttpPost]
	    public DeleteAuthKeyResponse DeleteAuthKey([FromBody] DeleteAuthKeyRequest request)
	    {
		    DeleteAuthKeyResponse response = _accountFacade.DeleteAuthKey(request);
		    return response;
	    }
	}
}
