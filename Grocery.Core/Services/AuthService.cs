using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IClientService _clientService;
        private Client? _currentUser;

        public Client? CurrentUser => _currentUser; 
        public AuthService(IClientService clientService)
        {
            _clientService = clientService;
        }

        public Client? Login(string email, string password)
        {
            Client? client = _clientService.Get(email);
            if (client == null) return null;

            if (PasswordHelper.VerifyPassword(password, client.Password))
            {
                _currentUser = client; 
                return client;
            }

            return null;
        }
    }
}
