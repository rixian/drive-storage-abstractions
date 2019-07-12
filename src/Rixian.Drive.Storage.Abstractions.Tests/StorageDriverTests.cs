// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Rixian.Drive.Storage.Abstractions;
using Xunit;
using Xunit.Abstractions;

public class StorageDriverTests
{
    private readonly ITestOutputHelper logger;

    public StorageDriverTests(ITestOutputHelper logger)
    {
        this.logger = logger;
    }

    [Fact]
    public async Task StorageDriverSampleTest()
    {
        var tenantId = Guid.NewGuid();
        var partitionId = Guid.NewGuid();
        var fileId = Guid.NewGuid();
        var streamName = "default";
        var fileName = "test.txt";
        var contentType = "text/plain";
        var text = "This is a test!";
        var sampleDriver = new SampleStorageDriver();

        var data = Encoding.UTF8.GetBytes(text);
        using (var ms = new MemoryStream(data))
        {
            await sampleDriver.UploadAsync(tenantId, partitionId, fileId, streamName, ms, new DriveFileMetadata
            {
                ContentType = contentType,
                FileName = fileName,
            }).ConfigureAwait(false);
        }

        DriveFile file = await sampleDriver.DownloadAsync(tenantId, partitionId, fileId, streamName).ConfigureAwait(false);

        file.Should().NotBeNull();
        file.Data.Should().NotBeNull();
        file.Metadata.Should().NotBeNull();
        file.Metadata.FileName.Should().Be(fileName);
        file.Metadata.ContentType.Should().Be(contentType);

        using (var ms = new MemoryStream())
        using (file.Data)
        {
            file.Data.CopyTo(ms);
            ms.Position = 0;
            var readData = ms.ToArray();
            var readText = Encoding.UTF8.GetString(readData);
            readText.Should().Be(text);
        }

        await sampleDriver.DeleteAsync(tenantId, partitionId, fileId, streamName).ConfigureAwait(false);
        Directory.Delete(tenantId.ToString(), true);
    }
}
