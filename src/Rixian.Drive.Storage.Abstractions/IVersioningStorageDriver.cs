// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Drive.Storage.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;
    using Rixian.Extensions.Errors;

    /// <summary>
    /// Interface to a storage subsystem. Performs basic read/write/delete of data streams.
    /// </summary>
    public interface IVersioningStorageDriver : IStorageDriver
    {
        /// <summary>
        /// Snapshots a file into a new version in the underlying storage subsystem.
        /// </summary>
        /// <param name="parameters">The operation parameters.</param>
        /// <param name="cancellationToken">Used to cancel the delete operation.</param>
        /// <returns>Awaitable task.</returns>
        Task<Result> SnapshotAsync(SnapshotOperationParameters parameters, CancellationToken cancellationToken);
    }
}
