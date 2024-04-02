namespace MatrixWebAPI.Models.DTO
{
    public class UserBasicDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public string Email { get; set; } = null!;
    }
}
