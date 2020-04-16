// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Drive.Storage.Abstractions
{
    using System;

    /// <summary>
    /// Defines the default set of file operation parameters.
    /// </summary>
    public class DefaultOperationParameters : IDefaultOperationParameters
    {
        private string version;

        /// <inheritdoc/>
        public Guid TenantId { get; set; }

        /// <inheritdoc/>
        public Guid PartitionId { get; set; }

        /// <inheritdoc/>
        public Guid FileId { get; set; }

        /// <inheritdoc/>
        public string AlternateId { get; set; }

        /// <inheritdoc/>
        public string Version
        {
            get => this.version;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    this.version = null;
                }
                else
                {
                    this.version = value.Trim();
                }
            }
        }

        /// <inheritdoc/>
        public bool IsVersioned => !string.IsNullOrWhiteSpace(this.Version);
    }
}
