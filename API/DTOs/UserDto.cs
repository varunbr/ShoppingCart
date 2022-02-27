namespace API.DTOs
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
    }

    public class UserInfoDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public bool Exist { get; set; }
    }
}
