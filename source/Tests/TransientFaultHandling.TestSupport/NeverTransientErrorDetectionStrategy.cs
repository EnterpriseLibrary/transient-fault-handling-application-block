// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

namespace EnterpriseLibrary.TransientFaultHandling.TestSupport
{
    using System;
    using EnterpriseLibrary.TransientFaultHandling;

    public class NeverTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        public bool IsTransient(Exception ex)
        {
            return false;
        }
    }
}
