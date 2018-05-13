﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TransientFaultHandling.Tests.TestObjects;

namespace EnterpriseLibrary.TransientFaultHandling.Bvt.Tests
{
    [TestClass]
    public class RetryPolicyProgrammaticallyFixture
    {
        [TestMethod]
        public void CreateFixedIntervalRetryStrategyWithCountAndInterval()
        {
            try
            {
                var retryPolicy = new RetryPolicy<MockErrorDetectionStrategy>(new FixedInterval(3, TimeSpan.FromSeconds(1)));
                retryPolicy.ExecuteAction(() =>
                {
                    // Do Stuff
                    throw new InvalidCastException();
                });
            }
            catch (InvalidCastException)
            { }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}
