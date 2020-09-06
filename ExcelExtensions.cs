// // <copyright file = "ExcelExtensions.cs" company = "Terry D. Eppler">
// // Copyright (c) Terry D. Eppler. All rights reserved.
// // </copyright>

namespace BudgetExecution
{
    // ********************************************************************************************************************************
    // *********************************************************  ASSEMBLIES   ********************************************************
    // ********************************************************************************************************************************

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Linq;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;
    using VisualPlus.Extensibility;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "CompareNonConstrainedGenericWithNull" ) ]
    public static class ExcelExtensions
    {
        // ***************************************************************************************************************************
        // ****************************************************    MEMBERS    ********************************************************
        // ***************************************************************************************************************************

        /// <summary>
        /// Converts to dataset.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="header">if set to <c>true</c> [header].</param>
        /// <returns></returns>
        public static DataSet ToDataSet( this ExcelPackage package, bool header = false )
        {
            var row = header
                ? 1
                : 0;

            return ToDataSet( package, row );
        }

        /// <summary>
        /// Converts to dataset.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="header">The header.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">header - Must be 0 or greater.</exception>
        public static DataSet ToDataSet( this ExcelPackage package, int header = 0 )
        {
            if( header < 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( header ), header, "Must be 0 or greater." );
            }

            var result = new DataSet();

            foreach( var sheet in package.Workbook.Worksheets )
            {
                var table = new DataTable
                {
                    TableName = sheet.Name
                };

                var startrow = 1;

                if( header > 0 )
                {
                    startrow = header;
                }

                var columns =
                    from cell in sheet.Cells[ startrow, 1, startrow,
                        sheet.Dimension.End.Column ] select new DataColumn( header > 0
                        ? cell.Value.ToString()
                        : $"Column {cell.Start.Column}" );

                table.Columns.AddRange( columns.ToArray() );

                var i = header > 0
                    ? startrow + 1
                    : startrow;

                for( var index = i; index <= sheet.Dimension.End.Row; index++ )
                {
                    var inputrow = sheet.Cells[ index, 1, index, sheet.Dimension.End.Column ];
                    var row = table.Rows.Add();

                    foreach( var cell in inputrow )
                    {
                        row[ cell.Start.Column - 1 ] = cell.Value;
                    }
                }

                result.Tables.Add( table );
            }

            return result;
        }

        /// <summary>
        /// Trims the last empty rows.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        public static void TrimLastEmptyRows( this ExcelWorksheet worksheet )
        {
            while( worksheet.IsLastRowEmpty() )
            {
                worksheet.DeleteRow( worksheet.Dimension.End.Row, 1 );
            }
        }

        /// <summary>
        /// Determines whether [is last row empty].
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <returns>
        ///   <c>true</c> if [is last row empty] [the specified worksheet]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLastRowEmpty( this ExcelWorksheet worksheet )
        {
            var empties = new List<bool>();

            for( var index = 1; index <= worksheet.Dimension.End.Column; index++ )
            {
                var value = worksheet.Cells[ worksheet.Dimension.End.Row, index ].Value;
                empties.Add( string.IsNullOrWhiteSpace( value?.ToString() ) );
            }

            return empties.All( e => e );
        }

        /// <summary>
        /// 
        /// </summary>
        public enum InsertMode
        {
            /// <summary>
            /// The row before
            /// </summary>
            RowBefore,

            /// <summary>
            /// The row after
            /// </summary>
            RowAfter,

            /// <summary>
            /// The column right
            /// </summary>
            ColumnRight,

            /// <summary>
            /// The column left
            /// </summary>
            ColumnLeft
        }

        /// <summary>
        /// Sets the width.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="width">The width.</param>
        public static void SetWidth( this ExcelColumn column, double width )
        {
            var num1 = width >= 1.0
                ? Math.Round( ( Math.Round( 7.0 * ( width - 0.0 ), 0 ) - 5.0 ) / 7.0, 2 )
                : Math.Round( ( Math.Round( 12.0 * ( width - 0.0 ), 0 ) - Math.Round( 5.0 * width, 0 ) ) / 12.0, 2 );

            var num2 = width - num1;

            var num3 = width >= 1.0
                ? Math.Round( 7.0 * num2 - 0.0, 0 ) / 7.0
                : Math.Round( 12.0 * num2 - 0.0, 0 ) / 12.0 + 0.0;

            if( num1 > 0.0 )
            {
                column.Width = width + num3;
            }
            else
            {
                column.Width = 0.0;
            }
        }

        /// <summary>
        /// Sets the height.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="height">The height.</param>
        public static void SetHeight( this ExcelRow row, double height )
        {
            row.Height = height;
        }

        /// <summary>
        /// Expands the column.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public static int[] ExpandColumn( this int[] index, int offset )
        {
            var column = index;
            column[ 3 ] += offset;
            return column;
        }

        /// <summary>
        /// Expands the row.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public static int[] ExpandRow( this int[] index, int offset )
        {
            var row = index;
            row[ 2 ] += offset;
            return row;
        }

        /// <summary>
        /// Moves the column.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public static int[] MoveColumn( this int[] index, int offset )
        {
            var column = index;
            column[ 1 ] += offset;
            column[ 3 ] += offset;
            return column;
        }

        /// <summary>
        /// Moves the row.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public static int[] MoveRow( this int[] index, int offset )
        {
            var row = index;
            row[ 0 ] += offset;
            row[ 2 ] += offset;
            return row;
        }

        /// <summary>
        /// Alls the border.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="borderstyle">The borderstyle.</param>
        public static void AllBorder( this ExcelRange range, ExcelBorderStyle borderstyle )
        {
            range.ForEach( r => r.Style.Border.BorderAround( borderstyle ) );
        }

        /// <summary>
        /// Backgrounds the color.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="color">The color.</param>
        /// <param name="fillstyle">The fillstyle.</param>
        public static void BackgroundColor( this ExcelRange range, Color color,
            ExcelFillStyle fillstyle = ExcelFillStyle.Solid )
        {
            range.Style.Fill.PatternType = fillstyle;
            range.Style.Fill.BackgroundColor.SetColor( color );
        }

        /// <summary>
        /// Fails the specified ex.
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
