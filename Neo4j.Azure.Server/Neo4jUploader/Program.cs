using System;
using System.Diagnostics;
using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Neo4jUploader
{
    class Program
    {
        static void Main(string[] args)
        {
            // if running Azure locally, make sure Azure Storage Emulator has started
            CloudStorageAccount localStorageAccount =
                CloudStorageAccount.Parse(
                    "UseDevelopmentStorage=true");
            CloudBlobClient localClient = localStorageAccount.CreateCloudBlobClient();

            
            CloudStorageAccount cloudStorageAccount =
                CloudStorageAccount.Parse(
                    "DefaultEndpointsProtocol=https;AccountName=neo4j2;AccountKey=TssPCp62gY232dcs8f+RsYn+lXzQyUPt54TTW2kXEM/FGaAJaslKyrG+RxB7kLtBqwqAgntOu01VAjb1/55ENw==");
            CloudBlobClient cloudClient = cloudStorageAccount.CreateCloudBlobClient();

            Console.WriteLine("Please press 1 to upload the binaries to Local Storage, or 2 to Azure Storage.");
            var key = Console.ReadKey().KeyChar;

            if (key == '1')
            {
                using (var info = Process.Start(Environment.ExpandEnvironmentVariables("%ProgramFiles%\\Microsoft SDKs\\Windows Azure\\Emulator\\csrun.exe"), "/devstore:start"))
                {
                    info.WaitForExit();
                }

                localClient.Timeout = TimeSpan.FromMinutes(30);
                CloudBlobContainer container = localClient.GetContainerReference("neo4j");
                container.CreateIfNotExist();
                UploadBlob(container, "jre7.zip", "binaries\\jre7.zip");
                UploadBlob(container, "neo4j-community-1.8.2.zip", "binaries\\neo4j-community-1.8.2.zip");
            }
            else if (key == '2')
            {
                cloudClient.Timeout = TimeSpan.FromMinutes(30);
                CloudBlobContainer container = cloudClient.GetContainerReference("neo4j");
                container.CreateIfNotExist();
                UploadBlob(container, "jre7.zip", "binaries\\jre7.zip");
                UploadBlob(container, "neo4j-community-1.8.2.zip", "binaries\\neo4j-community-1.8.2.zip");
            }
        }
 
        private static void UploadBlob(CloudBlobContainer container, string blobName, string filename)
        {
            CloudBlob blob = container.GetBlobReference(blobName);
 
            using (FileStream fileStream = File.OpenRead(filename))
                blob.UploadFromStream(fileStream);
        }
    }
}
