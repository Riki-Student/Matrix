namespace MatrixWebAPI.Models.DTO
{
    public class AccountBasicDto
    {
        public int Id { get; set; }

        public string CompanyName { get; set; } = null!;

        public string? Website { get; set; }
    }
}
