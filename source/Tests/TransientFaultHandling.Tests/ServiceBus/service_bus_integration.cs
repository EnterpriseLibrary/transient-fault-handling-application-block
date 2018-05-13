﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

namespace EnterpriseLibrary.TransientFaultHandling.Tests.ServiceBus.service_bus_integration
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using EnterpriseLibrary.Common.TestSupport.ContextBase;
    using EnterpriseLibrary.TransientFaultHandling;
    using EnterpriseLibrary.TransientFaultHandling.TestSupport;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class Context : ArrangeActAssert
    {
        protected ServiceBusTransientErrorDetectionStrategy strategy;
        protected NamespaceManager namespaceManager;

        protected override void Arrange()
        {
            var sbConnectionString = ConfigurationHelper.GetSetting("Microsoft.ServiceBus.ConnectionString");

            if (string.IsNullOrEmpty(sbConnectionString) 
                || sbConnectionString.Contains("[your namespace]")
                || sbConnectionString.Contains("[your secret]"))
            {
                Assert.Inconclusive("Cannot run tests because the Service Bus credentials are not configured in app.config");
            }

            this.namespaceManager = NamespaceManager.CreateFromConnectionString(sbConnectionString);
            this.strategy = new ServiceBusTransientErrorDetectionStrategy();
        }
    }

    [TestClass]
    public class when_accessing_service_bus : Context
    {
        [TestMethod]
        public void then_duplicate_topic_creation_is_not_transient()
        {
            var topicPath = "service_bus_integration/" + Guid.NewGuid();
            try
            {
                this.namespaceManager.CreateTopic(topicPath);

                try
                {
                    this.namespaceManager.CreateTopic(topicPath);
                }
                catch (Exception ex)
                {
                    Assert.IsInstanceOfType(ex, typeof(MessagingEntityAlreadyExistsException));
                    Assert.IsFalse(this.strategy.IsTransient(ex));
                }
            }
            finally
            {
                TryDeleteTopic(topicPath);
            }
        }

        [TestMethod]
        public void then_duplicate_subscription_creation_is_not_transient()
        {
            var topicPath = "service_bus_integration/" + Guid.NewGuid();
            var subscriptionName = "test";
            try
            {
                this.namespaceManager.CreateTopic(topicPath);
                this.namespaceManager.CreateSubscription(topicPath, subscriptionName);

                try
                {
                    this.namespaceManager.CreateSubscription(topicPath, subscriptionName);
                }
                catch (Exception ex)
                {
                    Assert.IsInstanceOfType(ex, typeof(MessagingEntityAlreadyExistsException));
                    Assert.IsFalse(this.strategy.IsTransient(ex));
                }
            }
            finally
            {
                TryDeleteTopic(topicPath);
            }
        }

        [TestMethod]
        public void then_timeouts_are_transient()
        {
            // Do several calls to force some timeouts
            int numberOfCalls = 10;
            var topicPath = "service_bus_integration/" + Guid.NewGuid();
            try
            {
                this.namespaceManager.CreateTopic(topicPath);

                var barrier = new Barrier(numberOfCalls);
                var countdown = new CountdownEvent(numberOfCalls);
                var exceptions = new ConcurrentBag<Exception>();

                for (int i = 0; i < numberOfCalls; i++)
                {
                    ThreadPool.QueueUserWorkItem(x =>
                    {
                        MessagingFactory factory = null;
                        try
                        {
                            factory = MessagingFactory.Create(this.namespaceManager.Address, new MessagingFactorySettings
                            {
                                // Explicitly set a VERY short timeout, to force a timeout very frequently.
                                OperationTimeout = TimeSpan.FromSeconds(1),
                                TokenProvider = this.namespaceManager.Settings.TokenProvider
                            });
                            var client = factory.CreateTopicClient(topicPath);
                            barrier.SignalAndWait();
                            client.Send(new BrokeredMessage(Guid.NewGuid()));
                        }
                        catch (Exception ex)
                        {
                            exceptions.Add(ex);
                        }
                        finally
                        {
                            if (factory != null) factory.Close();
                            countdown.Signal();
                        }
                    });
                }

                countdown.Wait();
                if (exceptions.Count == 0)
                {
                    Assert.Inconclusive("No exceptions were thrown to check if they are transient");
                }

                foreach (var ex in exceptions)
                {
                    Assert.IsTrue(strategy.IsTransient(ex), ex.ToString());
                }
            }
            finally
            {
                TryDeleteTopic(topicPath);
            }
        }

        private void TryDeleteTopic(string topicName)
        {
            try
            {
                this.namespaceManager.DeleteTopic(topicName);
            }
            catch { }
        }
    }
}
