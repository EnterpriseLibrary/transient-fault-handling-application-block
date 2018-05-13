// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using EnterpriseLibrary.Common.Configuration;
using EnterpriseLibrary.TransientFaultHandling.Bvt.Tests.TestObjects;
using EnterpriseLibrary.TransientFaultHandling.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransientFaultHandling.Tests.TestObjects;

namespace EnterpriseLibrary.TransientFaultHandling.Bvt.Tests.Extensibility
{
    [TestClass]
    public class ExtensibilityFixture
    {
        private SystemConfigurationSource configurationSource;
        private RetryPolicyConfigurationSettings retryPolicySettings;
        private RetryManager retryManager;

        [TestInitialize]
        public void Initialize()
        {
            this.configurationSource = new SystemConfigurationSource();
            this.retryPolicySettings = RetryPolicyConfigurationSettings.GetRetryPolicySettings(this.configurationSource);
            this.retryManager = retryPolicySettings.BuildRetryManager();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (this.configurationSource != null)
            {
                this.configurationSource.Dispose();
            }
        }

        [TestMethod]
        public void CreatesCustomRetryStrategyFromConfiguration()
        {
            var mockCustomStrategy = this.retryManager.GetRetryPolicy<MockErrorDetectionStrategy>("Test Retry Strategy");
            Assert.IsInstanceOfType(mockCustomStrategy.RetryStrategy, typeof(TestRetryStrategy));
        }
    }
}
