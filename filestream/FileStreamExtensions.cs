// <copyright file = "FileStreamExtensions.cs" company = "Terry D. Eppler">
// Copyright (c) Terry D. Eppler. All rights reserved.
// </copyright>

namespace BudgetExecution
{
    // ******************************************************************************************************************************
    // ******************************************************   ASSEMBLIES   ********************************************************
    // ******************************************************************************************************************************

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using System.Threading;

    [SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" )]
    public static class FileStreamExtensions
    {
        // ***************************************************************************************************************************
        // ************************************************  METHODS   ***************************************************************
        // ***************************************************************************************************************************

        /// <summary>
        /// The method provides an iterator through all lines of the str reader.
        /// </summary>
        /// <param name = "reader" >
        /// The str reader.
        /// </param>
        /// <returns>
        /// The iterator
        /// </returns>
        public static IEnumerable<string> IterateLines( this TextReader reader )
        {
            while( reader.ReadLine() != null )
            {
                yield return reader.ReadLine();
            }
        }

        /// <summary>
        /// The method executes the passed delegate /lambda expression) for all lines of
        /// the str reader.
        /// </summary>
        /// <param name = "reader" >
        /// The str reader.
        /// </param>
        /// <param name = "action" >
        /// The action.
        /// </param>
        public static void IterateLines( this TextReader reader, Action<string> action )
        {
            foreach( var line in reader.IterateLines() )
            {
                action( line );
            }
        }

        /// <summary>
        /// Opens a StreamReader using the default encoding.
        /// </summary>
        /// <param name = "stream" >
        /// The stream.
        /// </param>
        /// <returns>
        /// The stream reader
        /// </returns>
        public static StreamReader GetReader( this Stream stream )
        {
            return stream.GetReader( null );
        }

        /// <summary>
        /// Opens a StreamReader using the specified encoding.
        /// </summary>
        /// <param name = "stream" >
        /// The stream.
        /// </param>
        /// <param name = "encoding" >
        /// The encoding.
        /// </param>
        /// <returns>
        /// The stream reader
        /// </returns>
        public static StreamReader GetReader( this Stream stream, Encoding encoding )
        {
            if( stream.CanRead == false )
            {
                throw new InvalidOperationException( "Stream does not support reading." );
            }

            encoding ??= Encoding.Default;
            return new StreamReader( stream, encoding );
        }

        /// <summary>
        /// Opens a StreamWriter using the default encoding.
        /// </summary>
        /// <param name = "stream" >
        /// The stream.
        /// </param>
        /// <returns>
        /// The stream writer
        /// </returns>
        public static StreamWriter GetWriter( this Stream stream )
        {
            return stream.GetWriter( null );
        }

        /// <summary>
        /// Opens a StreamWriter using the specified encoding.
        /// </summary>
        /// <param name = "stream" >
        /// The stream.
        /// </param>
        /// <param name = "encoding" >
        /// The encoding.
        /// </param>
        /// <returns>
        /// The stream writer
        /// </returns>
        public static StreamWriter GetWriter( this Stream stream, Encoding encoding )
        {
            if( stream.CanWrite == false )
            {
                throw new InvalidOperationException( "Stream does not support writing." );
            }

            encoding ??= Encoding.Default;
            return new StreamWriter( stream, encoding );
        }

        /// <summary>
        /// Reads all str from the stream using the default encoding.
        /// </summary>
        /// <param name = "stream" >
        /// The stream.
        /// </param>
        /// <returns>
        /// The result str.
        /// </returns>
        public static string ReadToEnd( this Stream stream )
        {
            return stream.ReadToEnd( null );
        }

        /// <summary>
        /// Reads all str from the stream using a specified encoding.
        /// </summary>
        /// <param name = "stream" >
        /// The stream.
        /// </param>
        /// <param name = "encoding" >
        /// The encoding.
        /// </param>
        /// <returns>
        /// The result str.
        /// </returns>
        public static string ReadToEnd( this Stream stream, Encoding encoding )
        {
            using var reader = stream.GetReader( encoding );
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Sets the stream cursor to the beginning of the stream.
        /// </summary>
        /// <param name = "stream" >
        /// The stream.
        /// </param>
        /// <returns>
        /// The stream
        /// </returns>
        public static Stream SeekBeginning( this Stream stream )
        {
            if( stream.CanSeek == false )
            {
                throw new InvalidOperationException( "Stream does not support seeking." );
            }

            stream.Seek( 0, SeekOrigin.Begin );
            return stream;
        }

        /// <summary>
        /// Sets the stream cursor to the end of the stream.
        /// </summary>
        /// <param name = "stream" >
        /// The stream.
        /// </param>
        /// <returns>
        /// The stream
        /// </returns>
        public static Stream SeekEnding( this Stream stream )
        {
            if( stream.CanSeek == false )
            {
                throw new InvalidOperationException( "Stream does not support seeking." );
            }

            stream.Seek( 0, SeekOrigin.End );
            return stream;
        }

        /// <summary>
        /// Copies one stream into a another one.
        /// </summary>
        /// <param name = "stream" >
        /// The source stream.
        /// </param>
        /// <param name = "targetstream" >
        /// The target stream.
        /// </param>
        /// <param name = "buffersize" >
        /// The buffer size used to read / write.
        /// </param>
        /// <returns>
        /// The source stream.
        /// </returns>
        public static Stream CopyTo( this Stream stream, Stream targetstream, int buffersize )
        {
            if( stream.CanRead == false )
            {
                throw new InvalidOperationException( "Source stream does not support reading." );
            }

            if( targetstream.CanWrite == false )
            {
                throw new InvalidOperationException( "Target stream does not support writing." );
            }

            var buffer = new byte[ buffersize ];
            int bytesread;

            while( ( bytesread = stream.Read( buffer, 0, buffersize ) ) > 0 )
            {
                targetstream.Write( buffer, 0, bytesread );
            }

            return stream;
        }

        /// <summary>
        /// Copies any stream into a local MemoryStream
        /// </summary>
        /// <param name = "stream" >
        /// The source stream.
        /// </param>
        /// <returns>
        /// The copied memory stream.
        /// </returns>
        public static MemoryStream CopyToMemory( this Stream stream )
        {
            using var memorystream = new MemoryStream( (int)stream.Length );
            stream.CopyTo( memorystream );
            return memorystream;
        }

        /// <summary>
        /// Reads the entire stream and returns an IEnumerable byte.
        /// </summary>
        /// <param name = "stream" >
        /// The stream.
        /// </param>
        /// <returns>
        /// The IEnumerable byte
        /// </returns>
        public static IEnumerable<byte> ReadAllBytes( this Stream stream )
        {
            using var memorystream = stream.CopyToMemory();
            return memorystream.ToArray();
        }

        /// <summary>
        /// Reads a fixed number of bytes.
        /// </summary>
        /// <param name = "stream" >
        /// The stream to read from
        /// </param>
        /// <param name = "bufsize" >
        /// The number of bytes to read.
        /// </param>
        /// <returns>
        /// the read byte[]
        /// </returns>
        public static IEnumerable<byte> ReadFixedbuffersize( this Stream stream, int bufsize )
        {
            var buf = new byte[ bufsize ];
            var offset = 0;

            do
            {
                var cnt = stream.Read( buf, offset, bufsize - offset );

                if( cnt == 0 )
                {
                    return null;
                }

                offset += cnt;
            }
            while( offset < bufsize );

            return buf;
        }

        /// <summary>
        /// Writes all passed bytes to the specified stream.
        /// </summary>
        /// <param name = "stream" >
        /// The stream.
        /// </param>
        /// <param name = "bytes" >
        /// The byte array / buffer.
        /// </param>
        public static void Write( this Stream stream, byte[ ] bytes )
        {
            stream.Write( bytes, 0, bytes.Length );
        }
    }
}
