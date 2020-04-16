// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Drive.Storage.Abstractions
{
    using System.IO;

    /// <summary>
    /// Defines the upload operation parameters.
    /// </summary>
    public class UploadOperationParameters : StreamOperationParameters, IStreamOperationParameters
    {
        /// <summary>
        /// Gets or sets stream data.
        /// </summary>
        public Stream Data { get; set; }

        /// <summary>
        /// Gets or sets the file metadata.
        /// </summary>
        public DriveFileMetadata FileMetadata { get; set; } = default;
    }
}
