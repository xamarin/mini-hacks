using System;
using Particle.Models;

namespace Particle
{
	public class ParticleAccessToken
	{
		public ParticleAccessToken(string accessToken, string refreshToken, DateTime expiresAt) {
			token = accessToken;
			RefreshToken = refreshToken;
			Expiration = expiresAt;
		}

		public ParticleAccessToken (ParticleLoginResponse response){
			token = response.AccessToken;
			TokenType = response.Type;
			Expiration = DateTime.Now.AddSeconds (response.Expiration);
			RefreshToken = response.RefreshToken;
		}

		string token;
		public string Token { 
			get { 
				if (DateTime.Now < Expiration)
					return token;
				else {
					return "expired";
				}
			} 
		}

		public string TokenType { get; internal set; }
		public DateTime Expiration { get; internal set; }
		public string RefreshToken { get; internal set; }
	}
}