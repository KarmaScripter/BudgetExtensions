// <copyright file = "DictionaryExt.cs" company = "Terry D. Eppler">
// Copyright (c) Terry D. Eppler. All rights reserved.
// </copyright>

namespace BudgetExecution
{
    // ******************************************************************************************************************************
    // ******************************************************   ASSEMBLIES   ********************************************************
    // ******************************************************************************************************************************

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
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

        /// <summary> Sorts the specified dictionary. </summary>
        /// <typeparam name = "TKey" > The type of the key. </typeparam>
        /// <typeparam name = "TValue" > The type of the str. </typeparam>
        /// <param name = "dict" > The dictionary. </param>
        /// <returns> </returns>
        /// <exception cref = "ArgumentNullException" > dict </exception>
        public static IDictionary<TKey, TValue> Sort<TKey, TValue>( this IDictionary<TKey, TValue> dict )
        {
            if( dict == null )
            {
                throw new ArgumentNullException( nameof( dict ) );
            }

            try
            {
                return new SortedDictionary<TKey, TValue>( dict );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary> Sorts the dictionary by str. </summary>
        /// <typeparam name = "TKey" > The type of the key. </typeparam>
        /// <typeparam name = "TValue" > The type of the str. </typeparam>
        /// <param name = "dict" > The dictionary. </param>
        /// <returns> </returns>
        public static IDictionary<TKey, TValue> SortByValue<TKey, TValue>( this IDictionary<TKey, TValue> dict )
        {
            try
            {
                return new SortedDictionary<TKey, TValue>( dict )?.OrderBy( kvp => kvp.Value )
                    ?.ToDictionary( item => item.Key, item => item.Value );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary> An IDictionary extension method that converts the @datarow to a name str collection. </summary>
        /// <param name = "nvc" > The NVC. </param>
        /// <returns> @datarow as a NameValueCollection. </returns>
        public static NameValueCollection ToNameValueCollection( this IDictionary<string, string> nvc )
        {
            try
            {
                if( nvc == null )
                {
                    return null;
                }

                var col = new NameValueCollection();

                foreach( var item in nvc )
                {
                    col.Add( item.Key, item.Value );
                }

                return col;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

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
            if( Verify.Input( dict ) )
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
            if( Verify.Input( dict ) 
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