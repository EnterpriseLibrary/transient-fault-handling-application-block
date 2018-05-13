// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using EnterpriseLibrary.TransientFaultHandling.Bvt.Tests.TestObjects;
using EnterpriseLibrary.TransientFaultHandling.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;

namespace EnterpriseLibrary.TransientFaultHandling.Bvt.Tests.Sql
{
    [TestClass]
    public class ThrottlingConditionFixture
    {
        [TestMethod]
        public void GetsRejectUpdateInsertThrottlingConditionFromSqlError()
        {
            SqlError sqlError = SqlExceptionCreator.GenerateFakeSqlError(ThrottlingCondition.ThrottlingErrorNumber, "Code: 12345 SQL Error");
            ThrottlingCondition condition = ThrottlingCondition.FromError(sqlError);

            Assert.AreEqual(ThrottlingMode.RejectUpdateInsert, condition.ThrottlingMode);
        }
    }
}
