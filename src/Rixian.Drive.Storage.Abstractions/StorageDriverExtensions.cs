// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Drive.Storage.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extensions for the IStorageDriver interface.
    /// </summary>
    public static class StorageDriverExtensions
    {
        /// <summary>
        /// Uploads a file stream to the underlying storage subsystem.
        /// </summary>
        /// <param name="storageDriver">The IStorageDriver.</param>
        /// <param name="tenantId">The ID of the tenant.</param>
        /// <param name="partitionId">The ID of the partition to which the file is being written.</param>
        /// <param name="fileId">The ID of the file to write.</param>
        /// <param name="streamName">The name of the stream being written.</param>
        /// <param name="alternateId">The alternate id for the file.</param>
        /// <param name="stream">The data stream to upload.</param>
        /// <param name="fileMetadata">Optional. Metadata about the file being uploaded.</param>
        /// <param name="cancellationToken">Used to cancel the upload operation.</param>
        /// <returns>Awaitable task.</returns>
        public static async Task UploadAsync(this IStorageDriver storageDriver, Guid tenantId, Guid partitionId, Guid fileId, string streamName, string alternateId, Stream stream, DriveFileMetadata fileMetadata = default, CancellationToken cancellationToken = default)
        {
            if (storageDriver is null)
            {
                throw new ArgumentNullException(nameof(storageDriver));
            }

            await storageDriver.UploadAsync(
                new UploadOperationParameters
                {
                    TenantId = tenantId,
                    PartitionId = partitionId,
                    FileId = fileId,
                    StreamName = streamName,
                    AlternateId = alternateId,
                    Data = stream,
                    FileMetadata = fileMetadata,
                },
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Downloads a file stream from the underlying storage subsystem.
        /// </summary>
        /// <param name="storageDriver">The IStorageDriver.</param>
        /// <param name="tenantId">The ID of the tenant.</param>
        /// <param name="partitionId">The ID of the partition that contains the file.</param>
        /// <param name="fileId">The ID of the file to download.</param>
        /// <param name="streamName">The name of the stream being downloaded.</param>
        /// <param name="alternateId">The alternate id for the file.</param>
        /// <param name="cancellationToken">Used to cancel the download operation.</param>
        /// <returns>The downloaded file data.</returns>
        public static async Task<DriveFile> DownloadAsync(this IStorageDriver storageDriver, Guid tenantId, Guid partitionId, Guid fileId, string streamName, string alternateId, CancellationToken cancellationToken = default)
        {
            if (storageDriver is null)
            {
                throw new ArgumentNullException(nameof(storageDriver));
            }

            return await storageDriver.DownloadAsync(
                new DownloadOperationParameters
                {
                    TenantId = tenantId,
                    PartitionId = partitionId,
                    FileId = fileId,
                    StreamName = streamName,
                    AlternateId = alternateId,
                },
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes a file stream in the underlying storage subsystem.
        /// </summary>
        /// <param name="storageDriver">The IStorageDriver.</param>
        /// <param name="tenantId">The ID of the tenant.</param>
        /// <param name="partitionId">The ID of the partition that contains the file.</param>
        /// <param name="fileId">The ID of the file to delete.</param>
        /// <param name="streamName">The name of the stream being deleted.</param>
        /// <param name="alternateId">The alternate id for the file.</param>
        /// <param name="cancellationToken">Used to cancel the delete operation.</param>
        /// <returns>Awaitable task.</returns>
        public static async Task DeleteAsync(this IStorageDriver storageDriver, Guid tenantId, Guid partitionId, Guid fileId, string streamName, string alternateId, CancellationToken cancellationToken = default)
        {
            if (storageDriver is null)
            {
                throw new ArgumentNullException(nameof(storageDriver));
            }

            await storageDriver.DeleteAsync(
                new DeleteOperationParameters
                {
                    TenantId = tenantId,
                    PartitionId = partitionId,
                    FileId = fileId,
                    StreamName = streamName,
                    AlternateId = alternateId,
                },
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Lists all file streams for a file in the underlying storage subsystem.
        /// </summary>
        /// <param name="storageDriver">The IStorageDriver.</param>
        /// <param name="tenantId">The ID of the tenant.</param>
        /// <param name="partitionId">The ID of the partition that contains the file.</param>
        /// <param name="fileId">The ID of the file to delete.</param>
        /// <param name="alternateId">The alternate id for the file.</param>
        /// <param name="cancellationToken">Used to cancel the delete operation.</param>
        /// <returns>Awaitable task.</returns>
        public static async Task<ICollection<string>> ListStreamsAsync(this IStorageDriver storageDriver, Guid tenantId, Guid partitionId, Guid fileId, string alternateId, CancellationToken cancellationToken = default)
        {
            if (storageDriver is null)
            {
                throw new ArgumentNullException(nameof(storageDriver));
            }

            return await storageDriver.ListStreamsAsync(
                new ListStreamsOperationParameters
                {
                    TenantId = tenantId,
                    PartitionId = partitionId,
                    FileId = fileId,
                    AlternateId = alternateId,
                },
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Upgrades the partition to this storage driver version.
        /// </summary>
        /// <param name="storageDriver">The IStorageDriver.</param>
        /// <param name="tenantId">The tenant ID.</param>
        /// <param name="volumeId">The volume ID. Used for migrating off v1.0 storage.</param>
        /// <param name="partitionId">The partition ID.</param>
        /// <returns>Awaitable task.</returns>
        public static async Task UpgradePartitionAsync(this IStorageDriver storageDriver, Guid tenantId, Guid volumeId, Guid partitionId)
        {
            if (storageDriver is null)
            {
                throw new ArgumentNullException(nameof(storageDriver));
            }

            await storageDriver.UpgradePartitionAsync(tenantId, volumeId, partitionId, default).ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads a file stream to the underlying storage subsystem.
        /// </summary>
        /// <param name="storageDriver">The IStorageDriver.</param>
        /// <param name="parameters">The operation parameters.</param>
        /// <returns>Awaitable task.</returns>
        public static async Task UploadAsync(this IStorageDriver storageDriver, UploadOperationParameters parameters)
        {
            if (storageDriver is null)
            {
                throw new ArgumentNullException(nameof(storageDriver));
            }

            await storageDriver.UploadAsync(parameters, default).ConfigureAwait(false);
        }

        /// <summary>
        /// Downloads a file stream from the underlying storage subsystem.
        /// </summary>
        /// <param name="storageDriver">The IStorageDriver.</param>
        /// <param name="parameters">The operation parameters.</param>
        /// <returns>The downloaded file data.</returns>
        public static async Task<DriveFile> DownloadAsync(this IStorageDriver storageDriver, DownloadOperationParameters parameters)
        {
            if (storageDriver is null)
            {
                throw new ArgumentNullException(nameof(storageDriver));
            }

            return await storageDriver.DownloadAsync(parameters, default).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes a file stream in the underlying storage subsystem.
        /// </summary>
        /// <param name="storageDriver">The IStorageDriver.</param>
        /// <param name="parameters">The operation parameters.</param>
        /// <returns>Awaitable task.</returns>
        public static async Task DeleteAsync(this IStorageDriver storageDriver, DeleteOperationParameters parameters)
        {
            if (storageDriver is null)
            {
                throw new ArgumentNullException(nameof(storageDriver));
            }

            await storageDriver.DeleteAsync(parameters, default).ConfigureAwait(false);
        }

        /// <summary>
        /// Lists all file streams for a file in the underlying storage subsystem.
        /// </summary>
        /// <param name="storageDriver">The IStorageDriver.</param>
        /// <param name="parameters">The operation parameters.</param>
        /// <returns>Awaitable task.</returns>
        public static async Task<ICollection<string>> ListStreamsAsync(this IStorageDriver storageDriver, ListStreamsOperationParameters parameters)
        {
            if (storageDriver is null)
            {
                throw new ArgumentNullException(nameof(storageDriver));
            }

            return await storageDriver.ListStreamsAsync(parameters, default).ConfigureAwait(false);
        }

        /// <summary>
        /// Checks for existance for a file stream in the underlying storage subsystem.
        /// </summary>
        /// <param name="storageDriver">The IStorageDriver.</param>
        /// <param name="parameters">The operation parameters.</param>
        /// <returns>The downloaded file data.</returns>
        public static async Task<bool> ExistsAsync(this IStorageDriver storageDriver, ExistsOperationParameters parameters)
        {
            if (storageDriver is null)
            {
                throw new ArgumentNullException(nameof(storageDriver));
            }

            return await storageDriver.ExistsAsync(parameters, default).ConfigureAwait(false);
        }
    }
}
