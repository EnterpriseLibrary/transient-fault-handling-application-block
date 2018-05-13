// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;
using EnterpriseLibrary.Common.Configuration.Design;
using EnterpriseLibrary.TransientFaultHandling.Configuration;

[assembly: ComVisible(false)]

[assembly: SecurityTransparent]
[assembly: NeutralResourcesLanguage("en-US")]

[assembly: HandlesSection(RetryPolicyConfigurationSettings.SectionName)]
[assembly: AddApplicationBlockCommand(
            RetryPolicyConfigurationSettings.SectionName,
            typeof(RetryPolicyConfigurationSettings),
            TitleResourceName = "AddRetryPolicyConfigurationSettings",
            TitleResourceType = typeof(DesignResources),
            CommandModelTypeName = TransientFaultHandlingDesignTime.CommandTypeNames.AddTransientFaultHandlingBlockCommand)]