// // <copyright file = "EnumerableExtensions.cs" company = "Terry D. Eppler">
// // Copyright (c) Terry D. Eppler. All rights reserved.
// // </copyright>

using OfficeOpenXml.Table;

namespace BudgetExecution
{
    // ********************************************************************************************************************************
    // *********************************************************  ASSEMBLIES   ********************************************************
    // ********************************************************************************************************************************

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using OfficeOpenXml;
    using TableStyles = TableStyles;

    [ SuppressMessage( "ReSharper", "MergeCastWithTypeCheck" ) ]
    [ SuppressMessage( "ReSharper", "AssignNullToNotNullAttribute" ) ]
    public static class EnumerableExtensions
    {
        // ***************************************************************************************************************************
        // ****************************************************     METHODS   ********************************************************
        // ***************************************************************************************************************************

        /// <summary>
        /// Determines whether this instance has numeric.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        ///   <c>true</c> if the specified data has numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasNumeric( this IEnumerable<DataRow> data )
        {
            if( data?.Any() == true )
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
            if( data?.Any() == true )
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
                            if( !string.IsNullOrEmpty( k )
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
            if( data?.Any() == true
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
        /// Filters the specified columnname.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="columnname">The columnname.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static IEnumerable<DataRow> Filter( this IEnumerable<DataRow> data, string columnname,
            string filter )
        {
            if( data?.Any() == true
                && !string.IsNullOrEmpty( columnname )
                && !string.IsNullOrEmpty( filter ) )
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
            if( data?.Any() == true
                && column != null
                && !string.IsNullOrEmpty( filter ) )
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
        /// <param name = "style" > </param>
        /// <exception cref="Exception">
        /// Invalid file path.
        /// or
        /// Invalid file path.
        /// or
        /// No data to export.
        /// </exception>
        public static ExcelPackage ToExcel<T>( this IEnumerable<T> data, string path,
            TableStyles style = TableStyles.Light1 )
        {
            if( string.IsNullOrEmpty( path )
                && data?.Any() == true
                && Enum.IsDefined( typeof( TableStyles ), style ) )
            {
                throw new ArgumentException();
            }

            try
            {
                using var excel = new ExcelPackage( new FileInfo( path ) );
                var workbook = excel.Workbook;
                var worksheet = workbook.Worksheets[ 0 ];
                var range = worksheet.Cells;
                range?.LoadFromCollection( data, true, style );
                return excel;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary>
        /// Slices the specified start.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public static IEnumerable<T> LazySlice<T>( this IEnumerable<T> data, int start, int end )
        {
            if( data?.Any() == true
                && start > 0
                && end > 0 )
            {
                var index = 0;

                foreach( var item in data )
                {
                    if( index >= end )
                    {
                        yield break;
                    }

                    if( index >= start )
                    {
                        yield return item;
                    }

                    ++index;
                }
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
