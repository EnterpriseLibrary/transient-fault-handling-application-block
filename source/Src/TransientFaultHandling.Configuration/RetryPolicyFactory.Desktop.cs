// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

namespace EnterpriseLibrary.TransientFaultHandling
{
    /// <summary>
    /// Provides a factory class for instantiating application-specific retry policies described in the application configuration.
    /// </summary>
    public static partial class RetryPolicyFactory
    {
        /// <summary>
        /// Returns the default retry policy dedicated to handling transient conditions with Windows Azure Service Bus.
        /// </summary>
        /// <returns>The retry policy for Windows Azure Service Bus with the corresponding default strategy (or the default strategy if no retry strategy definition for Windows Azure Service Bus was found).</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "As designed")]
        public static RetryPolicy GetDefaultAzureServiceBusRetryPolicy()
        {
            return GetOrCreateRetryManager().GetDefaultAzureServiceBusRetryPolicy();
        }

        /// <summary>
        /// Returns the default retry policy dedicated to handling transient conditions with Windows Azure Storage.
        /// </summary>
        /// <returns>The retry policy for Windows Azure Storage with the corresponding default strategy (or the default strategy if no retry strategy definition for Windows Azure Storage was found).</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "As designed")]
        public static RetryPolicy GetDefaultAzureStorageRetryPolicy()
        {
            return GetOrCreateRetryManager().GetDefaultAzureStorageRetryPolicy();
        }
    }
}
