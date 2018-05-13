﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

namespace EnterpriseLibrary.TransientFaultHandling.Tests.ConfigurationScenarios.given_retry_policy_settings_with_policies
{
    using System;
    using Common.TestSupport.ContextBase;
    using EnterpriseLibrary.TransientFaultHandling.Configuration;
    using EnterpriseLibrary.TransientFaultHandling.TestSupport;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using EnterpriseLibrary.TransientFaultHandling;

    public class Context : ArrangeActAssert
    {
        protected RetryPolicyConfigurationSettings settings;

        protected override void Arrange()
        {
            this.settings = new RetryPolicyConfigurationSettings()
            {
                //DefaultRetryStrategy = "defaultPolicy",
                //DefaultSqlConnectionRetryStrategy = "defaultSqlConnectionPolicy",
                //DefaultSqlCommandRetryStrategy = "defaultSqlCommandPolicy",
                //DefaultAzureStorageRetryStrategy = "defaultAzureStoragePolicy",
                //DefaultAzureServiceBusRetryStrategy = "defaultAzureServiceBusStoragePolicy",
                //DefaultAzureCachingRetryStrategy = "defaultAzureCachingStoragePolicy",
                RetryStrategies = 
                {
                    new ExponentialBackoffData()
                    {
                        Name = "first",
                        MaxRetryCount = 1,
                        MinBackoff = TimeSpan.FromMilliseconds(2),
                        MaxBackoff = TimeSpan.FromMilliseconds(3),
                        DeltaBackoff = TimeSpan.FromMilliseconds(4)
                    },
                    new IncrementalData()
                    {
                        Name = "second",
                        MaxRetryCount = 1,
                        InitialInterval = TimeSpan.FromMilliseconds(2),
                        RetryIncrement = TimeSpan.FromMilliseconds(3)
                    },
                    new FixedIntervalData()
                    {
                        Name = "third",
                        MaxRetryCount = 1,
                        RetryInterval = TimeSpan.FromMilliseconds(2)
                    },  
                    new CustomRetryStrategyData(
                        "Test custom retry strategy",
                        "EnterpriseLibrary.TransientFaultHandling.TestSupport.TestRetryStrategy, EnterpriseLibrary.TransientFaultHandling.TestSupport, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
                        {
                            Name = "fourth",
                            FirstFastRetry = false,
                            Attributes = { { "customProperty", 10.ToString() } }
                        }
                    }
            };
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
        public void then_has_4_strategies_defined()
        {
            Assert.IsInstanceOfType(retryManager.GetRetryStrategy("first"), typeof(ExponentialBackoff));
            Assert.IsInstanceOfType(retryManager.GetRetryStrategy("second"), typeof(Incremental));
            Assert.IsInstanceOfType(retryManager.GetRetryStrategy("third"), typeof(FixedInterval));
            Assert.IsInstanceOfType(retryManager.GetRetryStrategy("fourth"), typeof(TestRetryStrategy));
        }

        [TestMethod]
        public void custom_strategy_has_custom_parameters()
        {
            var strategy = ((TestRetryStrategy)retryManager.GetRetryStrategy("fourth"));
            Assert.AreEqual(10, strategy.CustomProperty);
        }
    }
}
