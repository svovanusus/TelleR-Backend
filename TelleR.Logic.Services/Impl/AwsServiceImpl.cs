using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Threading.Tasks;
using TelleR.Data.Dto;

namespace TelleR.Logic.Services.Impl
{
    public class AwsServiceImpl : IAwsService
    {
        private readonly IConfiguration _configuration;

        public AwsServiceImpl(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<String> SaveFileToAws(FileDto file)
        {
            var keyId = _configuration.GetSection("AWS").GetValue<String>("AWSAccessKeyId");
            var keySecret = _configuration.GetSection("AWS").GetValue<String>("AWSSecretAccessKey");

            var config = new Amazon.S3.AmazonS3Config();
            config.RegionEndpoint = Amazon.RegionEndpoint.EUWest3;

            var client = new Amazon.S3.AmazonS3Client(keyId, keySecret, config);

            var fileName = $"users-uploads/{Guid.NewGuid().ToString()}-{file.FileName}";

            var request = new Amazon.S3.Model.PutObjectRequest
            {
                BucketName = "teller-uploads",
                ContentType = file.ContentType,
                InputStream = file.ReadStream,
                Key = fileName,
                StorageClass = Amazon.S3.S3StorageClass.Standard,
                CannedACL = Amazon.S3.S3CannedACL.PublicRead,
            };

            var response = await client.PutObjectAsync(request);
            if (response.HttpStatusCode == HttpStatusCode.OK) return "https://teller-uploads.s3.amazonaws.com/" + fileName;
            else return null;
        }
    }
}
