// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Drive.Storage.Abstractions
{
    using System;

    /// <summary>
    /// Defines a stream name in addition to the default file operation parameters.
    /// </summary>
    public class StreamOperationParameters : DefaultOperationParameters, IStreamOperationParameters
    {
        /// <summary>
        /// The default stream name: 'default'.
        /// </summary>
        public static readonly string DefaultStreamName = "default";

        private string streamName = DefaultStreamName;

        /// <inheritdoc/>
        public string StreamName
        {
            get => this.streamName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    this.streamName = DefaultStreamName;
                }
                else
                {
                    this.streamName = value.Trim();
                }
            }
        }

        /// <inheritdoc/>
        public bool IsDefaultStream => string.Equals(this.StreamName, DefaultStreamName, StringComparison.OrdinalIgnoreCase);
    }
}
