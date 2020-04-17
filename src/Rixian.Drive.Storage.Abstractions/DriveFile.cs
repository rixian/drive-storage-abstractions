// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Drive.Storage.Abstractions
{
    using System.IO;

    /// <summary>
    /// Represents information about a file.
    /// </summary>
    public class DriveFile
    {
        /// <summary>
        /// Gets or sets the file data.
        /// </summary>
        public Stream? Data { get; set; }

        /// <summary>
        /// Gets or sets the file metadata.
        /// </summary>
        public DriveFileMetadata? Metadata { get; set; }
    }
}
