// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

namespace EnterpriseLibrary.TransientFaultHandling.Configuration
{
    internal static class TransientFaultHandlingDesignTime
    {
        public static class ViewModelTypeNames
        {
            public const string RetryPolicyConfigurationSettingsViewModel =
                "EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.RetryPolicyConfigurationSettingsViewModel, EnterpriseLibrary.Configuration.DesignTime";

            public const string DefaultElementConfigurationProperty =
                "EnterpriseLibrary.Configuration.Design.ViewModel.DefaultElementConfigurationProperty, EnterpriseLibrary.Configuration.DesignTime";

            public const string TimeSpanElementConfigurationProperty =
                "EnterpriseLibrary.Configuration.Design.ViewModel.TimeSpanElementConfigurationProperty, EnterpriseLibrary.Configuration.DesignTime";
        }

        public static class CommandTypeNames
        {
            public const string WellKnownRetryStrategyElementCollectionCommand = "EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.WellKnownRetryStrategyElementCollectionCommand, EnterpriseLibrary.Configuration.DesignTime";

            public const string AddTransientFaultHandlingBlockCommand = "EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.AddTransientFaultHandlingBlockCommand, EnterpriseLibrary.Configuration.DesignTime";
        }

        /// <summary>
        /// Provides common editor types used by the design-time infrastructure.
        /// </summary>
        public static class EditorTypes
        {
            /// <summary>
            /// Type name of the TimeSpanEditor class, which is declared in the EnterpriseLibrary.Configuration.DesignTime assembly.
            /// </summary>
            public const string TimeSpanEditor = "EnterpriseLibrary.Configuration.Design.ComponentModel.Editors.TimeSpanEditorControl, EnterpriseLibrary.Configuration.DesignTime";

            /// <summary>
            /// Type name of the FrameworkElement, which is declared in the PresentationFramework assembly.
            /// </summary>
            public const string FrameworkElement = "System.Windows.FrameworkElement, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";
        }

        public static class ValidatorTypes
        {
            public const string NameValueCollectionValidator = "EnterpriseLibrary.Configuration.Design.Validation.NameValueCollectionValidator, EnterpriseLibrary.Configuration.DesignTime";

            public const string ExponentialBackoffValidator = "EnterpriseLibrary.Configuration.Design.Validation.BlockSpecifics.ExponentialBackoffValidator, EnterpriseLibrary.Configuration.DesignTime";
        }
    }
}
