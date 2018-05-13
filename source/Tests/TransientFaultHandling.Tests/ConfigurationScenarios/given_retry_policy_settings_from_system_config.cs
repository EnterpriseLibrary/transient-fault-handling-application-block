// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

namespace EnterpriseLibrary.TransientFaultHandling.Tests.ConfigurationScenarios.given_retry_policy_settings_from_system_config
{
    using Common.TestSupport.ContextBase;
    using EnterpriseLibrary.TransientFaultHandling.Configuration;
    using EnterpriseLibrary.Common.Configuration;
    using EnterpriseLibrary.TransientFaultHandling;
    using EnterpriseLibrary.TransientFaultHandling.TestSupport;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class Context : ArrangeActAssert
    {
        protected RetryPolicyConfigurationSettings settings;

        protected override void Arrange()
        {
            this.settings = RetryPolicyConfigurationSettings.GetRetryPolicySettings(new SystemConfigurationSource());
        }
    }

    [TestClass]
    public class when_getting_retry_manager : Context
    {
        private RetryManager retryManager;

        protected override void Act()
        {
            retryManager = settings.BuildRetryManager();
        }

        [TestMethod]
        public void then_has_some_strategies_defined()
        {
            Assert.IsInstanceOfType(retryManager.GetRetryStrategy("ExponentialIntervalDefault"), typeof(ExponentialBackoff));
            Assert.IsInstanceOfType(retryManager.GetRetryStrategy("IncrementalIntervalDefault"), typeof(Incremental));
            Assert.IsInstanceOfType(retryManager.GetRetryStrategy("FixedIntervalDefault"), typeof(FixedInterval));
            Assert.IsInstanceOfType(retryManager.GetRetryStrategy("TestCustomRetryStrategy"), typeof(TestRetryStrategy));
        }

        [TestMethod]
        public void custom_strategy_has_custom_parameters()
        {
            var strategy = ((TestRetryStrategy)retryManager.GetRetryStrategy("TestCustomRetryStrategy"));
            Assert.AreEqual(10, strategy.CustomProperty);
        }
    }
}
