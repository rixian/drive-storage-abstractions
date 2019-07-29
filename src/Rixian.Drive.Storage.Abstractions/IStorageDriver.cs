// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Drive.Storage.Abstractions
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface to a storage subsystem. Performs basic read/write/delete of data streams.
    /// </summary>
    public interface IStorageDriver
    {
        /// <summary>
        /// Uploads a file stream to the underlying storage subsystem.
        /// </summary>
        /// <param name="tenantId">The ID of the tenant.</param>
        /// <param name="partitionId">The ID of the partition to which the file is being written.</param>
        /// <param name="fileId">The ID of the file to write.</param>
        /// <param name="streamName">The name of the stream being written.</param>
        /// <param name="alternateId">The alternate id for the file.</param>
        /// <param name="stream">The data stream to upload.</param>
        /// <param name="fileMetadata">Optional. Metadata about the file being uploaded.</param>
        /// <param name="cancellationToken">Used to cancel the upload operation.</param>
        /// <returns>Awaitable task.</returns>
        Task UploadAsync(Guid tenantId, Guid partitionId, Guid fileId, string streamName, string alternateId, Stream stream, DriveFileMetadata fileMetadata = default(DriveFileMetadata), CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Downloads a file stream from the underlying storage subsystem.
        /// </summary>
        /// <param name="tenantId">The ID of the tenant.</param>
        /// <param name="partitionId">The ID of the partition that contains the file.</param>
        /// <param name="fileId">The ID of the file to download.</param>
        /// <param name="streamName">The name of the stream being downloaded.</param>
        /// <param name="alternateId">The alternate id for the file.</param>
        /// <param name="cancellationToken">Used to cancel the download operation.</param>
        /// <returns>The downloaded file data.</returns>
        Task<DriveFile> DownloadAsync(Guid tenantId, Guid partitionId, Guid fileId, string streamName, string alternateId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes a file stream in the underlying storage subsystem.
        /// </summary>
        /// <param name="tenantId">The ID of the tenant.</param>
        /// <param name="partitionId">The ID of the partition that contains the file.</param>
        /// <param name="fileId">The ID of the file to delete.</param>
        /// <param name="streamName">The name of the stream being deleted.</param>
        /// <param name="alternateId">The alternate id for the file.</param>
        /// <param name="cancellationToken">Used to cancel the delete operation.</param>
        /// <returns>Awaitable task.</returns>
        Task DeleteAsync(Guid tenantId, Guid partitionId, Guid fileId, string streamName, string alternateId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
