// <copyright file="{ClassName}.cs" company="Terry D. Eppler">
// Copyright (c) Eppler. All rights reserved.
// </copyright>

namespace BudgetExecution
{
    using System;

    public partial class ExtensionError
    {
        public ExtensionError()
        {
            InitializeComponent();
        }

        public ExtensionError( Exception ext )
        {
            InitializeComponent();
            Exception = ext;
        }

        public ExtensionError( string message )
        {
            InitializeComponent();
            Text = message;
        }

        public Exception Exception { get; }

        public new string Text { get; set; }

        public void SetText()
        {
        }
    }
}