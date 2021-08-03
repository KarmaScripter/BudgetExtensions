// // <copyright file = "DataTableExtensions.cs" company = "Terry D. Eppler">
// // Copyright (c) Terry D. Eppler. All rights reserved.
// // </copyright>

namespace BudgetExecution
{
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
    [SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "FunctionComplexityOverflow" ) ]
    [ SuppressMessage( "ReSharper", "UseObjectOrCollectionInitializer" ) ]
    [ SuppressMessage( "ReSharper", "StringIndexOfIsCultureSpecific.1" ) ]
    public static class DataTableExtensions
    {
        /// <summary>The connection string</summary>
        public static readonly ConnectionStringSettingsCollection ConnectionString =
            ConfigurationManager.ConnectionStrings;

        /// <summary>Converts to xml.</summary>
        /// <param name="dt">The dt.</param>
        /// <param name="rootName">Name of the root.</param>
        /// <returns></returns>
        public static XDocument ToXml( this DataTable dt, string rootName )
        {
            try
            {
                var _xdoc = new XDocument { Declaration = new XDeclaration( "1.0", "utf-8", "" ) };

                _xdoc.Add( new XElement( rootName ) );

                foreach( DataRow row in dt.Rows )
                {
                    var _element = new XElement( dt.TableName );

                    foreach( DataColumn col in dt.Columns )
                    {
                        _element.Add( new XElement( col.ColumnName,
                            row?[ col ]?.ToString()?.Trim( ' ' ) ) );
                    }

                    _xdoc.Root?.Add( _element );
                }

                return _xdoc;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( XDocument );
            }
        }

        /// <summary>Converts to excel.</summary>
        /// <param name="table">The table.</param>
        /// <param name="filePath">The file path.</param>
        /// <exception cref="Exception">
        /// OSExportToExcelFile: Null or empty input datatable!\n
        /// or
        /// OSExportToExcelFile: Excel file could not be saved.\n"
        ///                             + ex.Message
        /// </exception>
        public static void ToExcel( this DataTable table, string filePath = null )
        {
            try
            {
                if( table == null )
                {
                    throw new Exception( "OSExportToExcelFile: Null or empty input datatable!\n" );
                }

                var _excel = new ExcelPackage();
                var _worksheet = _excel?.Workbook?.Worksheets[ 0 ];

                for( var i = 0; i < table?.Columns?.Count; i++ )
                {
                    if( _worksheet != null
                        && !string.IsNullOrEmpty( table.Columns[ i ]?.ColumnName ) )
                    {
                        _worksheet.Cells[ 1, i + 1 ].Value = table.Columns[ i ]?.ColumnName;
                    }
                }

                for( var i = 0; i < table.Rows?.Count; i++ )
                {
                    for( var j = 0; j < table.Columns?.Count; j++ )
                    {
                        if( _worksheet != null )
                        {
                            _worksheet.Cells[ i + 2, j + 1 ].Value = table.Rows[ i ][ j ];
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

        /// <summary>Froms the excel.</summary>
        /// <param name="table">The table.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <returns></returns>
        public static DataTable FromExcel( this DataTable table, string filePath, string sheetName )
        {
            if( table?.Columns.Count > 0
                && table.Rows.Count > 0
                && !string.IsNullOrEmpty( filePath )
                && !string.IsNullOrEmpty( sheetName ) )
            {
                try
                {
                    var _connection = ConnectionString[ "Excel" ].ConnectionString;
                    var _sql = "SELECT * FROM [" + sheetName + "$]";
                    using var _adapter = new OleDbDataAdapter( _sql, _connection );
                    var _table = new DataTable();
                    _table.TableName = sheetName;
                    _adapter?.FillSchema( _table, SchemaType.Source );
                    _adapter.Fill( _table, _table.TableName );
                    return _table;
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
        /// <param name="table">The table.</param>
        /// <returns>
        ///   <c>true</c> if [has numeric column] [the specified table]; otherwise, <c>false</c>.
        /// </returns>
        [SuppressMessage( "ReSharper", "UnusedVariable" ) ]
        public static bool HasNumericColumn( this DataTable table )
        {
            try
            {
                if( table?.Rows?.Count > 0
                    && table.Columns?.Count > 0 )
                {
                    foreach( DataColumn k in table.Columns )
                    {
                        if( !string.IsNullOrEmpty( k.ColumnName )
                            && Enum.GetNames( typeof( Numeric ) )?.Contains( k?.ColumnName )
                            == true )
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
        /// <param name="table">The table.</param>
        /// <returns>
        ///   <c>true</c> if [has primary key] [the specified table]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasPrimaryKey( this DataTable table )
        {
            try
            {
                if( table?.Rows?.Count > 0
                    && table.Columns?.Count > 0 )
                {
                    foreach( DataColumn column in table.Columns )
                    {
                        if( Enum.GetNames( typeof( PrimaryKey ) )?.Contains( column?.ColumnName )
                            == true )
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

        /// <summary>Gets the primary key values.</summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static IEnumerable<int> GetPrimaryKeyValues( this DataTable table )
        {
            try
            {
                if( table?.Rows?.Count > 0
                    && table.Columns?.Count > 0 )
                {
                    var _list = new List<int>();

                    foreach( var row in table.AsEnumerable() )
                    {
                        if( row?.HasPrimaryKey() == true )
                        {
                            _list.Add( int.Parse( row[ 0 ].ToString() ) );
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

        /// <summary>Gets the unique values.</summary>
        /// <param name="table">The table.</param>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public static string[ ] GetUniqueValues( this DataTable table, string column )
        {
            if( table.Rows.Count > 0
                && !string.IsNullOrEmpty( column ) )
            {
                try
                {
                    var _query = table?.AsEnumerable()
                                     ?.Select( p => p.Field<string>( column ) )
                                     ?.Distinct();

                    var _enumerable = _query as string[ ] ?? _query.ToArray();

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

        /// <summary>Gets the unique values.</summary>
        /// <param name="table">The table.</param>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public static string[ ] GetUniqueValues( this DataTable table, Field field )
        {
            if( table?.Rows.Count > 0
                && Enum.IsDefined( typeof( Field ), field )
                && table.Columns.Contains( $"{field}" ) )
            {
                try
                {
                    var _query = table?.AsEnumerable()
                                     ?.Select( p => p.Field<string>( $"{field}" ) )
                                     ?.Distinct();

                    var _enumerable = _query as string[ ] ?? _query.ToArray();

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

        /// <summary>Filters the specified field.</summary>
        /// <param name="table">The table.</param>
        /// <param name="field">The field.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static IEnumerable<DataRow> Filter( this DataTable table, Field field,
            string filter )
        {
            if( table?.Columns.Count > 0
                && Enum.IsDefined( typeof( Field ), field )
                && !string.IsNullOrEmpty( filter )
                && table.Columns.Contains( $"{field}" ) )
            {
                try
                {
                    var _query = table?.AsEnumerable()
                            ?.Where( p => p.Field<string>( $"{field}" ).Equals( filter ) )
                            ?.Select( p => p );

                    return _query?.Any() == true
                        ? _query
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

        /// <summary>Gets the column names.</summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static string[ ] GetColumnNames( this DataTable table )
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

        /// <summary>Gets the index of the column name and.</summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static Dictionary<string, int> GetColumnNameAndIndex( this DataTable table )
        {
            try
            {
                var _index = new Dictionary<string, int>();

                for( var i = 0; i < table.Columns.Count; i++ )
                {
                    _index.Add( table.Columns[ i ].ColumnName, i );
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