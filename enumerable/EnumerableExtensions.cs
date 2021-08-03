// <copyright file = "EnumerableExtensions.cs" company = "Terry D. Eppler">
// Copyright (c) Terry D. Eppler. All rights reserved.
// </copyright>

namespace BudgetExecution
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using OfficeOpenXml;
    using TableStyles = OfficeOpenXml.Table.TableStyles;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "MergeCastWithTypeCheck" ) ]
    [ SuppressMessage( "ReSharper", "AssignNullToNotNullAttribute" ) ]
    public static class EnumerableExtensions
    {
        /// <summary>Determines whether this instance has numeric.</summary>
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
                    var _row = data?.First();
                    var _dictionary = _row?.ToDictionary();
                    var _key = _dictionary?.Keys.ToArray();
                    var _names = Enum.GetNames( typeof( Numeric ) );

                    if( _key != null )
                    {
                        foreach( var k in _key )
                        {
                            if( _names?.Contains( k ) == true )
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
                    return default( bool );
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
                    var _row = data?.First();
                    var _dictionary = _row?.ToDictionary();
                    var _key = _dictionary?.Keys.ToArray();
                    var _names = Enum.GetNames( typeof( PrimaryKey ) );

                    if( _key != null )
                    {
                        foreach( var k in _key )
                        {
                            if( !string.IsNullOrEmpty( k )
                                && _names?.Contains( k ) == true )
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
                    return default( bool );
                }
            }

            return false;
        }

        /// <summary>Gets the primary key values.</summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static IEnumerable<int> GetPrimaryKeyValues( this IEnumerable<DataRow> data )
        {
            if( data?.Any() == true
                && data.HasPrimaryKey() )
            {
                try
                {
                    var _list = new List<int>();

                    foreach( var row in data )
                    {
                        if( row?.ItemArray[ 0 ] != null )
                        {
                            _list?.Add( int.Parse( row.ItemArray[ 0 ]?.ToString() ) );
                        }
                    }

                    return _list?.Any() == true
                        ? _list.ToArray()
                        : default( int[ ] );
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default( IEnumerable<int> );
                }
            }

            return default( IEnumerable<int> );
        }

        /// <summary>Filters the specified column name.</summary>
        /// <param name="data">The data.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static IEnumerable<DataRow> Filter( this IEnumerable<DataRow> data, string columnName,
            string filter )
        {
            if( data?.Any() == true
                && !string.IsNullOrEmpty( columnName )
                && !string.IsNullOrEmpty( filter ) )
            {
                try
                {
                    var _row = data?.First();
                    var _dictionary = _row.ToDictionary();
                    var _columns = _dictionary.Keys.ToArray();

                    if( _columns?.Contains( columnName ) == true )
                    {
                        var _select = data?.Where( p => p.Field<string>( columnName ).Equals( filter ) )
                            ?.Select( p => p );

                        return _select?.Any() == true
                            ? _select
                            : default( IEnumerable<DataRow> );
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default( IEnumerable<DataRow> );
                }
            }

            return default( IEnumerable<DataRow> );
        }

        /// <summary>Filters the specified column.</summary>
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
                    var _row = data?.First();

                    var _columns = _row
                        ?.Table
                        ?.Columns;

                    if( _columns?.Contains( column?.ColumnName ) == true )
                    {
                        var _select = data
                            ?.Where( p => p.Field<string>( column.ColumnName ).Equals( filter ) )
                            ?.Select( p => p );

                        return _select?.Any() == true
                            ? _select.ToArray()
                            : default( DataRow[ ] );
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default( IEnumerable<DataRow> );
                }
            }

            return default( IEnumerable<DataRow> );
        }

        /// <summary>Converts to excel.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="path">The path.</param>
        /// <param name="style">The style.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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
                using var _excel = new ExcelPackage( new FileInfo( path ) );
                var _workbook = _excel.Workbook;
                var _worksheet = _workbook.Worksheets[ 0 ];
                var _range = _worksheet.Cells;
                _range?.LoadFromCollection( data, true, style );
                return _excel;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( ExcelPackage );
            }
        }

        /// <summary>Lazies the slice.</summary>
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
                var _index = 0;

                foreach( var item in data )
                {
                    if( _index >= end )
                    {
                        yield break;
                    }

                    if( _index >= start )
                    {
                        yield return item;
                    }

                    ++_index;
                }
            }
        }

        /// <summary>Fails the specified ex.</summary>
        /// <param name="ex">The ex.</param>
        private static void Fail( Exception ex )
        {
            using var _error = new Error( ex );
            _error?.SetText();
            _error?.ShowDialog();
        }
    }
}
