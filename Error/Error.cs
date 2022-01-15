// <copyright file = "Error.cs" company = "Terry D. Eppler">
// Copyright (c) Terry D. Eppler. All rights reserved.
// </copyright>

namespace BudgetExecution
{
    using System;
    using System.Drawing;
    using System.Drawing.Text;
    using System.Windows.Forms;
    using VisualPlus.Enumerators;
    using VisualPlus.Toolkit.Controls.Interactivity;

    internal partial class Error
    {
        public Error()
        {
            InitializeComponent();

            // 
            // Info
            // 
            Info.ForeColor = Color.Maroon;
            Info.Location = new Point( 42, 68 );
            Info.MouseState = MouseStates.Normal;
            Info.Name = "Info";
            Info.Orientation = Orientation.Horizontal;
            Info.Outline = false;
            Info.OutlineColor = Color.Red;
            Info.OutlineLocation = new Point( 0, 0 );
            Info.ReflectionColor = Color.FromArgb( 120, 0, 0, 0 );
            Info.ReflectionSpacing = 0;
            Info.ShadowColor = Color.Black;
            Info.ShadowDirection = 315;
            Info.ShadowLocation = new Point( 0, 0 );
            Info.ShadowOpacity = 100;
            Info.Size = new Size( 237, 23 );
            Info.TabIndex = 0;
            Info.Text = "Error Message";
            Info.TextAlignment = StringAlignment.Near;
            Info.TextLineAlignment = StringAlignment.Center;
            Info.TextStyle.Disabled = Color.FromArgb( 131, 129, 129 );
            Info.TextStyle.Enabled = Color.FromArgb( 0, 0, 0 );
            Info.TextStyle.Hover = Color.FromArgb( 0, 0, 0 );
            Info.TextStyle.Pressed = Color.FromArgb( 0, 0, 0 );
            Info.TextStyle.TextAlignment = StringAlignment.Center;
            Info.TextStyle.TextLineAlignment = StringAlignment.Center;
            Info.TextStyle.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // 
            // Error
            // 
            AutoScaleDimensions = new SizeF( 96F, 96F );
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb( 5, 5, 5 );
            BorderColor = Color.Maroon;
            CaptionBarColor = Color.FromArgb( 5, 5, 5 );
            CaptionFont = new Font( "Roboto", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0 );
            ClientSize = new Size( 650, 519 );
            Controls.Add( Info );
            Font = new Font( "Roboto", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0 );
            ForeColor = Color.LightGray;
            MetroColor = Color.FromArgb( 5, 5, 5 );
            Name = "Error";
            Text = "Error";
        }

        public Error( Exception ext )
        {
            InitializeComponent();
            Exception = ext;
        }

        public Error( string message )
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