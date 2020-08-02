// <copyright file = "DataGridExt.cs" company = "Terry D. Eppler">
// Copyright (c) Terry D. Eppler. All rights reserved.
// </copyright>

namespace BudgetExecution
{
    // ******************************************************************************************************************************
    // ******************************************************   ASSEMBLIES   ********************************************************
    // ******************************************************************************************************************************

    using System;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;

    /// <summary>
    /// 
    /// </summary>
    public static class DataGridExtensions
    {
        // ***************************************************************************************************************************
        // ************************************************  METHODS   ***************************************************************
        // ***************************************************************************************************************************

        /// <summary>
        /// The GetCurrentDataRow
        /// </summary>
        /// <param name="bindingsource">The bindingSource
        /// <see cref="BindingSource" /></param>
        /// <returns>
        /// The
        /// <see cref="System.Data.DataRow" />
        /// </returns>
        public static DataRow GetCurrentDataRow( this BindingSource bindingsource )
        {
            try
            {
                return ( (DataRowView)bindingsource?.Current )?.Row;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary>
        /// The GetDataTable
        /// </summary>
        /// <param name="datagridview">The datagridview
        /// <see cref="DataGridView" /></param>
        /// <returns>
        /// The
        /// <see cref="System.Data.DataTable" />
        /// </returns>
        public static DataTable GetDataTable( this DataGridView datagridview )
        {
            try
            {
                var dt = new DataTable();

                foreach( DataGridViewColumn column in datagridview.Columns )
                {
                    dt.Columns.Add( new DataColumn
                    {
                        ColumnName = column.Name,
                        DataType = column.ValueType
                    } );
                }

                foreach( DataGridViewRow row in datagridview.Rows )
                {
                    var cellvalues = new object[ row.Cells.Count ];

                    for( var i = 0; i < cellvalues.Length; i++ )
                    {
                        cellvalues[ i ] = row.Cells[ i ].Value;
                    }

                    dt.Rows.Add( cellvalues );
                }

                return dt;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary>
        /// The SetColumns
        /// </summary>
        /// <param name="datagridview">The datagridview</param>
        /// <param name="columns">The fields
        /// <see><cref> string[] </cref></see></param>
        /// <returns>
        /// The
        /// <see cref="System.Data.DataTable" />
        /// </returns>
        public static DataTable SetColumns( this DataGridView datagridview, string[] columns )
        {
            if( datagridview?.DataSource != null
                && columns != null
                && columns.Length > 0 )
            {
                try
                {
                    using var gridviewdatatable = datagridview.GetDataTable();
                    using var view = new DataView( gridviewdatatable );

                    if( gridviewdatatable?.Columns.Count > 0 )
                    {
                        var table = view.ToTable( true, columns );

                        return table?.Columns?.Count > 0
                            ? table
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
        /// The SetColumns
        /// </summary>
        /// <param name="datagridview">The datagridview
        /// <see cref="DataGridView" /></param>
        /// <param name="fields">The fields
        /// <see /></param>
        /// <returns>
        /// The
        /// <see cref="System.Data.DataTable" />
        /// </returns>
        public static DataTable SetColumns( this DataGridView datagridview, Field[] fields )
        {
            if( datagridview?.DataSource != null
                && fields != null
                && fields.Length > 0 )
            {
                try
                {
                    using var dstable = datagridview.GetDataTable();
                    using var view = new DataView( dstable );
                    using var dgvtable = datagridview.GetDataTable();

                    if( dgvtable?.Columns?.Count > 0 )
                    {
                        var columns = fields?.Select( f => f.ToString() )?.ToArray();
                        var table = view?.ToTable( true, columns );

                        return table?.Columns?.Count > 0
                            ? table
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
        /// The SetColumns
        /// </summary>
        /// <param name="datagridview">The datagridview
        /// <see cref="DataGridView" /></param>
        /// <param name="index">The index
        /// <see /></param>
        /// <returns>
        /// The
        /// <see cref="System.Data.DataTable" />
        /// </returns>
        public static DataTable SetColumns( this DataGridView datagridview, int[] index )
        {
            try
            {
                using var datatable = datagridview?.GetDataTable();

                if( datatable?.Columns?.Count > 0
                    && index?.Length > 0 )
                {
                    var datacolumns = datatable.Columns;
                    var columns = new string[ index.Length ];

                    if( datacolumns?.Count > 0
                        && columns?.Length > 0 )
                    {
                        for( var i = 0; i < index.Length; i++ )
                        {
                            columns[ i ] = datacolumns[ index[ i ] ].ColumnName;
                        }
                    }

                    using var view = new DataView( datatable );
                    var viewtable = view?.ToTable( true, columns );

                    return viewtable.Columns.Count > 0
                        ? viewtable
                        : default;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }

            return default;
        }

        /// <summary>
        /// The CommaDelimitedRows
        /// </summary>
        /// <param name="datagridview">The datagridview
        /// <see cref="DataGridView" /></param>
        /// <returns>
        /// The
        /// <see />
        /// </returns>
        public static string[] CommaDelimitedRows( this DataGridView datagridview )
        {
            if( datagridview?.RowCount > 0 )
            {
                try
                {
                    var list = new List<string>();

                    foreach( var row in datagridview.Rows )
                    {
                        if( !( (DataGridViewRow)row )?.IsNewRow == true )
                        {
                            var cells = ( (DataGridViewRow)row )?.Cells?.Cast<DataGridViewCell>()?.ToArray();

                            if( cells?.Any() == true )
                            {
                                var rowitem = string.Join( ",",
                                    Array.ConvertAll( ( (DataGridViewRow)row )?.Cells?.Cast<DataGridViewCell>()?.ToArray(),
                                        c => c.Value?.ToString() ?? string.Empty ) );

                                if( Verify.Input( rowitem ) )
                                {
                                    list?.Add( rowitem );
                                }
                            }
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
        /// The ExportToCommaDelimitedFile
        /// </summary>
        /// <param name="datagridview">The datagridview
        /// <see cref="DataGridView" /></param>
        /// <param name="filename">The filename
        /// <see cref="string" /></param>
        public static void ExportToCommaDelimitedFile( this DataGridView datagridview, string filename )
        {
            if( Verify.Input( filename )
                && datagridview != null )
            {
                try
                {
                    var path = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, filename );

                    if( Verify.Input( path ) )
                    {
                        File.WriteAllLines( path, datagridview.CommaDelimitedRows() );
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                }
            }
        }

        /// <summary>
        /// The ExpandColumns
        /// </summary>
        /// <param name="datagridview">The datagridview
        /// <see cref="DataGridView" /></param>
        public static void ExpandColumns( this DataGridView datagridview )
        {
            try
            {
                foreach( DataGridViewColumn col in datagridview.Columns )
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// The PascalizeHeaders
        /// </summary>
        /// <param name="datagridview">The datagridview
        /// <see cref="DataGridView" /></param>
        /// <param name="datatable">The datatable
        /// <see cref="System.Data.DataTable" /></param>
        public static void PascalizeHeaders( this DataGridView datagridview, DataTable datatable )
        {
            if( datagridview != null
                && datatable != null
                && datatable?.Columns?.Count > 0 )
            {
                try
                {
                    foreach( DataGridViewColumn column in datagridview.Columns )
                    {
                        if( Verify.Input( datatable.Columns[ column.Name ].Caption ) )
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

        /// <summary>
        /// The PascalizeHeaders
        /// </summary>
        /// <param name="datagridview">The datagridview
        /// <see cref="DataGridView" /></param>
        public static void PascalizeHeaders( this DataGridView datagridview )
        {
            if( datagridview?.DataSource != null )
            {
                try
                {
                    using var table = datagridview.GetDataTable();

                    if( table?.Columns?.Count > 0 )
                    {
                        foreach( DataGridViewColumn col in datagridview.Columns )
                        {
                            if( Verify.Input( table.Columns[ col.Name ].Caption ) )
                            {
                                col.HeaderText = table.Columns[ col.Name ].Caption;
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