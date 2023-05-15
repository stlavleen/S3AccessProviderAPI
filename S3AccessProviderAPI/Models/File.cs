using System.ComponentModel.DataAnnotations;

namespace S3AccessProviderAPI.Models
{
    public class File 
    {
        [Key]
        public string Key { get; set; }
        public DateTime? LastModified { get; set; }
    }

}
