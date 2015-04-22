namespace NoiseCalculator.Domain.Entities
{
    public class User
    {
        public string Shortname { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public bool IsNewIdentity { get; set; }
        public bool IsPersonMailbox { get; set; }
        public bool IsAdmin { get; set; }
    }
}
