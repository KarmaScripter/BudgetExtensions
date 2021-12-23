// <copyright file = "DataGridExtensions.cs" company = "Terry D. Eppler">
// Copyright (c) Terry D. Eppler. All rights reserved.
// </copyright>

namespace BudgetExecution
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// 
    /// </summary>
    public static class DataGridExtensions
    {
        /// <summary>Gets the current data row.</summary>
        /// <param name="bindingSource">The binding source.</param>
        /// <returns></returns>
        public static DataRow GetCurrentDataRow( this BindingSource bindingSource )
        {
            try
            {
                return ( (DataRowView)bindingSource?.Current )?.Row;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( DataRow );
            }
        }

        /// <summary>Gets the data table.</summary>
        /// <param name="dataGridView">The data grid view.</param>
        /// <returns></returns>
        public static DataTable GetDataTable( this DataGridView dataGridView )
        {
            try
            {
                var _table = new DataTable();

                foreach( DataGridViewColumn column in dataGridView.Columns )
                {
                    _table.Columns.Add( new DataColumn
                    {
                        ColumnName = column.Name,
                        DataType = column.ValueType
                    } );
                }

                foreach( DataGridViewRow row in dataGridView.Rows )
                {
                    var _values = new object[ row.Cells.Count ];

                    for( var i = 0; i < _values.Length; i++ )
                    {
                        _values[ i ] = row.Cells[ i ].Value;
                    }

                    _table.Rows.Add( _values );
                }

                return _table;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( DataTable );
            }
        }

        /// <summary>
        /// Sets the columns.
        /// </summary>
        /// <param name="dataGridView">
        /// The data grid view.
        /// </param>
        /// <param name="columns">
        /// The columns.
        /// </param>
        /// <returns></returns>
        public static DataTable SetColumns( this DataGridView dataGridView, string[ ] columns )
        {
            if( dataGridView?.DataSource != null 
                && columns?.Length > 0 )
            {
                try
                {
                    using var _gridTable = dataGridView.GetDataTable();
                    using var _dataView = new DataView( _gridTable );

                    if( _gridTable?.Columns.Count > 0 )
                    {
                        var _dataTable = _dataView.ToTable( true, columns );

                        return _dataTable?.Columns?.Count > 0
                            ? _dataTable
                            : default( DataTable );
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default( DataTable );
                }
            }

            return default( DataTable );
        }

        /// <summary>Sets the columns.</summary>
        /// <param name="dataGridView">The data grid view.</param>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        public static DataTable SetColumns( this DataGridView dataGridView, Field[ ] fields )
        {
            if( dataGridView?.DataSource != null
                && fields?.Length > 0 )
            {
                try
                {
                    using var _dataTable = dataGridView.GetDataTable();
                    using var _dataView = new DataView( _dataTable );
                    using var _gridTable = dataGridView.GetDataTable();

                    if( _gridTable?.Columns?.Count > 0 )
                    {
                        var _columns = fields?.Select( f => f.ToString() )?.ToArray();
                        var _table = _dataView?.ToTable( true, _columns );

                        return _table?.Columns?.Count > 0
                            ? _table
                            : default( DataTable );
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default( DataTable );
                }
            }

            return default( DataTable );
        }

        /// <summary>Sets the columns.</summary>
        /// <param name="dataGridView">The data grid view.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static DataTable SetColumns( this DataGridView dataGridView, int[ ] index )
        {
            try
            {
                using var _table = dataGridView?.GetDataTable();

                if( _table?.Columns?.Count > 0
                    && index?.Length > 0 )
                {
                    var _columns = _table.Columns;
                    var _names = new string[ index.Length ];

                    if( _columns?.Count > 0
                        && _names?.Length > 0 )
                    {
                        for( var i = 0; i < index.Length; i++ )
                        {
                            _names[ i ] = _columns[ index[ i ] ].ColumnName;
                        }
                    }

                    using var _dataView = new DataView( _table );
                    var _dataTable = _dataView?.ToTable( true, _names );

                    return _dataTable.Columns.Count > 0
                        ? _dataTable
                        : default( DataTable );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( DataTable );
            }

            return default( DataTable );
        }

        /// <summary>Commas the delimited rows.</summary>
        /// <param name="dataGridView">The data grid view.</param>
        /// <returns></returns>
        public static string[ ] CommaDelimitedRows( this DataGridView dataGridView )
        {
            if( dataGridView?.RowCount > 0 )
            {
                try
                {
                    var _list = new List<string>();

                    foreach( var row in dataGridView.Rows )
                    {
                        if( !( (DataGridViewRow)row )?.IsNewRow == true )
                        {
                            var _cells = ( (DataGridViewRow)row )?.Cells?.Cast<DataGridViewCell>()?.ToArray();

                            if( _cells?.Any() == true )
                            {
                                var _item = string.Join( ",",
                                    Array.ConvertAll( ( (DataGridViewRow)row )?.Cells?.Cast<DataGridViewCell>()?.ToArray(),
                                        c => c.Value?.ToString() ?? string.Empty ) );

                                if( !string.IsNullOrEmpty( _item ) )
                                {
                                    _list?.Add( _item );
                                }
                            }
                        }
                    }

                    return _list?.Any() == true
                        ? _list.ToArray()
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

        /// <summary>Exports to comma delimited file.</summary>
        /// <param name="dataGridView">The data grid view.</param>
        /// <param name="filename">The filename.</param>
        public static void ExportToCommaDelimitedFile( this DataGridView dataGridView, string filename )
        {
            if( !string.IsNullOrEmpty( filename )
                && dataGridView != null )
            {
                try
                {
                    var _path = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, filename );

                    if( !string.IsNullOrEmpty( _path ) )
                    {
                        File.WriteAllLines( _path, dataGridView.CommaDelimitedRows() );
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                }
            }
        }

        /// <summary>Expands the columns.</summary>
        /// <param name="dataGridView">The data grid view.</param>
        public static void ExpandColumns( this DataGridView dataGridView )
        {
            try
            {
                foreach( DataGridViewColumn col in dataGridView.Columns )
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>Pascalizes the headers.</summary>
        /// <param name="dataGridView">The data grid view.</param>
        /// <param name="datatable">The datatable.</param>
        public static void PascalizeHeaders( this DataGridView dataGridView, DataTable datatable )
        {
            if( dataGridView != null
                && datatable?.Columns?.Count > 0 )
            {
                try
                {
                    foreach( DataGridViewColumn column in dataGridView.Columns )
                    {
                        if( !string.IsNullOrEmpty( datatable.Columns[ column.Name ].Caption ) )
                        {
                            column.HeaderText = datatable.Columns[ column.Name ].Caption;
                        }
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                }
            }
        }

        /// <summary>Pascalizes the headers.</summary>
        /// <param name="dataGridView">The data grid view.</param>
        public static void PascalizeHeaders( this DataGridView dataGridView )
        {
            if( dataGridView?.DataSource != null )
            {
                try
                {
                    using var _table = dataGridView.GetDataTable();

                    if( _table?.Columns?.Count > 0 )
                    {
                        foreach( DataGridViewColumn col in dataGridView.Columns )
                        {
                            if( !string.IsNullOrEmpty( _table.Columns[ col.Name ].Caption ) )
                            {
                                col.HeaderText = _table.Columns[ col.Name ].Caption;
                            }
                        }
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                }
            }
        }

        /// <summary>Fails the specified ex.</summary>
        /// <param name="ex">The ex.</param>
        private static void Fail( Exception ex )
        {
            using var _error = new Error( ex );
            _error?.SetText( ex.Message );
            _error?.ShowDialog();
        }
    }
}
