// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using EnterpriseLibrary.Common.Configuration;
using EnterpriseLibrary.TransientFaultHandling.Configuration;
using System;
using System.Collections.Specialized;

namespace EnterpriseLibrary.TransientFaultHandling.Bvt.Tests.TestObjects
{
    [ConfigurationElementType(typeof(CustomRetryStrategyData))]
    public class TestRetryStrategy : RetryStrategy
    {
        public TestRetryStrategy(string name, bool firstFastRetry, NameValueCollection attributes)
            : base(name, firstFastRetry)
        {
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
