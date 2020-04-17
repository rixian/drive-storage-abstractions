// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Drive.Storage.Abstractions
{
    /// <summary>
    /// Defines a stream name in addition to the default file operation parameters.
    /// </summary>
    public interface IStreamOperationParameters : IDefaultOperationParameters
    {
        /// <summary>
        /// Gets a value indicating whether the stream indicates the default stream.
        /// </summary>
        bool IsDefaultStream { get; }

        /// <summary>
        /// Gets or sets the name of the stream. Defaults to the DefaultStreamName.
        /// </summary>
        string? StreamName { get; set; }
    }
}
