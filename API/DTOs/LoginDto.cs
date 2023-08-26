//DTO - data transfer object - object that caries data between processes
namespace API.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}