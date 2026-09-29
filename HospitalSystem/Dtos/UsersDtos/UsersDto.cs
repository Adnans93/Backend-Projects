namespace HospitalSystem.Dtos.UsersDtos
{
    public class UsersDto
    {
        public int Id { get; set; }

        public string? UID { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? DoctorName { get; set; }
        public List<string> Roles { get; set; } = new List<string>();

        public string? ImageURL { get; set; }
    }
}