// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

namespace EnterpriseLibrary.TransientFaultHandling.TestSupport
{
    using System;
    using System.Collections.Specialized;
    using EnterpriseLibrary.Common.Configuration;
    using EnterpriseLibrary.TransientFaultHandling;
    using EnterpriseLibrary.TransientFaultHandling.Configuration;

    [ConfigurationElementType(typeof(CustomRetryStrategyData))]
    public class TestRetryStrategy : RetryStrategy
    {
        public TestRetryStrategy()
            : base("TestRetryStrategy", true)
        {
            this.CustomProperty = 1;
        }

        public TestRetryStrategy(string name, bool firstFastRetry, NameValueCollection attributes)
            : base(name, firstFastRetry)
        {
            this.CustomProperty = int.Parse(attributes["customProperty"]);
        }

        public int CustomProperty { get; private set; }

        public int ShouldRetryCount { get; private set; }

        public override ShouldRetry GetShouldRetry()
        {
            return delegate(int currentRetryCount, Exception lastException, out TimeSpan interval)
            {
                if (this.CustomProperty == currentRetryCount)
                {
                    interval = TimeSpan.Zero;
                    return false;
                }

                this.ShouldRetryCount++;

                interval = TimeSpan.FromMilliseconds(1);
                return true;
            };
        }
    }
}
