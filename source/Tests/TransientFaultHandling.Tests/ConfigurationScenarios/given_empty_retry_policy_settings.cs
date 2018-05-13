// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

namespace EnterpriseLibrary.TransientFaultHandling.Tests.ConfigurationScenarios.given_empty_retry_policy_settings
{
    using Common.TestSupport.ContextBase;
    using EnterpriseLibrary.TransientFaultHandling.Configuration;
    using EnterpriseLibrary.TransientFaultHandling;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class Context : ArrangeActAssert
    {
        protected RetryPolicyConfigurationSettings settings;

        protected override void Arrange()
        {
            settings = new RetryPolicyConfigurationSettings()
                           {
                               //DefaultRetryStrategy = "defaultPolicy",
                               //DefaultSqlConnectionRetryStrategy = "defaultSqlConnectionPolicy",
                               //DefaultSqlCommandRetryStrategy = "defaultSqlCommandPolicy",
                               //DefaultAzureServiceBusRetryStrategy = "defaultAzureServiceBusStoragePolicy",
                               //DefaultAzureCachingRetryStrategy = "defaultAzureCachingStoragePolicy",
                               //DefaultAzureStorageRetryStrategy = "defaultAzureStoragePolicy"
                           };
        }
    }

    [TestClass]
    public class when_building_retry_manager : Context
    {
        private RetryManager retryManager;

        protected override void Act()
        {
            retryManager = settings.BuildRetryManager();
        }

        [TestMethod]
        public void then_is_not_null()
        {
            Assert.IsNotNull(retryManager);
            Assert.IsInstanceOfType(retryManager, typeof(RetryManager));
        }
    }
}
