// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Drive.Storage.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Rixian.Extensions.Errors;

    /// <summary>
    /// Represents the ability to manage drive controllers in the system.
    /// </summary>
    public interface IDriveControllerManager
    {
        /// <summary>
        /// Gets the currently loaded drive controllers.
        /// </summary>
        IReadOnlyDictionary<Guid, IDriveController> DriveControllers { get; }

        /// <summary>
        /// Loads a drive controller for use.
        /// </summary>
        /// <param name="driveController">The drive controller to use.</param>
        /// <returns>Returns either nothing or an error.</returns>
        Result LoadDriveController(IDriveController driveController);

        /// <summary>
        /// Unloads a drive controller.
        /// </summary>
        /// <param name="driveControllerId">The identifier of the drive controller to unload.</param>
        /// <returns>Returns either nothing or an error.</returns>
        Result UnloadDriveController(Guid driveControllerId);

        /// <summary>
        /// Lists drive controllers assigned to a tenant.
        /// </summary>
        /// <param name="tenantId">The tenantId to use.</param>
        /// <returns>List of drive controllers assigned to tenant.</returns>
        Task<Result<IReadOnlyList<IDriveController>>> ListDriveControllersAsync(Guid tenantId);

        /// <summary>
        /// Gets the default drive controller for the tenant.
        /// </summary>
        /// <param name="tenantId">The tenantId to use.</param>
        /// <returns>The default drive controller.</returns>
        Task<Result<IDriveController>> GetDefaultDriveControllerAsync(Guid tenantId);

        /// <summary>
        /// Gets the drive controller for a specific drive.
        /// </summary>
        /// <param name="tenantId">The tenantId to use.</param>
        /// <param name="driveId">The driveId to use.</param>
        /// <returns>The drive controller for the specified drive.</returns>
        Task<Result<IDriveController>> GetDriveControllerAsync(Guid tenantId, Guid driveId);
    }
}
