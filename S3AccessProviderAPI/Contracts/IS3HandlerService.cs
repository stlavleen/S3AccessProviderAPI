using S3AccessProviderAPI.Models;
using System.Net;

namespace S3AccessProviderAPI.Contracts
{
    public interface IS3HandlerService
    {
        string GetTemporaryLinkToUploadFile(string objectKey);
        Task<List<Models.File>> GetObjectsListAsync();
        string GetPermanentLink(string objectKey);
    }
}
