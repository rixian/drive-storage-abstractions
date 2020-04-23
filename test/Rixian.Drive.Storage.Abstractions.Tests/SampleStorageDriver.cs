// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rixian.Drive.Storage.Abstractions;
using Rixian.Extensions.Errors;
using Xunit;
using Xunit.Abstractions;

public class SampleStorageDriver : IStorageDriver
{
    public string DriverVersion => "1.0";

    public Task<Result> DeleteAsync(DeleteOperationParameters parameters, CancellationToken cancellationToken)
    {
        if (parameters is null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var path = GetLocalPath(parameters.TenantId, parameters.PartitionId, parameters.FileId, parameters.StreamName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        return Task.FromResult(Result.Default);
    }

    public Task<Result<DriveFile>> DownloadAsync(DownloadOperationParameters parameters, CancellationToken cancellationToken)
    {
        if (parameters is null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var path = GetLocalPath(parameters.TenantId, parameters.PartitionId, parameters.FileId, parameters.StreamName);
        var metadataPath = $"{path}-metadata";
        var fileInfo = new FileInfo(path);

        DriveFileMetadata? metadata = null;
        if (File.Exists(metadataPath))
        {
            var metadataFileJson = File.ReadAllText(metadataPath);
            metadata = JsonConvert.DeserializeObject<DriveFileMetadata>(metadataFileJson);
        }

        return Task.FromResult<Result<DriveFile>>(new DriveFile
        {
            Data = fileInfo.OpenRead(),
            Metadata = metadata,
        });
    }

    public Task<Result<bool>> ExistsAsync(ExistsOperationParameters parameters, CancellationToken cancellationToken)
    {
        if (parameters is null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var path = GetLocalPath(parameters.TenantId, parameters.PartitionId, parameters.FileId, parameters.StreamName);
        return Task.FromResult<Result<bool>>(File.Exists(path));
    }

    public Task<Result<ICollection<string>>> ListStreamsAsync(ListStreamsOperationParameters parameters, CancellationToken cancellationToken)
    {
        if (parameters is null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var path = GetLocalPath(parameters.TenantId, parameters.PartitionId, parameters.FileId, null);
        var files = Directory.GetFiles(path, "*.*");
        return Task.FromResult<Result<ICollection<string>>>(files);
    }

    public Task<Result> UpgradePartitionAsync(Guid tenantId, Guid volumeId, Guid partitionId, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Default);
    }

    public Task<Result> UploadAsync(UploadOperationParameters parameters, CancellationToken cancellationToken)
    {
        if (parameters is null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        if (parameters.Data == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var path = GetLocalPath(parameters.TenantId, parameters.PartitionId, parameters.FileId, parameters.StreamName);

        using (var fs = new FileStream(path, FileMode.OpenOrCreate))
        {
            parameters.Data.CopyTo(fs);
        }

        if (parameters.FileMetadata != null)
        {
            var metadataFileJson = JsonConvert.SerializeObject(parameters.FileMetadata);
            File.WriteAllText($"{path}-metadata", metadataFileJson);
        }

        return Task.FromResult(Result.Default);
    }

    private static string GetLocalPath(Guid tenantId, Guid partitionId, Guid fileId, string? streamName)
    {
        Directory.CreateDirectory($"{tenantId}/{partitionId}/{fileId}");
        if (string.IsNullOrWhiteSpace(streamName))
        {
            return $"{tenantId}/{partitionId}/{fileId}";
        }
        else
        {
            return $"{tenantId}/{partitionId}/{fileId}/{streamName}";
        }
    }
}
