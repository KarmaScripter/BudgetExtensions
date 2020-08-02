// <copyright file = "DataTableExt.cs" company = "Terry D. Eppler">
// Copyright (c) Terry D. Eppler. All rights reserved.
// </copyright>

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
        // ***************************************************************************************************************************
        // ************************************************  METHODS   ***************************************************************
        // ***************************************************************************************************************************

        /// <summary>
        /// The connection string
        /// </summary>
        public static readonly ConnectionStringSettingsCollection ConnectionString =
            ConfigurationManager.ConnectionStrings;

        /// <summary>
        /// Converts to xml.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="rootname">The rootname.</param>
        /// <returns></returns>
        public static XDocument ToXml( this DataTable dt, string rootname )
        {
            try
            {
                var xdoc = new XDocument
                {
                    Declaration = new XDeclaration( "1.0", "utf-8", "" )
                };

                xdoc.Add( new XElement( rootname ) );

                foreach( DataRow row in dt.Rows )
                {
                    var element = new XElement( dt.TableName );

                    foreach( DataColumn col in dt.Columns )
                    {
                        element.Add( new XElement( col.ColumnName, row?[ col ]?.ToString()?.Trim( ' ' ) ) );
                    }

                    xdoc.Root?.Add( element );
                }

                return xdoc;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary>
        /// Converts to excel.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="filepath">The filepath.</param>
        /// <exception cref="Exception">
        /// OSExportToExcelFile: Null or empty input datatable!\n
        /// or
        /// OSExportToExcelFile: Excel file could not be saved.\n"
        ///                             + ex.Message
        /// </exception>
        public static void ToExcel( this DataTable table, string filepath = null )
        {
            try
            {
                if( table == null )
                {
                    throw new Exception( "OSExportToExcelFile: Null or empty input datatable!\n" );
                }

                var excel = new ExcelPackage();
                var worksheet = excel?.Workbook?.Worksheets[ 0 ];

                for( var i = 0; i < table?.Columns?.Count; i++ )
                {
                    if( worksheet != null
                        && Verify.Input( table.Columns[ i ]?.ColumnName ) )
                    {
                        worksheet.Cells[ 1, i + 1 ].Value = table.Columns[ i ]?.ColumnName;
                    }
                }

                for( var i = 0; i < table.Rows?.Count; i++ )
                {
                    for( var j = 0; j < table.Columns?.Count; j++ )
                    {
                        if( worksheet != null )
                        {
                            worksheet.Cells[ i + 2, j + 1 ].Value = table.Rows[ i ][ j ];
                        }
                    }
                }

                if( Verify.Input( filepath ) )
                {
                    try
                    {
                        excel.Save( filepath );
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
        /// <param name="table">The table.</param>
        /// <param name="filepath">The filepath.</param>
        /// <param name="sheetname">The sheetname.</param>
        /// <returns></returns>
        public static DataTable FromExcel( this DataTable table, string filepath, string sheetname )
        {
            if( table != null
                && table.Columns.Count > 0
                && table.Rows.Count > 0
                && Verify.Input( filepath )
                && Verify.Input( sheetname ) )
            {
                try
                {
                    var connection = ConnectionString[ "Excel" ].ConnectionString;
                    var sql = "SELECT * FROM [" + sheetname + "$]";
                    using var adapter = new OleDbDataAdapter( sql, connection );
                    var datatable = new DataTable();
                    datatable.TableName = sheetname;
                    adapter?.FillSchema( datatable, SchemaType.Source );
                    adapter.Fill( datatable, datatable.TableName );
                    return datatable;
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
        /// Determines whether [has numeric column].
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns>
        ///   <c>true</c> if [has numeric column] [the specified table]; otherwise, <c>false</c>.
        /// </returns>
        [ SuppressMessage( "ReSharper", "UnusedVariable" ) ]
        public static bool HasNumericColumn( this DataTable table )
        {
            try
            {
                if( table != null
                    && table?.Rows?.Count > 0
                    && table.Columns?.Count > 0 )
                {
                    foreach( DataColumn k in table.Columns )
                    {
                        if( Verify.Input( k.ColumnName )
                            && Enum.GetNames( typeof( Numeric ) )?.Contains( k?.ColumnName ) == true )
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
                if( table != null
                    && table?.Rows?.Count > 0
                    && table.Columns?.Count > 0 )
                {
                    foreach( DataColumn column in table.Columns )
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
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static IEnumerable<int> GetPrimaryKeyValues( this DataTable table )
        {
            try
            {
                if( table != null
                    && table.Rows?.Count > 0
                    && table.Columns?.Count > 0 )
                {
                    var list = new List<int>();

                    foreach( var row in table.AsEnumerable() )
                    {
                        if( row?.HasPrimaryKey() == true )
                        {
                            list.Add( int.Parse( row[ 0 ].ToString() ) );
                        }
                    }

                    return list?.Any() == true
                        ? list
                        : default;
                }

                return default;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary>
        /// Gets the unique values.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public static string[] GetUniqueValues( this DataTable table, string column )
        {
            if( table.Rows.Count > 0
                && Verify.Input( column ) )
            {
                try
                {
                    var query = table?.AsEnumerable()?.Select( p => p.Field<string>( column ) )?.Distinct();
                    var enumerable = query as string[] ?? query.ToArray();

                    return enumerable.Any()
                        ? enumerable
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
        /// Gets the unique values.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public static string[] GetUniqueValues( this DataTable table, Field field )
        {
            if( table != null
                && table.Rows.Count > 0
                && Enum.IsDefined( typeof( Field ), field )
                && table.Columns.Contains( $"{field}" ) )
            {
                try
                {
                    var query = table?.AsEnumerable()
                        ?.Select( p => p.Field<string>( $"{field}" ) )
                        ?.Distinct();

                    var enumerable = query as string[] ?? query.ToArray();

                    return enumerable.Any()
                        ? enumerable
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
        /// <param name="table">The table.</param>
        /// <param name="field">The field.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static IEnumerable<DataRow> Filter( this DataTable table, Field field, string filter )
        {
            if( table != null
                && table.Columns.Count > 0
                && Enum.IsDefined( typeof( Field ), field )
                && Verify.Input( filter )
                && table.Columns.Contains( $"{field}" ) )
            {
                try
                {
                    var query = table?.AsEnumerable()
                        ?.Where( p => p.Field<string>( $"{field}" ).Equals( filter ) )
                        ?.Select( p => p );

                    return query?.Any() == true
                        ? query
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
        /// Gets the column names.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static string[] GetColumnNames( this DataTable table )
        {
            try
            {
                var fields = new string[ table.Columns.Count ];

                for( var i = 0; i < table.Columns.Count; i++ )
                {
                    fields[ i ] = table.Columns[ i ].ColumnName;
                }

                var cols = fields?.OrderBy( f => f.IndexOf( f ) )?.Select( f => f )?.ToArray();

                return cols.Any()
                    ? cols
                    : default;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary>
        /// Gets the index of the column name and.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static Dictionary<string, int> GetColumnNameAndIndex( this DataTable table )
        {
            try
            {
                var namindex = new Dictionary<string, int>();

                for( var i = 0; i < table.Columns.Count; i++ )
                {
                    namindex.Add( table.Columns[ i ].ColumnName, i );
                }

                return namindex.Count > 0
                    ? namindex
                    : default;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
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