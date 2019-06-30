using CoinMonitoringPortalApi.Data.Messages.Account;

namespace CoinMonitoringApi.Interfaces.Account
{
	public interface IAccountFacade
	{
		LoginResponse Login(LoginRequest request);
		RegisterResponse Register(RegisterRequest request);
		UpdateUserResponse UpdateUser(UpdateUserRequest request);
		ChangeUserPasswordResponse ChangePassword(ChangeUserPasswordRequest request);
		AuthKeysResponse GetAuthKeys(AuthKeysRequest request);
		CreateAuthKeyResponse CreateAuthKey(CreateAuthKeyRequest request);
		DeleteAuthKeyResponse DeleteAuthKey(DeleteAuthKeyRequest request);
	}
}
