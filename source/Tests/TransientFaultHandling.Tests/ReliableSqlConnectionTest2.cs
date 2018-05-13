﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Data.SqlClient;
using EnterpriseLibrary.TransientFaultHandling.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnterpriseLibrary.TransientFaultHandling.Tests
{
    [TestClass]
    public class ReliableSqlConnectionTest2
    {
        private string connectionString;

        [TestInitialize]
        public void Initialize()
        {
            this.connectionString = TestSqlSupport.SqlDatabaseConnectionString;
            RetryPolicyFactory.CreateDefault();
        }

        [TestCleanup]
        public void Cleanup()
        {
            RetryPolicyFactory.SetRetryManager(null, false);
        }

        [TestMethod]
        public void TestDoubleCommandsWithoutClosing()
        {
            ReliableSqlConnection connection = new ReliableSqlConnection(connectionString);

            SqlCommand command = new SqlCommand("SELECT 1");
            SqlCommand command2 = new SqlCommand("SELECT 2");

            connection.ExecuteCommand(command);
            connection.ExecuteCommand(command2);
        }

        [TestMethod]
        public void TestDoubleCommandsWithClosing()
        {
            ReliableSqlConnection connection = new ReliableSqlConnection(connectionString);

            SqlCommand command = new SqlCommand("SELECT 1");
            SqlCommand command2 = new SqlCommand("SELECT 2");

            connection.ExecuteCommand(command);
            connection.Close();
            connection.ExecuteCommand(command2);
        }

        [TestMethod]
        public void TestSingleCommandWithoutClosing()
        {
            ReliableSqlConnection connection = new ReliableSqlConnection(connectionString);

            SqlCommand command = new SqlCommand("SELECT 1");

            connection.ExecuteCommand(command);
            connection.ExecuteCommand(command);
        }

        [TestMethod]
        public void TestSingleCommandWithClosing()
        {
            ReliableSqlConnection connection = new ReliableSqlConnection(connectionString);

            SqlCommand command = new SqlCommand("SELECT 1");

            connection.ExecuteCommand(command);
            connection.Close();
            connection.ExecuteCommand(command);
        }
    }
}
