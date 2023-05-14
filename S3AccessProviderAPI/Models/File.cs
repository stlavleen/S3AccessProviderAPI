using System.ComponentModel.DataAnnotations;

namespace S3AccessProviderAPI.Models
{
    //public class FilesAudit
    //{
    //    public long Id { get; set; }

    //    public int StorageId { get; set; } // foreign key
    //    public StorageService? StorageService { get; set; }

    //    public long FileId { get; set; } // foreign key
    //    public File? File { get; set; }

    //    public long BucketId { get; set; } // foreign key
    //    public Bucket? Bucket { get; set; }

    //    //public DateTime FileChangeDateTime { get; set; }


    //}

    //public class StorageService 
    //{
    //    public int Id { get; set; }
    //    public string? Name { get; set; }

    //    //public List<Bucket> Buckets { get; set; } = new()!;
    //}

    //public class Bucket
    //{
    //    public long Id { get; set; }
    //    public string? Name { get; set; }

    //    //public int StorageId { get; set; } // foreign key
    //    //public StorageService? StorageService { get; set; }

    //    //public List<File> Files { get; set; } = new();

    //}

    public class File 
    {
        [Key]
        public string Key { get; set; }
        public DateTime? LastModified { get; set; }

        //public long BucketId { get; set; } // foreign key
        //public Bucket? Bucket { get; set; }
        //public int StorageId { get; set; } // foreign key
        //public StorageService? Storage { get; set; }
    }

}
