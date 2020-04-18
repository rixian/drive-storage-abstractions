// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Drive.Storage.Abstractions
{
    using System;
    using System.Threading.Tasks;
    using Rixian.Extensions.Errors;

    /// <summary>
    /// Represents a storage device type. Anologous to a physical machine which may contain many disks.
    /// </summary>
    public interface IDriveController
    {
        /// <summary>
        /// Gets the drive controller unique identifier.
        /// </summary>
        Guid ControllerId { get; }

        /// <summary>
        /// Gets the display name of this storage device.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Gets the driver for this storage device.
        /// </summary>
        /// <param name="driverInfo">Information required to initialize an instance of a storage driver.</param>>
        /// <returns>The initialized storage driver.</returns>
        Task<Result<IStorageDriver>> GetDriverAsync(string driverInfo);
    }
}
