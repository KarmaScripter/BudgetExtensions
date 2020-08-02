// <copyright file = "Verify.cs" company = "Terry D. Eppler">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    public class Verify 
    {
        // ***************************************************************************************************************************
        // ****************************************************    METHODS    ********************************************************
        // ***************************************************************************************************************************

        /// <summary>
        /// Datas the specified input.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static bool Table<T>( T input )
            where T : IListSource
        {
            if( !input?.ContainsListCollection == true )
            {
                Fail( new ArgumentException() );
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Rows the specified input.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static bool Row<T>( T input )
            where T : DataRow
        {
            if( !input?.ItemArray?.Any() == true )
            {
                Fail( new ArgumentException() );
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Inputs the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static bool Input( object input )
        {
            if( string.IsNullOrEmpty( input?.ToString() ) )
            {
                Fail( new ArgumentException() );
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Inputs the specified input.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static bool Rows<T>( T input ) 
            where T : IEnumerable<DataRow>
        {
            if( !input?.Any() == true )
            {
                Fail( new ArgumentException() );
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Maps the specified input.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static bool Map<T>( T input ) 
            where T : IDictionary<string, object>
        {
            if( !input?.Any() == true )
            {
                Fail( new ArgumentException() );
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Determines whether the specified input is bindable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if the specified input is bindable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBindable<T>( T input )
            where T : IBindingList
        {
            if( input?.IsEmpty() == true )
            {
                Fail( new ArgumentException() );
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Sequences the specified input.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static bool Sequence<T>( IEnumerable<T> input )
        {
            if( input?.Any() == true )
            {
                Fail( new ArgumentException() );
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Determines whether the specified source is outlay.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns>
        /// <c> true </c>
        /// if the specified source is outlay; otherwise,
        /// <c> false </c>
        /// .
        /// </returns>
        public static bool IsOutlay<T>( T source ) where T : struct
        {
            if( !Enum.IsDefined( typeof( Source ), source ) )
            {
                Fail( new ArgumentException() );
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Determines whether [is date time] [the specified date].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="date">The date.</param>
        /// <returns>
        /// <c> true </c>
        /// if [is date time] [the specified date]; otherwise,
        /// <c> false </c>
        /// .
        /// </returns>
        public static bool DateTime<T>( T date ) where T : struct
        {
            if( !System.DateTime.TryParse( date.ToString(), out _ ) )
            {
                Fail( new ArgumentException() );
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Nots the null.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static bool Ref( object input )
        {
            if( input == null )
            {
                Fail( new ArgumentException() );
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Providers the specified input.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static bool Provider<T>( T input ) where T : struct
        {
            if( !Enum.IsDefined( typeof( Provider ), input ) )
            {
                Fail( new ArgumentException() );
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Get Error Dialog.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private protected static void Fail( Exception ex )
        {
            using var error = new Error( ex );
            error?.SetText();
            error?.ShowDialog();
        }
    }
}