// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.

namespace Rixian.Drive.Storage.Abstractions.TestCases
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using NSubstitute;
    using Rixian.Drive.Storage.Abstractions;
    using Xunit.Abstractions;

    public abstract class TestHarnessBase : IDisposable
    {
        protected const string DefaultStorageDriverInfo = "UseDefaultDriver=true;";

        protected static readonly Guid DefaultDriveControllerId = Guid.Parse("c33f55ed-5e53-40e9-9282-d1c824bd1b54");
        private bool disposedValue = false; // To detect redundant calls

        public TestHarnessBase(ITestOutputHelper logger)
        {
            this.Logger = logger;
        }

        protected ITestOutputHelper Logger { get; }

        protected List<IDisposable> Disposables { get; } = new List<IDisposable>();

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    foreach (IDisposable disposable in this.Disposables)
                    {
                        disposable.Dispose();
                    }
                }

                this.disposedValue = true;
            }
        }
    }
}
