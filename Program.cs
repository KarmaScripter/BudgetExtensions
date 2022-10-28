// <copyright file = "Program.cs" company = "Terry D. Eppler">
// Copyright (c) Terry D. Eppler. All rights reserved.
// </copyright>
//

namespace BudgetExecution
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main( )
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run( );
        }
    }
}