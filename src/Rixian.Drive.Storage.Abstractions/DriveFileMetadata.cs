// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Drive.Storage.Abstractions
{
    /// <summary>
    /// Represents metadata about a file.
    /// </summary>
    public class DriveFileMetadata
    {
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// Gets or sets the content type of the file.
        /// </summary>
        public string? ContentType { get; set; }
    }
}
