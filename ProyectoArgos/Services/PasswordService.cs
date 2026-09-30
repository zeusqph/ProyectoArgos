

namespace ProyectoArgos.Services
{
    public class PasswordService
    {
        public string Hashear(string password) =>
            BCrypt.Net.BCrypt.HashPassword(password);   

        public bool Verificar(string password, string hash) =>
            BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
