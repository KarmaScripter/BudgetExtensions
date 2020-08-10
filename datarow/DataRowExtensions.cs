// <copyright file = "DataRowExt.cs" company = "Terry D. Eppler">
// Copyright (c) Terry D. Eppler. All rights reserved.
// </copyright>

namespace BudgetExecution
{
    // ******************************************************************************************************************************
    // ******************************************************   ASSEMBLIES   ********************************************************
    // ******************************************************************************************************************************

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Data.SQLite;
    using System.Data.SqlServerCe;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary> </summary>
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    public static class DataRowExtensions
    {
        // ***************************************************************************************************************************
        // ************************************************  METHODS   ***************************************************************
        // ***************************************************************************************************************************

        /// <summary> Converts to DataRow into Database Parameters. </summary>
        /// <param name = "datarow" > The datarow. </param>
        /// <param name = "provider" > The provider. </param>
        /// <returns> </returns>
        public static IEnumerable<DbParameter> ToSqlDbParameters( this DataRow datarow, Provider provider )
        {
            if( !Enum.IsDefined( typeof( Provider ), provider ) )
            {
                throw new InvalidEnumArgumentException( nameof( provider ), (int)provider,
                    typeof( Provider ) );
            }

            if( datarow?.ItemArray.Length > 0 
                && Enum.IsDefined( typeof( Provider ), provider ) )
            {
                try
                {
                    {
                        var table = datarow.Table;
                        var columns = table?.Columns;
                        var values = datarow.ItemArray;

                        switch( provider )
                        {
                            case Provider.SQLite:
                            {
                                var sqlite = new List<SQLiteParameter>();

                                for( var i = 0; i < columns?.Count; i++ )
                                {
                                    var parameter = new SQLiteParameter
                                    {
                                        SourceColumn = columns[ i ].ColumnName,
                                        Value = values[ i ]
                                    };

                                    sqlite.Add( parameter );
                                }

                                return sqlite?.Any() == true
                                    ? sqlite
                                    : default;
                            }

                            case Provider.SqlCe:
                            {
                                var sqlce = new List<SqlCeParameter>();

                                for( var i = 0; i < columns?.Count; i++ )
                                {
                                    var parameter = new SqlCeParameter
                                    {
                                        SourceColumn = columns[ i ].ColumnName,
                                        Value = values[ i ]
                                    };

                                    sqlce.Add( parameter );
                                }

                                return sqlce?.Any() == true
                                    ? sqlce
                                    : default;
                            }

                            case Provider.OleDb:
                            case Provider.Excel:
                            case Provider.Access:
                            {
                                var oledb = new List<OleDbParameter>();

                                for( var i = 0; i < columns?.Count; i++ )
                                {
                                    var parameter = new OleDbParameter
                                    {
                                        SourceColumn = columns[ i ].ColumnName,
                                        Value = values[ i ]
                                    };

                                    oledb.Add( parameter );
                                }

                                return oledb.Any()
                                    ? oledb
                                    : default;
                            }

                            case Provider.SqlServer:
                            {
                                var sqlserver = new List<SqlParameter>();

                                for( var i = 0; i < columns?.Count; i++ )
                                {
                                    var parameter = new SqlParameter
                                    {
                                        SourceColumn = columns[ i ].ColumnName,
                                        Value = values[ i ]
                                    };

                                    sqlserver.Add( parameter );
                                }

                                return sqlserver?.Any() == true
                                    ? sqlserver
                                    : default;
                            }
                        }

                        return default;
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

        /// <summary> Converts DataRow to Dictionary. </summary>
        /// <param name = "datarow" > The DataRow. </param>
        /// <returns> Dictionary </returns>
        public static IDictionary<string, object> ToDictionary( this DataRow datarow )
        {
            try
            {
                if( datarow?.ItemArray.Length > 0 )
                {
                    var dict = new Dictionary<string, object>();
                    var table = datarow?.Table;
                    var column = table?.Columns;
                    var items = datarow?.ItemArray;

                    for( var i = 0; i < column?.Count; i++ )
                    {
                        if( !string.IsNullOrEmpty( column[ i ]?.ColumnName ) )
                        {
                            dict?.Add( column[ i ].ColumnName, items[ i ] ?? default );
                        }
                    }

                    return dict?.Keys?.Count > 0
                        ? dict
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
        /// Converts to sortedlist.
        /// </summary>
        /// <param name="datarow">The datarow.</param>
        /// <returns></returns>
        public static SortedList<string, object> ToSortedList( this DataRow datarow )
        {
            try
            {
                if( datarow?.ItemArray.Length > 0 )
                {
                    var sortedlist = new SortedList<string, object>();
                    var table = datarow?.Table;
                    var column = table?.Columns;
                    var items = datarow?.ItemArray;

                    for( var i = 0; i < column?.Count; i++ )
                    {
                        if( !string.IsNullOrEmpty( column[ i ]?.ColumnName ) )
                        {
                            sortedlist?.Add( column[ i ].ColumnName, items[ i ] ?? default );
                        }
                    }

                    return sortedlist?.Count > 0
                        ? sortedlist
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

        /// <summary> Gets the record str casted as byte array. </summary>
        /// <param name = "row" > The data row. </param>
        /// <param name = "field" > The name of the record field. </param>
        /// <returns> The record str </returns>
        public static IEnumerable<byte> GetBytes( this DataRow row, string field )
        {
            try
            {
                return row[ field ] as byte[];
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary> Gets the field. </summary>
        /// <param name = "datarow" > The datarow. </param>
        /// <param name = "field" > The field. </param>
        /// <returns> </returns>
        public static string GetField( this DataRow datarow, Field field )
        {
            if( datarow != null
                && Enum.IsDefined( typeof( Field ), field ) )
            {
                var columns = datarow.Table?.GetColumnNames();

                if( columns?.Any() == true
                    && columns.Contains( $"{field}" ) )
                {
                    try
                    {
                        return datarow[ $"{field}" ].ToString();
                    }
                    catch( Exception ex )
                    {
                        Fail( ex );
                        return default;
                    }
                }
            }

            return default;
        }

        /// <summary> Gets the numeric. </summary>
        /// <param name = "datarow" > The datarow. </param>
        /// <param name = "numeric" > The numeric. </param>
        /// <returns> </returns>
        public static double GetNumeric( this DataRow datarow, Numeric numeric )
        {
            if( datarow != null & Enum.IsDefined( typeof( Numeric ), numeric ) )
            {
                var columns = datarow.Table?.GetColumnNames();

                if( columns?.Any() == true
                    && columns.Contains( $"{numeric}" ) )
                {
                    try
                    {
                        return double.Parse( datarow[ $"{numeric}" ].ToString() );
                    }
                    catch( Exception ex )
                    {
                        Fail( ex );
                        return 0.0;
                    }
                }
            }

            return 0.0;
        }

        /// <summary> Gets the date. </summary>
        /// <param name = "datarow" > The datarow. </param>
        /// <param name = "field" > The field. </param>
        /// <returns> </returns>
        public static DateTime GetDate( this DataRow datarow, Field field )
        {
            if( datarow != null & Enum.IsDefined( typeof( Field ), field ) )
            {
                var columns = datarow.Table?.GetColumnNames();

                if( columns != null
                    && columns?.Any() == true & columns.Contains( $"{field}" ) )
                {
                    try
                    {
                        return DateTime.Parse( datarow[ $"{field}" ].ToString() );
                    }
                    catch( Exception ex )
                    {
                        Fail( ex );
                        return default;
                    }
                }
            }

            return default;
        }

        /// <summary> Determines whether this instance has numeric. </summary>
        /// <param name = "row" > The row. </param>
        /// <returns>
        /// <c> true </c>
        /// if the specified row has numeric; otherwise,
        /// <c> false </c>
        /// .
        /// </returns>
        public static bool HasNumeric( this DataRow row )
        {
            if( row != null )
            {
                try
                {
                    var colums = row.Table?.GetColumnNames();
                    var names = Enum.GetNames( typeof( Numeric ) );

                    for( var i = 1; i < colums?.Length; i++ )
                    {
                        if( names.Contains( colums[ i ] ) )
                        {
                            return true;
                        }

                        if( !names.Contains( colums[ i ] ) )
                        {
                            return false;
                        }
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default;
                }
            }

            return false;
        }

        /// <summary> Determines whether [has a primary key]. </summary>
        /// <param name = "row" > The row. </param>
        /// <returns>
        /// <c> true </c>
        /// if [has primary key] [the specified row]; otherwise,
        /// <c> false </c>
        /// .
        /// </returns>
        public static bool HasPrimaryKey( this DataRow row )
        {
            if( row != null
                && row.ItemArray?.Length > 0 )
            {
                try
                {
                    var dict = row.ToDictionary();
                    var key = dict.Keys?.ToArray();
                    var names = Enum.GetNames( typeof( PrimaryKey ) );
                    var count = 0;

                    for( var i = 1; i < key.Length; i++ )
                    {
                        var name = key[ i ];

                        if( names.Contains( name ) )
                        {
                            count++;
                        }
                    }

                    return count > 0;
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return false;
                }
            }

            return false;
        }

        /// <summary> Gets the key. </summary>
        /// <param name = "row" > The row. </param>
        /// <returns> </returns>
        public static IDictionary<string, object> GetPrimaryKey( this DataRow row )
        {
            if( row?.ItemArray?.Length > 0 )
            {
                try
                {
                    var dict = row.ToDictionary();
                    var key = dict.Keys?.ToArray();
                    var names = Enum.GetNames( typeof( PrimaryKey ) );

                    for( var i = 1; i < key?.Length; i++ )
                    {
                        var name = key[ i ];

                        if( names.Contains( name ) )
                        {
                            return new Dictionary<string, object>
                            {
                                [ name ] = int.Parse( dict[ name ].ToString() )
                            };
                        }

                        if( !names.Contains( name ) )
                        {
                            return default;
                        }
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