// <copyright file="CollectionExt.cs" company="Terry D. Eppler">
// Copyright (c) Terry Eppler. All rights reserved.
// </copyright>

namespace BudgetExecution
{
    // ******************************************************************************************************************************
    // ******************************************************   ASSEMBLIES   ********************************************************
    // ******************************************************************************************************************************

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;

    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "CompareNonConstrainedGenericWithNull" ) ]
    public static class CollectionExtensions
    {
        // ***************************************************************************************************************************
        // ************************************************  METHODS   ***************************************************************
        // ***************************************************************************************************************************

        /// <summary>
        /// Adds if.
        /// </summary>
        /// <typeparam name = "T" >
        /// </typeparam>
        /// <param name = "collection" >
        /// The collection.
        /// </param>
        /// <param name = "predicate" >
        /// The predicate.
        /// </param>
        /// <param name = "value" >
        /// The value.
        /// </param>
        /// <returns>
        /// </returns>
        public static bool AddIf<T>( this ICollection<T> collection, Func<T, bool> predicate, T value )
        {
            if( collection?.Count > 0
                && predicate( value ) )
            {
                try
                {
                    collection.Add( value );
                    return true;
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name = "T" >
        /// </typeparam>
        /// <param name = "collection" >
        /// The collection.
        /// </param>
        /// <param name = "values" >
        /// The values.
        /// </param>
        public static void AddRange<T>( this ICollection<T> collection, params T[] values )
        {
            if( values?.Length > 0
                && collection?.Any() == true )
            {
                try
                {
                    foreach( var value in values )
                    {
                        if( value != null )
                        {
                            collection.Add( value );
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
        /// Determines whether this instance is empty.
        /// </summary>
        /// <typeparam name = "T" >
        /// </typeparam>
        /// <param name = "collection" >
        /// The collection.
        /// </param>
        /// <returns>
        /// <c>
        /// true
        /// </c>
        /// if the specified collection is empty; otherwise,
        /// <c>
        /// false
        /// </c>
        /// .
        /// </returns>
        public static bool IsEmpty<T>( this ICollection<T> collection )
        {
            try
            {
                return !( collection?.Count > 0 );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return false;
            }
        }

        /// <summary>
        /// Removes if contains.
        /// </summary>
        /// <typeparam name = "T" >
        /// </typeparam>
        /// <param name = "collection" >
        /// The collection.
        /// </param>
        /// <param name = "value" >
        /// The value.
        /// </param>
        public static void RemoveIfContains<T>( this ICollection<T> collection, T value )
        {
            if( collection?.Contains( value ) == true )
            {
                try
                {
                    collection.Remove( value );
                }
                catch( Exception ex )
                {
                    Fail( ex );
                }
            }
        }

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <typeparam name = "T" >
        /// </typeparam>
        /// <param name = "collection" >
        /// The collection.
        /// </param>
        /// <param name = "values" >
        /// The values.
        /// </param>
        public static void RemoveRange<T>( this ICollection<T> collection, params T[] values )
        {
            if( collection?.Any() == true
                && values?.Any() == true )
            {
                try
                {
                    foreach( var value in values )
                    {
                        collection.Remove( value );
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                }
            }
        }

        /// <summary>
        /// Remove an item from the collection with predicate
        /// </summary>
        /// <typeparam name = "T" >
        /// </typeparam>
        /// <param name = "collection" >
        /// </param>
        /// <param name = "predicate" >
        /// </param>
        public static void RemoveWhere<T>( this ICollection<T> collection, Predicate<T> predicate )
        {
            if( collection?.Any() == true )
            {
                try
                {
                    var list = collection
                        ?.Where( child => predicate( child ) )
                        ?.ToList();

                    if( list?.Any() == true )
                    {
                        list.ForEach( t => collection.Remove( t ) );
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                }
            }
        }

        /// <summary>
        /// Tests if the collection is empty.
        /// </summary>
        /// <param name = "collection" >
        /// The collection to test.
        /// </param>
        /// <returns>
        /// True if the collection is empty.
        /// </returns>
        public static bool IsEmpty( this ICollection collection )
        {
            try
            {
                return !( collection?.Count > 0 );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return true;
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