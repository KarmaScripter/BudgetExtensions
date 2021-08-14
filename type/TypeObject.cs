// <copyright file="{ClassName}.cs" company="Terry D. Eppler">
// Copyright (c) Eppler. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;

namespace BudgetExecution
{
    // ********************************************************************************************************************************
    // *********************************************************  ASSEMBLIES   ********************************************************
    // ********************************************************************************************************************************

    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Web.Script.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [SuppressMessage( "ReSharper", "CompareNonConstrainedGenericWithNull" )]
    public static class TypeObject
    {
        // ***************************************************************************************************************************
        // ****************************************************    MEMBERS    ********************************************************
        // ***************************************************************************************************************************

        /// <summary>
        /// Copies the specified input.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static T Copy<T>( this T input ) where T : ISerializable
        {
            if( input != null )
            {
                try
                {
                    using var stream = new MemoryStream();
                    var formatter = new BinaryFormatter();
                    formatter.Serialize( stream, input );
                    stream.Position = 0;
                    return (T)formatter.Deserialize( stream );
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
        /// Converts to json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static string ToJson<T>( this T item )
        {
            var encoding = Encoding.Default;
            var serializer = new DataContractJsonSerializer( typeof( T ) );
            using var stream = new MemoryStream();
            serializer.WriteObject( stream, item );
            var json = encoding.GetString( stream.ToArray() );
            return json;
        }

        /// <summary>
        /// An object extension method that serialize an object to binary.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>
        /// A string.
        /// </returns>
        public static string SerializeBinary<T>( this T @this )
        {
            var write = new BinaryFormatter();
            using var stream = new MemoryStream();
            write.Serialize( stream, @this );
            return Encoding.Default.GetString( stream.ToArray() );
        }

        /// <summary>
        /// An object extension method that serialize an object to binary.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// A string.
        /// </returns>
        public static string SerializeBinary<T>( this T @this, Encoding encoding )
        {
            var write = new BinaryFormatter();
            using var stream = new MemoryStream();
            write.Serialize( stream, @this );
            return encoding.GetString( stream.ToArray() );
        }

        /// <summary>
        /// An object extension method that serialize a string to XML.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>
        /// The string representation of the Xml Serialization.
        /// </returns>
        public static string SerializeXml( this object @this )
        {
            var serializer = new XmlSerializer( @this.GetType() );
            using var writer = new StringWriter();
            serializer.Serialize( writer, @this );
            using var reader = new StringReader( writer.GetStringBuilder().ToString() );
            return reader.ReadToEnd();
        }

        /// <summary>
        ///     A T extension method that serialize java script.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A string.</returns>
        public static string SerializeJavaScript<T>( this T @this )
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize( @this );
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
