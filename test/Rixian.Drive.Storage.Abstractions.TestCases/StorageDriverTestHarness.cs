// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Drive.Storage.Abstractions.TestCases
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using Rixian.Extensions.Errors;
    using Xunit;
    using Xunit.Abstractions;

    public abstract class StorageDriverTestHarness : TestHarnessBase
    {
        protected StorageDriverTestHarness(ITestOutputHelper logger)
            : base(logger)
        {
        }

        [Fact]
        public async Task TestHarness_UploadAsync_Default_Success()
        {
            // Arrange
            IStorageDriver storageDriver = await this.GetDefaultStorageDriverAsync().ConfigureAwait(false);
            var tenantId = Guid.NewGuid();
            var partitionId = Guid.NewGuid();
            var fileId = Guid.NewGuid();
            string? streamName = null;
            string? alternateId = null;
            var metadata = new DriveFileMetadata
            {
                FileName = "test.txt",
                ContentType = "text/plain",
            };
            var fileContents = "Hello world!";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContents));

            // Act
            await storageDriver.UploadAsync(tenantId, partitionId, fileId, streamName, alternateId, stream, metadata).ConfigureAwait(false);

            // Assert
            DriveFile result = await storageDriver.DownloadAsync(tenantId, partitionId, fileId, streamName, alternateId).ConfigureAwait(false);
            result.Should().NotBeNull();

            result.Data.Should().NotBeNull();
            result.Data?.Length.Should().Be(stream.Length);
            result.Data?.Position.Should().Be(0);
            using (var sr = new StreamReader(result.Data!))
            {
                var contents = sr.ReadToEnd();
                contents.Should().Be(fileContents);
            }

            result.Metadata?.Should().NotBeNull();
            result.Metadata?.FileName.Should().Be(metadata.FileName);
            result.Metadata?.ContentType.Should().Be(metadata.ContentType);

            // Cleanup
            stream?.Dispose();
            await storageDriver.DeleteAsync(tenantId, partitionId, fileId, streamName, alternateId).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestHarness_UploadAsync_NoFileName_Success()
        {
            // Arrange
            IStorageDriver storageDriver = await this.GetDefaultStorageDriverAsync().ConfigureAwait(false);
            var tenantId = Guid.NewGuid();
            var partitionId = Guid.NewGuid();
            var fileId = Guid.NewGuid();
            string? streamName = null;
            string? alternateId = null;
            var metadata = new DriveFileMetadata
            {
                FileName = null,
                ContentType = "text/plain",
            };
            var stream = new MemoryStream();

            // Act
            await storageDriver.UploadAsync(tenantId, partitionId, fileId, streamName, alternateId, stream, metadata).ConfigureAwait(false);

            // Assert
            DriveFile result = await storageDriver.DownloadAsync(tenantId, partitionId, fileId, streamName, alternateId).ConfigureAwait(false);
            result.Should().NotBeNull();

            result.Metadata?.Should().NotBeNull();
            result.Metadata?.FileName.Should().BeNull();
            result.Metadata?.ContentType.Should().Be(metadata.ContentType);

            // Cleanup
            stream?.Dispose();
            await storageDriver.DeleteAsync(tenantId, partitionId, fileId, streamName, alternateId).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestHarness_UploadAsync_NoContentType_Success()
        {
            // Arrange
            IStorageDriver storageDriver = await this.GetDefaultStorageDriverAsync().ConfigureAwait(false);
            var tenantId = Guid.NewGuid();
            var partitionId = Guid.NewGuid();
            var fileId = Guid.NewGuid();
            string? streamName = null;
            string? alternateId = null;
            var metadata = new DriveFileMetadata
            {
                FileName = "test.txt",
                ContentType = null,
            };
            var stream = new MemoryStream();

            // Act
            await storageDriver.UploadAsync(tenantId, partitionId, fileId, streamName, alternateId, stream, metadata).ConfigureAwait(false);

            // Assert
            DriveFile result = await storageDriver.DownloadAsync(tenantId, partitionId, fileId, streamName, alternateId).ConfigureAwait(false);
            result.Should().NotBeNull();

            result.Metadata?.Should().NotBeNull();
            result.Metadata?.FileName.Should().Be(metadata.FileName);
            result.Metadata?.ContentType.Should().Be("application/octet-stream");

            // Cleanup
            stream?.Dispose();
            await storageDriver.DeleteAsync(tenantId, partitionId, fileId, streamName, alternateId).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestHarness_UploadAsync_NoMetadata_Success()
        {
            // Arrange
            IStorageDriver storageDriver = await this.GetDefaultStorageDriverAsync().ConfigureAwait(false);
            var tenantId = Guid.NewGuid();
            var partitionId = Guid.NewGuid();
            var fileId = Guid.NewGuid();
            string? streamName = null;
            DriveFileMetadata? metadata = null;
            var stream = new MemoryStream();
            string? alternateId = null;

            // Act
            await storageDriver.UploadAsync(tenantId, partitionId, fileId, streamName, alternateId, stream, metadata).ConfigureAwait(false);

            // Assert
            DriveFile result = await storageDriver.DownloadAsync(tenantId, partitionId, fileId, streamName, alternateId).ConfigureAwait(false);
            result.Should().NotBeNull();

            result.Metadata?.Should().NotBeNull();
            result.Metadata?.FileName.Should().BeNull();
            result.Metadata?.ContentType.Should().Be("application/octet-stream");

            // Cleanup
            stream?.Dispose();
            await storageDriver.DeleteAsync(tenantId, partitionId, fileId, streamName, alternateId).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("aaaaa")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")] // 64 characters
        public async Task TestHarness_UploadAsync_StreamName_Success(string streamName)
        {
            // Arrange
            IStorageDriver storageDriver = await this.GetDefaultStorageDriverAsync().ConfigureAwait(false);
            var tenantId = Guid.NewGuid();
            var partitionId = Guid.NewGuid();
            var fileId = Guid.NewGuid();
            var metadata = new DriveFileMetadata
            {
                FileName = "test.txt",
                ContentType = "text/plain",
            };
            var fileContents = "Hello world!";
            string? alternateId = null;
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContents));

            // Act
            await storageDriver.UploadAsync(tenantId, partitionId, fileId, streamName, alternateId, stream, metadata).ConfigureAwait(false);

            // Assert
            DriveFile result = await storageDriver.DownloadAsync(tenantId, partitionId, fileId, streamName, alternateId).ConfigureAwait(false);
            result.Should().NotBeNull();

            result.Data.Should().NotBeNull();
            result.Data?.Length.Should().Be(stream.Length);
            result.Data?.Position.Should().Be(0);
            using (var sr = new StreamReader(result.Data!))
            {
                var contents = sr.ReadToEnd();
                contents.Should().Be(fileContents);
            }

            result.Metadata?.Should().NotBeNull();
            result.Metadata?.FileName.Should().Be(metadata.FileName);
            result.Metadata?.ContentType.Should().Be(metadata.ContentType);

            // Cleanup
            stream?.Dispose();
            await storageDriver.DeleteAsync(tenantId, partitionId, fileId, streamName, alternateId).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestHarness_ListStreamsAsync_MultipleStreams_Success()
        {
            // Arrange
            IStorageDriver storageDriver = await this.GetDefaultStorageDriverAsync().ConfigureAwait(false);
            var tenantId = Guid.NewGuid();
            var partitionId = Guid.NewGuid();
            var fileId = Guid.NewGuid();
            var metadata = new DriveFileMetadata
            {
                FileName = "test.txt",
                ContentType = "text/plain",
            };
            var fileContents = "Hello world!";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContents));

            // Act
            await storageDriver.UploadAsync(new UploadOperationParameters
            {
                TenantId = tenantId,
                PartitionId = partitionId,
                FileId = fileId,
                Data = stream,
                FileMetadata = metadata,
            }).ConfigureAwait(false);
            await storageDriver.UploadAsync(new UploadOperationParameters
            {
                TenantId = tenantId,
                PartitionId = partitionId,
                FileId = fileId,
                StreamName = "stream1",
                Data = stream,
                FileMetadata = metadata,
            }).ConfigureAwait(false);
            await storageDriver.UploadAsync(new UploadOperationParameters
            {
                TenantId = tenantId,
                PartitionId = partitionId,
                FileId = fileId,
                StreamName = "stream2",
                Data = stream,
                FileMetadata = metadata,
            }).ConfigureAwait(false);

            // Assert
            Result<ICollection<string>> streamsResult = await storageDriver.ListStreamsAsync(new ListStreamsOperationParameters
            {
                TenantId = tenantId,
                PartitionId = partitionId,
                FileId = fileId,
            }).ConfigureAwait(false);
            ICollection<string> streams = streamsResult.Value;

            streams.Should().NotBeNullOrEmpty();
            streams.Should().Contain(new[] { StreamOperationParameters.DefaultStreamName, "stream1", "stream2" });

            // Cleanup
            await storageDriver.DeleteAsync(new DeleteOperationParameters
            {
                TenantId = tenantId,
                PartitionId = partitionId,
                FileId = fileId,
            }).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestHarness_ListStreamsAsync_SingleStream_Success()
        {
            // Arrange
            IStorageDriver storageDriver = await this.GetDefaultStorageDriverAsync().ConfigureAwait(false);
            var tenantId = Guid.NewGuid();
            var partitionId = Guid.NewGuid();
            var fileId = Guid.NewGuid();
            var metadata = new DriveFileMetadata
            {
                FileName = "test.txt",
                ContentType = "text/plain",
            };
            var fileContents = "Hello world!";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContents));

            // Act
            await storageDriver.UploadAsync(new UploadOperationParameters
            {
                TenantId = tenantId,
                PartitionId = partitionId,
                FileId = fileId,
                Data = stream,
                FileMetadata = metadata,
            }).ConfigureAwait(false);

            // Assert
            Result<ICollection<string>> streamsResult = await storageDriver.ListStreamsAsync(new ListStreamsOperationParameters
            {
                TenantId = tenantId,
                PartitionId = partitionId,
                FileId = fileId,
            }).ConfigureAwait(false);
            ICollection<string> streams = streamsResult.Value;

            streams.Should().NotBeNullOrEmpty();
            streams.Should().Contain(new[] { StreamOperationParameters.DefaultStreamName });

            // Cleanup
            await storageDriver.DeleteAsync(new DeleteOperationParameters
            {
                TenantId = tenantId,
                PartitionId = partitionId,
                FileId = fileId,
            }).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestHarness_DeleteAsync_MultipleStreams_Success()
        {
            // Arrange
            IStorageDriver storageDriver = await this.GetDefaultStorageDriverAsync().ConfigureAwait(false);
            var tenantId = Guid.NewGuid();
            var partitionId = Guid.NewGuid();
            var fileId = Guid.NewGuid();
            var metadata = new DriveFileMetadata
            {
                FileName = "test.txt",
                ContentType = "text/plain",
            };
            var fileContents = "Hello world!";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContents));

            // Act
            await storageDriver.UploadAsync(new UploadOperationParameters
            {
                TenantId = tenantId,
                PartitionId = partitionId,
                FileId = fileId,
                Data = stream,
                FileMetadata = metadata,
            }).ConfigureAwait(false);
            await storageDriver.UploadAsync(new UploadOperationParameters
            {
                TenantId = tenantId,
                PartitionId = partitionId,
                FileId = fileId,
                StreamName = "stream1",
                Data = stream,
                FileMetadata = metadata,
            }).ConfigureAwait(false);
            await storageDriver.UploadAsync(new UploadOperationParameters
            {
                TenantId = tenantId,
                PartitionId = partitionId,
                FileId = fileId,
                StreamName = "stream2",
                Data = stream,
                FileMetadata = metadata,
            }).ConfigureAwait(false);

            await storageDriver.DeleteAsync(new DeleteOperationParameters
            {
                TenantId = tenantId,
                PartitionId = partitionId,
                FileId = fileId,
            }).ConfigureAwait(false);

            // Assert
            bool defaultStreamExists = await storageDriver.ExistsAsync(new ExistsOperationParameters
            {
                TenantId = tenantId,
                PartitionId = partitionId,
                FileId = fileId,
            }).ConfigureAwait(false);
            bool stream1Exists = await storageDriver.ExistsAsync(new ExistsOperationParameters
            {
                TenantId = tenantId,
                PartitionId = partitionId,
                FileId = fileId,
                StreamName = "stream1",
            }).ConfigureAwait(false);
            bool stream2Exists = await storageDriver.ExistsAsync(new ExistsOperationParameters
            {
                TenantId = tenantId,
                PartitionId = partitionId,
                FileId = fileId,
                StreamName = "stream2",
            }).ConfigureAwait(false);

            defaultStreamExists.Should().BeFalse();
            stream1Exists.Should().BeFalse();
            stream2Exists.Should().BeFalse();

            // Cleanup
        }

        [Fact]
        public async Task TestHarness_UploadAsync_NullStream_Throws()
        {
            // Arrange
            IStorageDriver storageDriver = await this.GetDefaultStorageDriverAsync().ConfigureAwait(false);
            var tenantId = Guid.NewGuid();
            var partitionId = Guid.NewGuid();
            var fileId = Guid.NewGuid();
            string? streamName = null;
            string? alternateId = null;
            DriveFileMetadata? metadata = null;
            Stream? stream = null;

            // Act

            // Assert
            Result result = await storageDriver.UploadAsync(tenantId, partitionId, fileId, streamName, alternateId, stream, metadata).ConfigureAwait(false);
            result.IsError.Should().BeTrue();

            // Cleanup
            stream?.Dispose();
            await storageDriver.DeleteAsync(tenantId, partitionId, fileId, streamName, alternateId).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestHarness_UploadAsync_EmptyTenantId_Throws()
        {
            // Arrange
            IStorageDriver storageDriver = await this.GetDefaultStorageDriverAsync().ConfigureAwait(false);
            Guid tenantId = Guid.Empty;
            var partitionId = Guid.NewGuid();
            var fileId = Guid.NewGuid();
            string? streamName = null;
            string? alternateId = null;
            DriveFileMetadata? metadata = null;
            Stream stream = new MemoryStream();

            // Act

            // Assert
            Result result = await storageDriver.UploadAsync(tenantId, partitionId, fileId, streamName, alternateId, stream, metadata).ConfigureAwait(false);
            result.IsError.Should().BeTrue();

            // Cleanup
            stream?.Dispose();
        }

        [Fact]
        public async Task TestHarness_UploadAsync_EmptyPartitionId_Throws()
        {
            // Arrange
            IStorageDriver storageDriver = await this.GetDefaultStorageDriverAsync().ConfigureAwait(false);
            var tenantId = Guid.NewGuid();
            Guid partitionId = Guid.Empty;
            var fileId = Guid.NewGuid();
            string? streamName = null;
            string? alternateId = null;
            DriveFileMetadata? metadata = null;
            Stream stream = new MemoryStream();

            // Act

            // Assert
            Result result = await storageDriver.UploadAsync(tenantId, partitionId, fileId, streamName, alternateId, stream, metadata).ConfigureAwait(false);
            result.IsError.Should().BeTrue();

            // Cleanup
            stream?.Dispose();
        }

        [Fact]
        public async Task TestHarness_UploadAsync_EmptyFileId_Throws()
        {
            // Arrange
            IStorageDriver storageDriver = await this.GetDefaultStorageDriverAsync().ConfigureAwait(false);
            var tenantId = Guid.NewGuid();
            var partitionId = Guid.NewGuid();
            Guid fileId = Guid.Empty;
            string? streamName = null;
            string? alternateId = null;
            DriveFileMetadata? metadata = null;
            Stream stream = new MemoryStream();

            // Act

            // Assert
            Result result = await storageDriver.UploadAsync(tenantId, partitionId, fileId, streamName, alternateId, stream, metadata).ConfigureAwait(false);
            result.IsError.Should().BeTrue();

            // Cleanup
            stream?.Dispose();
        }

        protected abstract Task<IStorageDriver> GetStorageDriverAsync(string testName);

        private Task<IStorageDriver> GetDefaultStorageDriverAsync([CallerMemberName]string memberName = "")
        {
            return this.GetStorageDriverAsync(memberName);
        }
    }
}
