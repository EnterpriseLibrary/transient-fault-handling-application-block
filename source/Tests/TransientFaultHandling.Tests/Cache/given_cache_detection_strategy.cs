﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using EnterpriseLibrary.TransientFaultHandling;

namespace EnterpriseLibrary.TransientFaultHandling.Tests.Cache.given_cache_detection_strategy
{
    using System;
    using System.Net.Sockets;
    using System.Reflection;
    using System.ServiceModel;
    using EnterpriseLibrary.Common.TestSupport.ContextBase;
    using Microsoft.ApplicationServer.Caching;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class Context : ArrangeActAssert
    {
        protected CacheTransientErrorDetectionStrategy strategy;

        protected override void Arrange()
        {
            this.strategy = new CacheTransientErrorDetectionStrategy();
        }
    }

    [TestClass]
    public class when_checking_exceptions : Context
    {
        [TestMethod]
        public void then_determines_null_failure_is_non_transient()
        {
            Assert.IsFalse(this.strategy.IsTransient(null));
        }

        [TestMethod]
        public void then_determines_transient_failure_is_transient()
        {
            Assert.IsTrue(this.strategy.IsTransient(new ServerTooBusyException()));
        }

        [TestMethod]
        public void then_determines_non_transient_failure_is_non_transient()
        {
            Assert.IsFalse(this.strategy.IsTransient(new CommunicationObjectFaultedException()));
        }

        [TestMethod]
        public void then_determines_transient_failure_inner_of_non_transient_failure_is_transient()
        {
            Assert.IsTrue(this.strategy.IsTransient(new CommunicationObjectFaultedException("transient", new SocketException((int)SocketError.TimedOut))));
        }

        [TestMethod]
        public void then_determines_transient_failure_inner_of_non_transient_failure_inner_of_non_transient_failure_is_transient()
        {
            Assert.IsTrue(this.strategy.IsTransient((new CommunicationObjectFaultedException("transient", new CommunicationObjectFaultedException("transient", new ServerTooBusyException())))));
        }

        [TestMethod]
        public void then_determines_datacacheexception_with_specific_code_is_transient()
        {
            int[] transientErrorCodes = new int[]
            {
                DataCacheErrorCode.ServiceAccessError,
                DataCacheErrorCode.ConnectionTerminated,
                DataCacheErrorCode.RetryLater,
                DataCacheErrorCode.Timeout,
            };
            int substatus = -1;
            foreach (var errorCode in transientErrorCodes)
            {
                var exception = (Exception)Activator.CreateInstance(typeof(DataCacheException), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { "source", errorCode, substatus, "message" }, null);
                Assert.IsTrue(this.strategy.IsTransient(exception));
            }
        }

        [TestMethod]
        public void then_determines_nontransient_datacacheexception_is_not_transient()
        {
            int errorCode = -1;
            int substatus = -1;
            var exception = (Exception)Activator.CreateInstance(typeof(DataCacheException), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { "source", errorCode, substatus, "message" }, null);
            Assert.IsFalse(this.strategy.IsTransient(exception));
        }
    }
}
