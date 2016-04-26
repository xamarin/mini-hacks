using System;
using System.Threading.Tasks;
using Particle;
namespace EvolveApp.ViewModels
{
	public class LoginViewModel : BaseViewModel
	{
		public LoginViewModel()
		{
		}

		public async Task<bool> HandleLoginAsync(string username, string password, string token)
		{
			IsBusy = true;

			await ParticleCloud.SharedInstance.CreateOAuthClientAsync(token, "xamarin");
			var response = await ParticleCloud.SharedInstance.LoginWithUserAsync(username, password);

			IsBusy = false;

			return response;
		}
	}
}