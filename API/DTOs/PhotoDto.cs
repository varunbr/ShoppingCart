using Microsoft.AspNetCore.Http;

namespace API.DTOs
{
    public class PhotoDto
    {
        public string Url { get; set; }
        public bool IsMain { get; set; }
    }
    
    public class PhotoUpdateDto
    {
        public IFormFile File { get; set; }
        public bool Remove { get; set; }
    }
}
