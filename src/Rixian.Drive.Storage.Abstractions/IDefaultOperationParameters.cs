// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Drive.Storage.Abstractions
{
    using System;

    /// <summary>
    /// Defines the default set of file operation parameters.
    /// </summary>
    public interface IDefaultOperationParameters
    {
        /// <summary>
        /// Gets or sets the ID of the tenant.
        /// </summary>
        Guid TenantId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the partition to which the file belongs.
        /// </summary>
        Guid PartitionId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the file.
        /// </summary>
        Guid FileId { get; set; }

        /// <summary>
        /// Gets or sets the alternate id for the file.
        /// </summary>
        string? AlternateId { get; set; }

        /// <summary>
        /// Gets or sets the file version.
        /// </summary>
        string? Version { get; set; }

        /// <summary>
        /// Gets a value indicating whether the parameters indicates a file version.
        /// </summary>
        bool IsVersioned { get; }
    }
}
