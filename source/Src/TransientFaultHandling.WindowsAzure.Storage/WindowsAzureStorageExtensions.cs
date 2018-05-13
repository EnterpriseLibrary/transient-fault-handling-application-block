﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;

namespace EnterpriseLibrary.TransientFaultHandling
{
    /// <summary>
    /// Extends the RetryStrategy class to allow using the retry strategies from the Transient Fault Handling Application Block with Windows Azure Store.
    /// </summary>
    public static class WindowsAzureStorageExtensions
    {
        /// <summary>
        /// Wraps a Transient Fault Handling Application Block retry strategy into a Microsoft.WindowsAzure.StorageClient.RetryPolicy.
        /// </summary>
        /// <param name="retryStrategy">The Transient Fault Handling Application Block retry strategy to wrap.</param>
        /// <returns>Returns a wrapped Transient Fault Handling Application Block retry strategy into a Microsoft.WindowsAzure.StorageClient.RetryPolicy.</returns>
        public static Microsoft.WindowsAzure.StorageClient.RetryPolicy AsAzureStorageClientRetryPolicy(this RetryStrategy retryStrategy)
        {
            if (retryStrategy == null) throw new ArgumentNullException("retryStrategy");

            return () => new ShouldRetryWrapper(retryStrategy.GetShouldRetry()).ShouldRetry;
        }

        private class ShouldRetryWrapper
        {
            private readonly ShouldRetry shouldRetry;

            public ShouldRetryWrapper(ShouldRetry shouldRetry)
            {
                this.shouldRetry = shouldRetry;
            }

            public bool ShouldRetry(int retryCount, Exception lastException, out TimeSpan delay)
            {
                return this.shouldRetry(retryCount, lastException, out delay);
            }
        }
    }
}
