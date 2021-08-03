// <copyright file = "DictionaryExtensions.cs" company = "Terry D. Eppler">
// Copyright (c) Terry D. Eppler. All rights reserved.
// </copyright>

namespace BudgetExecution
{
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

    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public static class DictionaryExtensions
    {
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
                    return default( TValue );
                }
            }
            else
            {
                dict[ key ] = value;
            }

            return dict[ key ];
        }

        public static string Predicate( this IDictionary<string, object> dict, Logic logic = Logic.AND )
        {
            if( dict?.Any() == true
                && Enum.IsDefined( typeof( Logic ), logic ) )
            {
                try
                {
                    var _conjuction = logic.ToString();
                    var _sqlString = "";

                    if( dict.HasPrimaryKey() )
                    {
                        var _primaryKey = dict.GetPrimaryKey();

                        if( !string.IsNullOrEmpty( _primaryKey.Key )
                            & int.Parse( _primaryKey.Value.ToString() ) > -1 )
                        {
                            foreach( var kvp in dict )
                            {
                                _sqlString += $"{kvp.Key} = {kvp.Value}  {_conjuction} ";
                            }

                            var _sql = _sqlString.TrimEnd( $"  {_conjuction} ".ToCharArray() );
                            _sql += $" WHERE {_primaryKey.Key} = {int.Parse( _primaryKey.Value.ToString() )};";

                            return !string.IsNullOrEmpty( _sql )
                                ? _sql
                                : string.Empty;
                        }
                    }
                    else if( !dict.HasPrimaryKey() )
                    {
                        foreach( var kvp in dict )
                        {
                            _sqlString += $"{kvp.Key} = {kvp.Value} {_conjuction} ";
                        }

                        var _sql = _sqlString.TrimEnd( $" {_conjuction} ".ToCharArray() );

                        return !string.IsNullOrEmpty( _sql )
                            ? _sql
                            : string.Empty;
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

        public static SortedDictionary<TKey, TValue> ToSortedDictionary<TKey, TValue>( this IDictionary<TKey, TValue> nvc )
        {
            try
            {
                return new SortedDictionary<TKey, TValue>( nvc );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( SortedDictionary<TKey, TValue> );
            }
        }

        public static SortedList<TKey, TValue> ToSortedList<TKey, TValue>( this IDictionary<TKey, TValue> dict )
        {
            try
            {
                return new SortedList<TKey, TValue>( dict );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( SortedList<TKey, TValue> );
            }
        }

        public static IEnumerable<DbParameter> ToSqlDbParameters( this IDictionary<string, object> dict,
            Provider provider )
        {
            if( dict?.Keys?.Count > 0
                && Enum.IsDefined( typeof( Provider ), provider ) )
            {
                try
                {
                    var _columns = dict.Keys.ToArray();
                    var _values = dict.Values.ToArray();

                    switch( provider )
                    {
                        case Provider.NS:
                        case Provider.SQLite:
                        {
                            var _sqlite = new List<SQLiteParameter>();

                            for( var i = 0; i < _columns.Length; i++ )
                            {
                                var _parameter = new SQLiteParameter
                                {
                                    SourceColumn = _columns[ i ],
                                    Value = _values[ i ]
                                };

                                _sqlite.Add( _parameter );
                            }

                            return _sqlite.Any()
                                ? _sqlite.ToArray()
                                : default( SQLiteParameter[ ] );
                        }

                        case Provider.SqlCe:
                        {
                            var _sqlce = new List<SqlCeParameter>();

                            for( var i = 0; i < _columns.Length; i++ )
                            {
                                var _parameter = new SqlCeParameter
                                {
                                    SourceColumn = _columns[ i ],
                                    Value = _values[ i ]
                                };

                                _sqlce.Add( _parameter );
                            }

                            return _sqlce.Any()
                                ? _sqlce.ToArray()
                                : default( SqlCeParameter[ ] );
                        }

                        case Provider.OleDb:
                        case Provider.Excel:
                        case Provider.Access:
                        {
                            var _oledb = new List<OleDbParameter>();

                            for( var i = 0; i < _columns.Length; i++ )
                            {
                                var _parameter = new OleDbParameter
                                {
                                    SourceColumn = _columns[ i ],
                                    Value = _values[ i ]
                                };

                                _oledb.Add( _parameter );
                            }

                            return _oledb.Any()
                                ? _oledb.ToArray()
                                : default( OleDbParameter[ ] );
                        }

                        case Provider.SqlServer:
                        {
                            var _sqlserver = new List<SqlParameter>();

                            for( var i = 0; i < _columns.Length; i++ )
                            {
                                var _parameter = new SqlParameter
                                {
                                    SourceColumn = _columns[ i ],
                                    Value = _values[ i ]
                                };

                                _sqlserver.Add( _parameter );
                            }

                            return _sqlserver?.Any() == true
                                ? _sqlserver.ToArray()
                                : default( SqlParameter[ ] );
                        }
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default( IEnumerable<DbParameter> );
                }

                return default( IEnumerable<DbParameter> );
            }

            return default( IEnumerable<DbParameter> );
        }

        public static bool HasPrimaryKey( this IDictionary<string, object> dict )
        {
            if( dict?.Any() == true )
            {
                try
                {
                    var _array = dict.Keys?.ToArray();
                    var _names = Enum.GetNames( typeof( PrimaryKey ) );
                    var _count = 0;

                    for( var i = 1; i < _array.Length; i++ )
                    {
                        var name = _array[ i ];

                        if( _names.Contains( name ) )
                        {
                            _count++;
                        }
                    }

                    return _count > 0;
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default( bool );
                }
            }

            return false;
        }

        public static KeyValuePair<string, object> GetPrimaryKey( this IDictionary<string, object> dict )
        {
            if( dict?.Any() == true
                && dict.HasPrimaryKey() )
            {
                try
                {
                    var _names = Enum.GetNames( typeof( PrimaryKey ) );

                    foreach( var kvp in dict )
                    {
                        if( _names.Contains( kvp.Key ) )
                        {
                            return new KeyValuePair<string, object>( kvp.Key, kvp.Value );
                        }
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default( KeyValuePair<string, object> );
                }
            }

            return default( KeyValuePair<string, object> );
        }

        private static void Fail( Exception ex )
        {
            using var _error = new Error( ex );
            _error?.SetText();
            _error?.ShowDialog();
        }
    }
}
