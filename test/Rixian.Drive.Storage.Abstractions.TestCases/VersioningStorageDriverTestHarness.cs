// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Drive.Storage.Abstractions.TestCases
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using Rixian.Drive.Storage.Abstractions;
    using Xunit;
    using Xunit.Abstractions;

    public abstract class VersioningStorageDriverTestHarness : StorageDriverTestHarness
    {
        protected VersioningStorageDriverTestHarness(ITestOutputHelper logger)
            : base(logger)
        {
        }

        [Fact]
        [Trait("TestCategory", "FailsInCloudTest")]
        public async Task TestHarness_Snapshot_MultipleStreams_Success()
        {
            // Arrange
            IVersioningStorageDriver storageDriver = await this.GetDefaultStorageDriverAsync().ConfigureAwait(false);
            var tenantId = Guid.NewGuid();
            var partitionId = Guid.NewGuid();
            Guid fileId = Guid.NewGuid();
            string? alternateId = null;
            DriveFileMetadata? metadata = null;
            using var defaultData = new MemoryStream();
            using var stream1Data = new MemoryStream();

            // Act
            await storageDriver.UploadAsync(tenantId, partitionId, fileId, "default", alternateId, defaultData, metadata).ConfigureAwait(false);
            await storageDriver.UploadAsync(tenantId, partitionId, fileId, "stream1", alternateId, stream1Data, metadata).ConfigureAwait(false);

            await storageDriver.SnapshotAsync(new SnapshotOperationParameters
            {
                TenantId = tenantId,
                PartitionId = partitionId,
                FileId = fileId,
                AlternateId = alternateId,
                Version = "123",
            }).ConfigureAwait(false);

            // Assert
            bool versionedDefaultStreamExists = await storageDriver.ExistsAsync(new ExistsOperationParameters
            {
                TenantId = tenantId,
                PartitionId = partitionId,
                FileId = fileId,
                AlternateId = alternateId,
                Version = "123",
            }).ConfigureAwait(false);
            bool versionedStream1Exists = await storageDriver.ExistsAsync(new ExistsOperationParameters
            {
                TenantId = tenantId,
                PartitionId = partitionId,
                FileId = fileId,
                AlternateId = alternateId,
                Version = "123",
                StreamName = "stream1",
            }).ConfigureAwait(false);
            versionedDefaultStreamExists.Should().BeTrue();
            versionedStream1Exists.Should().BeTrue();

            // Cleanup
            await storageDriver.DeleteAsync(tenantId, partitionId, fileId, "default", alternateId).ConfigureAwait(false);
        }

        protected override async Task<IStorageDriver> GetStorageDriverAsync(string testName)
        {
            return await this.GetVersioningStorageDriverAsync(testName).ConfigureAwait(false);
        }

        protected abstract Task<IVersioningStorageDriver> GetVersioningStorageDriverAsync(string testName);

        private Task<IVersioningStorageDriver> GetDefaultStorageDriverAsync([CallerMemberName]string memberName = "")
        {
            return this.GetVersioningStorageDriverAsync(memberName);
        }
    }
}
