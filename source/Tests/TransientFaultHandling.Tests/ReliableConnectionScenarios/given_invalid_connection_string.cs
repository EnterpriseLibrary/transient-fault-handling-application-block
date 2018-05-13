﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using EnterpriseLibrary.TransientFaultHandling.Data;

namespace EnterpriseLibrary.TransientFaultHandling.Tests.ReliableConnectionScenarios.given_invalid_connection_string
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    using Common.TestSupport.ContextBase;

    using EnterpriseLibrary.TransientFaultHandling.TestSupport;
    using EnterpriseLibrary.TransientFaultHandling;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public abstract class Context : ArrangeActAssert
    {
        protected ReliableSqlConnection reliableConnection;
        protected TestRetryStrategy connectionStrategy;
        protected TestRetryStrategy commandStrategy;
        protected SqlCommand command;

        protected override void Arrange()
        {
            this.command = new SqlCommand(TestSqlSupport.InvalidSqlText);

            this.connectionStrategy = new TestRetryStrategy();

            this.commandStrategy = new TestRetryStrategy();

            this.reliableConnection = new ReliableSqlConnection(
                TestSqlSupport.InvalidConnectionString,
                new RetryPolicy<AlwaysTransientErrorDetectionStrategy>(this.connectionStrategy),
                new RetryPolicy<AlwaysTransientErrorDetectionStrategy>(this.commandStrategy));
        }
    }

    [TestClass]
    public class when_executing_command_with_no_connection : Context
    {
        protected override void Act()
        {
            try
            {
                this.reliableConnection.ExecuteCommand<IDataReader>(this.command);
                Assert.Fail();
            }
            catch (SqlException ex)
            {
                if (!ex.Message.StartsWith("A network-related or instance-specific error occurred while establishing a connection to SQL Server."))
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void then_connection_is_null()
        {
            Assert.IsNull(this.command.Connection);
        }

        [TestMethod]
        // TODO: this is a bug, and we need to fix it!
        [Ignore]
        public void then_retried()
        {
            Assert.AreEqual(2, this.connectionStrategy.ShouldRetryCount);
            Assert.AreEqual(1, this.commandStrategy.ShouldRetryCount);
        }
    }

    [TestClass]
    public class when_executing_command_with_closed_connection : Context
    {
        protected override void Act()
        {
            try
            {
                this.command.Connection = new SqlConnection(TestSqlSupport.InvalidConnectionString);
                this.reliableConnection.ExecuteCommand<IDataReader>(this.command);
                Assert.Fail();
            }
            catch (SqlException ex)
            {
                if (!ex.Message.StartsWith("A network-related or instance-specific error occurred while establishing a connection to SQL Server."))
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void then_connection_is_closed()
        {
            Assert.IsTrue(this.command.Connection.State == ConnectionState.Closed);
        }

        [TestMethod]
        public void then_retried()
        {
            Assert.AreEqual(0, this.connectionStrategy.ShouldRetryCount);
            Assert.AreEqual(1, this.commandStrategy.ShouldRetryCount);
        }
    }
}