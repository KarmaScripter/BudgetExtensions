// <copyright file="{ClassName}.cs" company="Terry D. Eppler">
// Copyright (c) Eppler. All rights reserved.
// </copyright>

namespace BudgetExecution
{
    // ********************************************************************************************************************************
    // *********************************************************  ASSEMBLIES   ********************************************************
    // ********************************************************************************************************************************

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;
    using Microsoft.Office.Interop.Excel;
    using DataTable = System.Data.DataTable;

    public static class EnumerableExtensions
    {
        // ***************************************************************************************************************************
        // ****************************************************     METHODS   ********************************************************
        // ***************************************************************************************************************************

        /// <summary>
        /// Converts to datatable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>( this IEnumerable<T> data )
        {
            if( data == null )
            {
                return default;
            }

            try
            {
                var table = new DataTable();
                PropertyInfo[] oprops = null;

                foreach( var rec in data )
                {
                    if( oprops == null )
                    {
                        oprops = rec.GetType().GetProperties();

                        foreach( var pi in oprops )
                        {
                            var coltype = pi.PropertyType;

                            if( coltype?.IsGenericType == true
                                && coltype.GetGenericTypeDefinition() == typeof( Nullable<> ) )
                            {
                                coltype = coltype.GetGenericArguments()[ 0 ];
                            }

                            table?.Columns?.Add( new DataColumn( pi.Name, coltype ) );
                        }
                    }

                    var dr = table?.NewRow();

                    foreach( var pi in oprops )
                    {
                        dr[ pi.Name ] = pi.GetValue( rec, null ) ?? DBNull.Value;
                    }

                    table?.Rows?.Add( dr );
                }

                return table;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary>
        /// Determines whether this instance has numeric.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        ///   <c>true</c> if the specified data has numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasNumeric( this IEnumerable<DataRow> data )
        {
            if( Verify.Input( data ) )
            {
                try
                {
                    var row = data?.First();
                    var dict = row?.ToDictionary();
                    var key = dict?.Keys.ToArray();
                    var names = Enum.GetNames( typeof( Numeric ) );

                    if( key != null )
                    {
                        foreach( var k in key )
                        {
                            if( names?.Contains( k ) == true )
                            {
                                return true;
                            }
                        }
                    }

                    return false;
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether [has primary key].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        ///   <c>true</c> if [has primary key] [the specified data]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasPrimaryKey( this IEnumerable<DataRow> data )
        {
            if( Verify.Input( data ) )
            {
                try
                {
                    var row = data?.First();
                    var dict = row?.ToDictionary();
                    var key = dict?.Keys.ToArray();
                    var names = Enum.GetNames( typeof( PrimaryKey ) );

                    if( key != null )
                    {
                        foreach( var k in key )
                        {
                            if( Verify.Input( k )
                                && names?.Contains( k ) == true )
                            {
                                return true;
                            }
                        }
                    }

                    return false;
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the primary key values.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static IEnumerable<int> GetPrimaryKeyValues( this IEnumerable<DataRow> data )
        {
            if( Verify.Input( data )
                && data.HasPrimaryKey() )
            {
                try
                {
                    var list = new List<int>();

                    foreach( var row in data )
                    {
                        if( row?.ItemArray[ 0 ] != null )
                        {
                            list?.Add( int.Parse( row.ItemArray[ 0 ]?.ToString() ) );
                        }
                    }

                    return list?.Any() == true
                        ? list.ToArray()
                        : default;
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default;
                }
            }

            return default;
        }

        /// <summary>
        /// Filters the specified field.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="field">The field.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static IEnumerable<DataRow> Filter( this IEnumerable<DataRow> data, Field field,
            string filter )
        {
            if( Verify.Input( data )
                && Enum.IsDefined( typeof( Field ), field )
                && Verify.Input( filter ) )
            {
                try
                {
                    var datarow = data?.First();
                    var dict = datarow.ToDictionary();
                    var columns = dict.Keys.ToArray();

                    if( columns?.Contains( $"{field}" ) == true )
                    {
                        var query = data?.Where( p => p.Field<string>( $"{field}" ).Equals( filter ) )
                            ?.Select( p => p );

                        return query?.Any() == true
                            ? query.ToArray()
                            : default;
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default;
                }
            }

            return default;
        }

        public static IEnumerable<DataRow> Filter( this IEnumerable<DataRow> data, string columnname,
            string filter )
        {
            if( Verify.Input( data )
                && Verify.Input( columnname )
                && Verify.Input( filter ) )
            {
                try
                {
                    var datarow = data?.First();
                    var dict = datarow.ToDictionary();
                    var columns = dict.Keys.ToArray();

                    if( columns?.Contains( columnname ) == true )
                    {
                        var query = data?.Where( p => p.Field<string>( columnname ).Equals( filter ) )
                            ?.Select( p => p );

                        return query?.Any() == true
                            ? query
                            : default;
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default;
                }
            }

            return default;
        }

        /// <summary>
        /// Filters the specified column.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="column">The column.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static IEnumerable<DataRow> Filter( this IEnumerable<DataRow> data, DataColumn column,
            string filter )
        {
            if( Verify.Input( data )
                && column != null
                && Verify.Input( filter ) )
            {
                try
                {
                    var datarow = data?.First();
                    var columns = datarow?.Table?.Columns;

                    if( columns?.Contains( column?.ColumnName ) == true )
                    {
                        var query = data?.Where( p => p.Field<string>( column.ColumnName ).Equals( filter ) )
                            ?.Select( p => p );

                        return query?.Any() == true
                            ? query.ToArray()
                            : default;
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default;
                }
            }

            return default;
        }

        /// <summary>
        /// Converts to excel.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="path">The path.</param>
        /// <exception cref="Exception">
        /// Invalid file path.
        /// or
        /// Invalid file path.
        /// or
        /// No data to export.
        /// </exception>
        public static void ToExcel<T>( this IEnumerable<T> data, string path )
        {
            if( string.IsNullOrEmpty( path ) )
            {
                throw new Exception( "Invalid file path." );
            }
            else if( path.ToLower().Contains( "" ) == false )
            {
                throw new Exception( "Invalid file path." );
            }

            if( data == null )
            {
                throw new Exception( "No data to export." );
            }

            // Optional argument variable
            object optionalvalue = Missing.Value;
            const string strheaderstart = "A2";
            const string strdatastart = "A3";

            try
            {
                var excelapp = new Application();
                var books = excelapp.Workbooks;
                var sheets = books.Add( optionalvalue ).Worksheets;
                var sheet = (_Worksheet)sheets?.Item[ 1 ];
                var objheaders = new Dictionary<string, string>();
                var headerinfo = typeof( T ).GetProperties();

                foreach( var property in headerinfo )
                {
                    var attribute = property.GetCustomAttributes( typeof( DisplayNameAttribute ), false )
                        .Cast<DisplayNameAttribute>()
                        .FirstOrDefault();

                    objheaders.Add( property.Name, attribute == null
                        ? property.Name
                        : attribute.DisplayName );
                }

                var range = sheet?.Range[ strheaderstart, optionalvalue ];
                range = range?.Resize[ 1, objheaders.Count ];

                if( range != null )
                {
                    range.Value[ optionalvalue ] = objheaders.Values.ToArray();

                    range.BorderAround( Type.Missing, XlBorderWeight.xlThin,
                        XlColorIndex.xlColorIndexAutomatic, Type.Missing );

                    var font = range.Font;
                    font.Bold = true;
                    range.Interior.Color = Color.LightGray.ToArgb();
                    var count = data.Count();
                    var objdata = new object[ count, objheaders.Count ];

                    for( var j = 0; j < count; j++ )
                    {
                        var item = data.ToArray()[ j ];
                        var i = 0;

                        foreach( var entry in objheaders )
                        {
                            var y = typeof( T ).InvokeMember( entry.Key, BindingFlags.GetProperty, null, item,
                                null );

                            objdata[ j, i++ ] = y?.ToString() ?? "";
                        }
                    }

                    range = sheet.Range[ strdatastart, optionalvalue ];
                    range = range.Resize[ count, objheaders.Count ];
                    range.Value[ optionalvalue ] = objdata;

                    range.BorderAround( Type.Missing, XlBorderWeight.xlThin,
                        XlColorIndex.xlColorIndexAutomatic, Type.Missing );

                    range = sheet.Range[ strheaderstart, optionalvalue ];
                    range = range.Resize[ count + 1, objheaders.Count ];
                }

                range?.Columns.AutoFit();

                if( string.IsNullOrEmpty( path ) == false )
                {
                    books.Add( optionalvalue ).SaveAs( path );
                }

                excelapp.Visible = true;
                books.Add( optionalvalue );

                try
                {
                    if( sheet != null )
                    {
                        Marshal.ReleaseComObject( sheet );
                    }

                    if( sheets != null )
                    {
                        Marshal.ReleaseComObject( sheets );
                    }

                    if( books.Add( optionalvalue ) != null )
                    {
                        Marshal.ReleaseComObject( books.Add( optionalvalue ) );
                    }

                    if( books != null )
                    {
                        Marshal.ReleaseComObject( books );
                    }

                    if( excelapp != null )
                    {
                        Marshal.ReleaseComObject( excelapp );
                    }
                }
                catch( Exception ex )
                {
                    using var error = new Error( ex );
                    error?.SetText();
                    error?.ShowDialog();
                }
                finally
                {
                    GC.Collect();
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Get Error Dialog.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private static void Fail( Exception ex )
        {
            using var error = new Error( ex );
            error?.SetText();
            error?.ShowDialog();
        }
    }
}