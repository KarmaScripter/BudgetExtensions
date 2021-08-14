// <copyright file = "DataTableExtensions.cs" company = "Terry D. Eppler">
// Copyright (c) Terry D. Eppler. All rights reserved.
// </copyright>

// ReSharper disable All
namespace BudgetExecution
{
    // ******************************************************************************************************************************
    // ******************************************************   ASSEMBLIES   ********************************************************
    // ******************************************************************************************************************************

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.OleDb;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Windows.Forms;
    using System.Xml.Linq;
    using OfficeOpenXml;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "FunctionComplexityOverflow" ) ]
    [ SuppressMessage( "ReSharper", "UseObjectOrCollectionInitializer" ) ]
    public static class DataTableExtensions
    {
        /// <summary>
        /// The connection string
        /// </summary>
        public static readonly ConnectionStringSettingsCollection ConnectionString =
            ConfigurationManager.ConnectionStrings;

        /// <summary>
        /// Converts to xml.
        /// </summary>
        /// <param name="dataTable">The dataTable.</param>
        /// <param name="rootName">The rootName.</param>
        /// <returns></returns>
        public static XDocument ToXml( this DataTable dataTable, string rootName )
        {
            try
            {
                var _xml = new XDocument
                {
                    Declaration = new XDeclaration( "1.0", "utf-8", "" )
                };

                _xml.Add( new XElement( rootName ) );

                foreach( DataRow row in dataTable.Rows )
                {
                    var element = new XElement( dataTable.TableName );

                    foreach( DataColumn col in dataTable.Columns )
                    {
                        element.Add( new XElement( col.ColumnName, row?[ col ]?.ToString()?.Trim( ' ' ) ) );
                    }

                    _xml.Root?.Add( element );
                }

                return _xml;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( XDocument );
            }
        }

        /// <summary>
        /// Converts to excel.
        /// </summary>
        /// <param name="dataTable">The dataTable.</param>
        /// <param name="filePath">The filePath.</param>
        /// <exception cref="Exception">
        /// OSExportToExcelFile: Null or empty input datatable!\n
        /// or
        /// OSExportToExcelFile: Excel file could not be saved.\n"
        ///                             + ex.Message
        /// </exception>
        public static void ToExcel( this DataTable dataTable, string filePath = null )
        {
            try
            {
                if( dataTable == null )
                {
                    throw new Exception( "OSExportToExcelFile: Null or empty input datatable!\n" );
                }

                var _excel = new ExcelPackage();
                var _worksheet = _excel?.Workbook?.Worksheets[ 0 ];

                for( var i = 0; i < dataTable?.Columns?.Count; i++ )
                {
                    if( _worksheet != null
                        && !string.IsNullOrEmpty( dataTable.Columns[ i ]?.ColumnName ) )
                    {
                        _worksheet.Cells[ 1, i + 1 ].Value = dataTable.Columns[ i ]?.ColumnName;
                    }
                }

                for( var i = 0; i < dataTable.Rows?.Count; i++ )
                {
                    for( var j = 0; j < dataTable.Columns?.Count; j++ )
                    {
                        if( _worksheet != null )
                        {
                            _worksheet.Cells[ i + 2, j + 1 ].Value = dataTable.Rows[ i ][ j ];
                        }
                    }
                }

                if( !string.IsNullOrEmpty( filePath ) )
                {
                    try
                    {
                        _excel.Save( filePath );
                        MessageBox.Show( "Excel file saved!" );
                    }
                    catch( Exception ex )
                    {
                        throw new Exception( "OSExportToExcelFile: Excel file could not be saved.\n"
                            + ex.Message );
                    }
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Froms the excel.
        /// </summary>
        /// <param name="dataTable">The dataTable.</param>
        /// <param name="filePath">The filePath.</param>
        /// <param name="sheetName">The sheetName.</param>
        /// <returns></returns>
        public static DataTable FromExcel( this DataTable dataTable, string filePath, string sheetName )
        {
            if( dataTable?.Columns.Count > 0
                && dataTable.Rows.Count > 0 
                && !string.IsNullOrEmpty( filePath ) 
                && !string.IsNullOrEmpty( sheetName ) )
            {
                try
                {
                    var _connection = ConnectionString[ "Excel" ].ConnectionString;
                    var _sql = "SELECT * FROM [" + sheetName + "$]";
                    using var _adapter = new OleDbDataAdapter( _sql, _connection );
                    var _dataTable = new DataTable();
                    _dataTable.TableName = sheetName;
                    _adapter?.FillSchema( _dataTable, SchemaType.Source );
                    _adapter.Fill( _dataTable, _dataTable.TableName );
                    return _dataTable;
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default( DataTable );
                }
            }

            return default( DataTable );
        }

        /// <summary>
        /// Determines whether [has numeric column].
        /// </summary>
        /// <param name="dataTable">The dataTable.</param>
        /// <returns>
        ///   <c>true</c> if [has numeric column] [the specified dataTable]; otherwise, <c>false</c>.
        /// </returns>
        [ SuppressMessage( "ReSharper", "UnusedVariable" ) ]
        public static bool HasNumericColumn( this DataTable dataTable )
        {
            try
            {
                if( dataTable?.Rows?.Count > 0
                    && dataTable.Columns?.Count > 0 )
                {
                    foreach( DataColumn _column in dataTable.Columns )
                    {
                        if( !string.IsNullOrEmpty( _column.ColumnName )
                            && Enum.GetNames( typeof( Numeric ) )?.Contains( _column?.ColumnName ) == true )
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
                return false;
            }
        }

        /// <summary>
        /// Determines whether [has primary key].
        /// </summary>
        /// <param name="dataTable">The dataTable.</param>
        /// <returns>
        ///   <c>true</c> if [has primary key] [the specified dataTable]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasPrimaryKey( this DataTable dataTable )
        {
            try
            {
                if( dataTable?.Rows?.Count > 0
                    && dataTable.Columns?.Count > 0 )
                {
                    foreach( DataColumn column in dataTable.Columns )
                    {
                        if( Enum.GetNames( typeof( PrimaryKey ) )?.Contains( column?.ColumnName ) == true )
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
                return false;
            }
        }

        /// <summary>
        /// Gets the primary key values.
        /// </summary>
        /// <param name="dataTable">The dataTable.</param>
        /// <returns></returns>
        public static IEnumerable<int> GetPrimaryKeyValues( this DataTable dataTable )
        {
            try
            {
                if( dataTable?.Rows?.Count > 0 
                    && dataTable.Columns?.Count > 0 )
                {
                    var _list = new List<int>();

                    foreach( var _row in dataTable.AsEnumerable() )
                    {
                        if( _row?.HasPrimaryKey() == true )
                        {
                            _list.Add( int.Parse( _row[ 0 ].ToString() ) );
                        }
                    }

                    return _list?.Any() == true
                        ? _list
                        : default( List<int> );
                }

                return default( IEnumerable<int> );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IEnumerable<int> );
            }
        }

        /// <summary>
        /// Gets the unique values.
        /// </summary>
        /// <param name="dataTable">The dataTable.</param>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public static string[] GetUniqueValues( this DataTable dataTable, string column )
        {
            if( dataTable.Rows.Count > 0
                && !string.IsNullOrEmpty( column ) )
            {
                try
                {
                    var _query = dataTable
                        ?.AsEnumerable()
                        ?.Select( p => p.Field<string>( column ) )
                        ?.Distinct();

                    var _enumerable = _query as string[] ?? _query.ToArray();

                    return _enumerable.Any()
                        ? _enumerable
                        : default( string[ ] );
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default( string[ ] );
                }
            }

            return default( string[ ] );
        }

        /// <summary>
        /// Gets the unique values.
        /// </summary>
        /// <param name="dataTable">The dataTable.</param>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public static string[] GetUniqueValues( this DataTable dataTable, Field field )
        {
            if( dataTable?.Rows.Count > 0 
                && Enum.IsDefined( typeof( Field ), field ) 
                && dataTable.Columns.Contains( $"{field}" ) )
            {
                try
                {
                    var _query = dataTable?.AsEnumerable()
                        ?.Select( p => p.Field<string>( $"{field}" ) )
                        ?.Distinct();

                    var _enumerable = _query as string[] ?? _query.ToArray();

                    return _enumerable.Any()
                        ? _enumerable
                        : default( string[ ] );
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default( string[ ] );
                }
            }

            return default( string[ ] );
        }

        /// <summary>
        /// Filters the specified field.
        /// </summary>
        /// <param name="dataTable">The dataTable.</param>
        /// <param name="field">The field.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static IEnumerable<DataRow> Filter( this DataTable dataTable, Field field, string filter )
        {
            if( dataTable?.Columns.Count > 0 
                && Enum.IsDefined( typeof( Field ), field ) 
                && !string.IsNullOrEmpty( filter ) 
                && dataTable.Columns.Contains( $"{field}" ) )
            {
                try
                {
                    var _select = dataTable
                        ?.AsEnumerable()
                        ?.Where( p => p.Field<string>( $"{field}" ).Equals( filter ) )
                        ?.Select( p => p );

                    return _select?.Any() == true
                        ? _select
                        : default( EnumerableRowCollection<DataRow> );
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default( IEnumerable<DataRow> );
                }
            }

            return default( IEnumerable<DataRow> );
        }

        /// <summary>
        /// Gets the column names.
        /// </summary>
        /// <param name="table">The dataTable.</param>
        /// <returns></returns>
        public static string[] GetColumnNames( this DataTable table )
        {
            try
            {
                var _fields = new string[ table.Columns.Count ];

                for( var i = 0; i < table.Columns.Count; i++ )
                {
                    _fields[ i ] = table.Columns[ i ].ColumnName;
                }

                var _names = _fields
                    ?.OrderBy( f => f.IndexOf( f ) )
                    ?.Select( f => f )?.ToArray();

                return _names.Any()
                    ? _names
                    : default( string[ ] );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( string[ ] );
            }
        }

        /// <summary>
        /// Gets the index of the column name and.
        /// </summary>
        /// <param name="dataTable">The dataTable.</param>
        /// <returns></returns>
        public static Dictionary<string, int> GetColumnNameAndIndex( this DataTable dataTable )
        {
            try
            {
                var _index = new Dictionary<string, int>();

                for( var i = 0; i < dataTable.Columns.Count; i++ )
                {
                    _index.Add( dataTable.Columns[ i ].ColumnName, i );
                }

                return _index.Count > 0
                    ? _index
                    : default( Dictionary<string, int> );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( Dictionary<string, int> );
            }
        }

        /// <summary>
        /// Get Error Dialog.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private static void Fail( Exception ex )
        {
            using var _error = new Error( ex );
            _error?.SetText();
            _error?.ShowDialog();
        }
    }
}
