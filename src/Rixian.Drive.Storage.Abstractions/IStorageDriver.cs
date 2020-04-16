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
    /// Interface to a storage subsystem. Performs basic read/write/delete of data streams.
    /// </summary>
    public interface IStorageDriver
    {
        /// <summary>
        /// Gets the current storage driver verision.
        /// </summary>
        string DriverVersion { get; }

        /// <summary>
        /// Upgrades the partition to this storage driver version.
        /// </summary>
        /// <param name="tenantId">The tenant ID.</param>
        /// <param name="volumeId">The volume ID. Used for migrating off v1.0 storage.</param>
        /// <param name="partitionId">The partition ID.</param>
        /// <param name="cancellationToken">Used to cancel the upload operation.</param>
        /// <returns>Awaitable task.</returns>
        Task UpgradePartitionAsync(Guid tenantId, Guid volumeId, Guid partitionId, CancellationToken cancellationToken);

        /// <summary>
        /// Uploads a file stream to the underlying storage subsystem.
        /// </summary>
        /// <param name="parameters">The operation parameters.</param>
        /// <param name="cancellationToken">Used to cancel the upload operation.</param>
        /// <returns>Awaitable task.</returns>
        Task UploadAsync(UploadOperationParameters parameters, CancellationToken cancellationToken);

        /// <summary>
        /// Downloads a file stream from the underlying storage subsystem.
        /// </summary>
        /// <param name="parameters">The operation parameters.</param>
        /// <param name="cancellationToken">Used to cancel the download operation.</param>
        /// <returns>The downloaded file data.</returns>
        Task<DriveFile> DownloadAsync(DownloadOperationParameters parameters, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a file stream in the underlying storage subsystem.
        /// </summary>
        /// <param name="parameters">The operation parameters.</param>
        /// <param name="cancellationToken">Used to cancel the delete operation.</param>
        /// <returns>Awaitable task.</returns>
        Task DeleteAsync(DeleteOperationParameters parameters, CancellationToken cancellationToken);

        /// <summary>
        /// Lists all file streams for a file in the underlying storage subsystem.
        /// </summary>
        /// <param name="parameters">The operation parameters.</param>
        /// <param name="cancellationToken">Used to cancel the delete operation.</param>
        /// <returns>Awaitable task.</returns>
        Task<ICollection<string>> ListStreamsAsync(ListStreamsOperationParameters parameters, CancellationToken cancellationToken);

        /// <summary>
        /// Checks for existance for a file stream in the underlying storage subsystem.
        /// </summary>
        /// <param name="parameters">The operation parameters.</param>
        /// <param name="cancellationToken">Used to cancel the download operation.</param>
        /// <returns>The downloaded file data.</returns>
        Task<bool> ExistsAsync(ExistsOperationParameters parameters, CancellationToken cancellationToken);
    }
}
