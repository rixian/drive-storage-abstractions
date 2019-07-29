// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rixian.Drive.Storage.Abstractions;
using Xunit;
using Xunit.Abstractions;

public class SampleStorageDriver : IStorageDriver
{
    public Task DeleteAsync(Guid tenantId, Guid partitionId, Guid fileId, string streamName, string alternateId, CancellationToken cancellationToken = default)
    {
        File.Delete(this.GetLocalPath(tenantId, partitionId, fileId, streamName));
        return Task.CompletedTask;
    }

    public Task<DriveFile> DownloadAsync(Guid tenantId, Guid partitionId, Guid fileId, string streamName, string alternateId, CancellationToken cancellationToken = default)
    {
        var path = this.GetLocalPath(tenantId, partitionId, fileId, streamName);
        var metadataPath = $"{path}-metadata";
        var fileInfo = new FileInfo(path);

        DriveFileMetadata metadata = null;
        if (File.Exists(metadataPath))
        {
            var metadataFileJson = File.ReadAllText(metadataPath);
            metadata = JsonConvert.DeserializeObject<DriveFileMetadata>(metadataFileJson);
        }

        return Task.FromResult(new DriveFile
        {
            Data = fileInfo.OpenRead(),
            Metadata = metadata,
        });
    }

    public Task UploadAsync(Guid tenantId, Guid partitionId, Guid fileId, string streamName, string alternateId, Stream stream, DriveFileMetadata fileMetadata = null, CancellationToken cancellationToken = default)
    {
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        var path = this.GetLocalPath(tenantId, partitionId, fileId, streamName);
        var metadataPath = $"{path}-metadata";

        using (var fs = new FileStream(path, FileMode.OpenOrCreate))
        {
            stream.CopyTo(fs);
        }

        if (fileMetadata != null)
        {
            var metadataFileJson = JsonConvert.SerializeObject(fileMetadata);
            File.WriteAllText($"{path}-metadata", metadataFileJson);
        }

        return Task.CompletedTask;
    }

    private string GetLocalPath(Guid tenantId, Guid partitionId, Guid fileId, string streamName)
    {
        Directory.CreateDirectory($"{tenantId}/{partitionId}/{fileId}");
        return $"{tenantId}/{partitionId}/{fileId}/{streamName}";
    }
}
