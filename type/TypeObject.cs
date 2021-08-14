// <copyright file="TypeExtensions.cs" company="Terry D. Eppler">
// Copyright (c) Eppler. All rights reserved.
// </copyright>

namespace BudgetExecution
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Web.Script.Serialization;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Xml.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "CompareNonConstrainedGenericWithNull" ) ]
    public static class TypeObject
    {
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
                    using var _stream = new MemoryStream();
                    var _formatter = new BinaryFormatter();
                    _formatter.Serialize( _stream, input );
                    _stream.Position = 0;
                    return (T)_formatter.Deserialize( _stream );
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default( T );
                }
            }

            return default( T );
        }

        /// <summary>
        /// Converts to json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static string ToJson<T>( this T item )
        {
            var _encoding = Encoding.Default;
            var _serializer = new DataContractJsonSerializer( typeof( T ) );
            using var stream = new MemoryStream();
            _serializer.WriteObject( stream, item );
            var _json = _encoding.GetString( stream.ToArray() );
            return _json;
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
            var _formatter = new BinaryFormatter();
            using var _stream = new MemoryStream();
            _formatter.Serialize( _stream, @this );
            return Encoding.Default.GetString( _stream.ToArray() );
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
            var _formatter = new BinaryFormatter();
            using var _stream = new MemoryStream();
            _formatter.Serialize( _stream, @this );
            return encoding.GetString( _stream.ToArray() );
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
            var _serializer = new XmlSerializer( @this.GetType() );
            using var _writer = new StringWriter();
            _serializer.Serialize( _writer, @this );
            using var _reader = new StringReader( _writer.GetStringBuilder().ToString() );
            return _reader.ReadToEnd();
        }

        /// <summary>
        ///     A T extension method that serialize java script.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A string.</returns>
        public static string SerializeJavaScript<T>( this T @this )
        {
            var _serializer = new JavaScriptSerializer();
            return _serializer.Serialize( @this );
        }

        /// <summary>
        /// Get Error Dialog.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private static void Fail( Exception ex )
        {
            using var _error = new Error( ex );
            _error?.SetText();
            _error?.ShowDialog();
        }
    }
}
