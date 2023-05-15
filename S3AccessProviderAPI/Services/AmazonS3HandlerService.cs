using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Dependency;
using S3AccessProviderAPI.Contracts;
using S3AccessProviderAPI.Models;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace S3AccessProviderAPI.Services
{
    public class AmazonS3HandlerService : IS3HandlerService
    {
        IAmazonS3 client;
        string bucketName;
        string accessKeyValue;
        string secretKeyValue;
        readonly double timeoutDuration;

        public AmazonS3HandlerService(IConfiguration configuration)
        {
            timeoutDuration = 600.0/3600.0;

            string section = "AmazonS3Settings";
            string accessKey = "AccessKey";
            string secretKey = "SecretKey";
            string bucketNameKey = "BucketName";

            var configsection = configuration.GetSection(section);
            accessKeyValue = configsection.GetValue<string>(accessKey);
            secretKeyValue = configsection.GetValue<string>(secretKey);
            bucketName = configsection.GetValue<string>(bucketNameKey);

            var credentials = new BasicAWSCredentials(accessKeyValue, secretKeyValue);
            var region = Amazon.RegionEndpoint.EUCentral1;

            client = new AmazonS3Client(credentials, region);
        }

        /*
            Learn more about getting temporary link 
            at https://docs.aws.amazon.com/AmazonS3/latest/userguide/PresignedUrlUploadObject.html
        */
        string IS3HandlerService.GetTemporaryLinkToUploadFile(string objectKey)
        {
            string urlString = string.Empty;

            try
            {
                var request = new GetPreSignedUrlRequest()
                {
                    BucketName = bucketName,
                    Key = objectKey,
                    Expires = DateTime.UtcNow.AddHours(timeoutDuration),
                };
                urlString = client.GetPreSignedURL(request);
            }
            catch (AmazonS3Exception ex) // TODO: implement an exception handling
            {
                Debug.WriteLine($"Error:'{ex.Message}'");
            }

            return urlString;
        }

        /*
            Learn more about getting Amazon S3 Objects 
            at https://docs.aws.amazon.com/AmazonS3/latest/userguide/example_s3_ListObjects_section.html
        */
        async Task<List<Models.File>> IS3HandlerService.GetObjectsListAsync()
        {
            List<Models.File> items = new List<Models.File>();
            
            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = bucketName
                };

                ListObjectsV2Response response;

                do
                {
                    response = await client.ListObjectsV2Async(request);

                    items = response
                        .S3Objects
                        .Select(s3obj => new Models.File()
                            {
                                Key = s3obj.Key,
                                LastModified = s3obj.LastModified
                            }
                        )
                        .ToList();

                    // If the response is truncated, set the request ContinuationToken
                    // from the NextContinuationToken property of the response.
                    request.ContinuationToken = response.NextContinuationToken;
                }
                while (response.IsTruncated);
            }
            catch (AmazonS3Exception ex) // TODO: implement an exception handling
            {
                Debug.WriteLine($"Error encountered on Amazon server. Message:'{ex.Message}' getting list of objects.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error encountered on API. Message:'{ex.Message}'");
            }

            return items;
        }

        /*
            Learn more about getting Amazon S3 Objects 
            at https://stackoverflow.com/questions/44400227/how-to-get-the-url-of-a-file-on-aws-s3-using-aws-sdk
        */
        string IS3HandlerService.GetPermanentLink(string objectKey)
        {
            string aws = "s3.amazonaws.com";
            string protocol = "https";
            return $"{protocol}://{bucketName}.{aws}/{objectKey}"; //https://bucket.s3.amazonaws.com/key
        }

        
    }
}
