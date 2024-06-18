namespace BarberCo.SharedLibrary.Dtos
{
    /// <summary>
    /// this dto is used for registering a new barber
    /// </summary>
    public class BarberRegistrationDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
