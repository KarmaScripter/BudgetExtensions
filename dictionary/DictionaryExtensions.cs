// // <copyright file = "DictionaryExtensions.cs" company = "Terry D. Eppler">
// // Copyright (c) Terry D. Eppler. All rights reserved.
// // </copyright>

namespace BudgetExecution
{
    // ******************************************************************************************************************************
    // ******************************************************   ASSEMBLIES   ********************************************************
    // ******************************************************************************************************************************

    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Data.SQLite;
    using System.Data.SqlServerCe;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;

    /// <summary> </summary>
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public static class DictionaryExtensions
    {
        // ***************************************************************************************************************************
        // ************************************************  METHODS   ***************************************************************
        // ***************************************************************************************************************************

        /// <summary> Adds the or update. </summary>
        /// <typeparam name = "TKey" > The type of the key. </typeparam>
        /// <typeparam name = "TValue" > The type of the value. </typeparam>
        /// <param name = "dict" > The dictionary. </param>
        /// <param name = "key" > The key. </param>
        /// <param name = "value" > The value. </param>
        /// <returns> </returns>
        public static TValue AddOrUpdate<TKey, TValue>( this IDictionary<TKey, TValue> dict, TKey key,
            TValue value )
        {
            if( !dict.ContainsKey( key ) )
            {
                try
                {
                    dict.Add( new KeyValuePair<TKey, TValue>( key, value ) );
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default;
                }
            }
            else
            {
                dict[ key ] = value;
            }

            return dict[ key ];
        }

        /// <summary>
        /// Predicates the specified logic.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <param name="logic">The logic.</param>
        /// <returns></returns>
        public static string Predicate( this IDictionary<string, object> dict, Logic logic = Logic.AND )
        {
            if( dict?.Any() == true
                && Enum.IsDefined( typeof( Logic ), logic ) )
            {
                try
                {
                    switch( logic )
                    {
                        case Logic.AND:
                        {
                            var conjuction = logic.ToString();
                            var sqlstring = "";

                            foreach( var kvp in dict )
                            {
                                sqlstring += $"{kvp.Key} = {kvp.Value} {conjuction}";
                            }

                            var sql = sqlstring.TrimEnd( $" {conjuction}".ToCharArray() );

                            return !string.IsNullOrEmpty( sql )
                                ? sql
                                : string.Empty;
                        }

                        case Logic.OR:
                        {
                            var sqlstring = "";

                            foreach( var kvp in dict )
                            {
                                sqlstring += $"{kvp.Key} = {kvp.Value} OR";
                            }

                            var sql = sqlstring.TrimEnd( " OR".ToCharArray() );

                            return !string.IsNullOrEmpty( sql )
                                ? sql
                                : string.Empty;
                        }

                        default:
                        {
                            return string.Empty;
                        }
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return string.Empty;
                }
            }

            return string.Empty;
        }

        /// <summary> Converts to sorteddictionary. </summary>
        /// <typeparam name = "TKey" > The type of the key. </typeparam>
        /// <typeparam name = "TValue" > The type of the value. </typeparam>
        /// <param name = "nvc" > The this. </param>
        /// <returns> </returns>
        public static SortedDictionary<TKey, TValue> ToSortedDictionary<TKey, TValue>( this IDictionary<TKey, TValue> nvc )
        {
            try
            {
                return new SortedDictionary<TKey, TValue>( nvc );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary> Converts to sorteddictionary. </summary>
        /// <typeparam name = "TKey" > The type of the key. </typeparam>
        /// <typeparam name = "TValue" > The type of the value. </typeparam>
        /// <param name = "dict" > The dictionary. </param>
        /// <returns> </returns>
        public static SortedList<TKey, TValue> ToSortedList<TKey, TValue>( this IDictionary<TKey, TValue> dict )
        {
            try
            {
                return new SortedList<TKey, TValue>( dict );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary> Converts to sqldbparameters. </summary>
        /// <param name = "dict" > The dictionary. </param>
        /// <param name = "provider" > The provider. </param>
        /// <returns> </returns>
        public static IEnumerable<DbParameter> ToSqlDbParameters( this IDictionary<string, object> dict,
            Provider provider )
        {
            if( dict.Keys.Count > 0
                && Enum.IsDefined( typeof( Provider ), provider ) )
            {
                try
                {
                    var columns = dict.Keys.ToArray();
                    var values = dict.Values.ToArray();

                    switch( provider )
                    {
                        case Provider.None:
                        case Provider.SQLite:
                        {
                            var sqlite = new List<SQLiteParameter>();

                            for( var i = 0; i < columns.Length; i++ )
                            {
                                var parameter = new SQLiteParameter
                                {
                                    SourceColumn = columns[ i ],
                                    Value = values[ i ]
                                };

                                sqlite.Add( parameter );
                            }

                            return sqlite.Any()
                                ? sqlite.ToArray()
                                : default;
                        }

                        case Provider.SqlCe:
                        {
                            var sqlce = new List<SqlCeParameter>();

                            for( var i = 0; i < columns.Length; i++ )
                            {
                                var parameter = new SqlCeParameter
                                {
                                    SourceColumn = columns[ i ],
                                    Value = values[ i ]
                                };

                                sqlce.Add( parameter );
                            }

                            return sqlce.Any()
                                ? sqlce.ToArray()
                                : default;
                        }

                        case Provider.OleDb:
                        case Provider.Excel:
                        case Provider.Access:
                        {
                            var oledb = new List<OleDbParameter>();

                            for( var i = 0; i < columns.Length; i++ )
                            {
                                var parameter = new OleDbParameter
                                {
                                    SourceColumn = columns[ i ],
                                    Value = values[ i ]
                                };

                                oledb.Add( parameter );
                            }

                            return oledb.Any()
                                ? oledb.ToArray()
                                : default;
                        }

                        case Provider.SqlServer:
                        {
                            var sqlserver = new List<SqlParameter>();

                            for( var i = 0; i < columns.Length; i++ )
                            {
                                var parameter = new SqlParameter
                                {
                                    SourceColumn = columns[ i ],
                                    Value = values[ i ]
                                };

                                sqlserver.Add( parameter );
                            }

                            return sqlserver?.Any() == true
                                ? sqlserver.ToArray()
                                : default;
                        }
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default;
                }

                return default;
            }

            return default;
        }

        /// <summary> Determines whether [has a primary key]. </summary>
        /// <param name = "dict" > The row. </param>
        /// <returns>
        /// <c> true </c>
        /// if [has primary key] [the specified row]; otherwise,
        /// <c> false </c>
        /// .
        /// </returns>
        public static bool HasPrimaryKey( this IDictionary<string, object> dict )
        {
            if( dict?.Any() == true )
            {
                try
                {
                    var array = dict.Keys?.ToArray();
                    var names = Enum.GetNames( typeof( PrimaryKey ) );
                    var count = 0;

                    for( var i = 1; i < array.Length; i++ )
                    {
                        var name = array[ i ];

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
                    return default;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the primary key.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <returns></returns>
        public static KeyValuePair<string, object> GetPrimaryKey( this IDictionary<string, object> dict )
        {
            if( dict?.Any() == true
                && dict.HasPrimaryKey() )
            {
                try
                {
                    var names = Enum.GetNames( typeof( PrimaryKey ) );

                    foreach( var kvp in dict )
                    {
                        if( names.Contains( kvp.Key ) )
                        {
                            return new KeyValuePair<string, object>( kvp.Key, kvp.Value );
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
