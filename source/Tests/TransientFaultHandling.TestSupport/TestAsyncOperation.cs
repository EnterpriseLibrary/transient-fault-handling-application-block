﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

namespace EnterpriseLibrary.TransientFaultHandling.TestSupport
{
    using System;
    using System.Threading;

    public class TestAsyncOperation
    {
        public TestAsyncOperation(Exception exceptionToThrow)
        {
            this.ExceptionToThrow = exceptionToThrow;
        }

        public int BeginMethodCount { get; private set; }
        public int EndMethodCount { get; private set; }
        public Exception ExceptionToThrow { get; set; }

        public IAsyncResult BeginMethod(AsyncCallback callback, object state)
        {
            this.BeginMethodCount++;
            var asyncResult = new TestAsyncResult();
            ThreadPool.QueueUserWorkItem(_ => callback(asyncResult), null);
            return asyncResult;
        }

        public bool EndMethod(IAsyncResult asyncResult)
        {
            this.EndMethodCount++;

            if (this.ExceptionToThrow != null)
            {
                throw this.ExceptionToThrow;
            }

            return true;
        }
    }

    public class TestAsyncResult : IAsyncResult
    {
        public bool IsCompleted { get; set; }

        public WaitHandle AsyncWaitHandle { get; set; }

        public object AsyncState { get; set; }

        public bool CompletedSynchronously { get; set; }
    }
}
