// <copyright file = "FileStreamExtensions.cs" company = "Terry D. Eppler">
// Copyright (c) Terry D. Eppler. All rights reserved.
// </copyright>

namespace BudgetExecution
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// 
    /// </summary>
    [SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" )]
    public static class FileStreamExtensions
    {
        /// <summary>Iterates the lines.</summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static IEnumerable<string> IterateLines( this TextReader reader )
        {
            while( reader.ReadLine() != null )
            {
                yield return reader.ReadLine();
            }
        }

        /// <summary>Iterates the lines.</summary>
        /// <param name="reader">The reader.</param>
        /// <param name="action">The action.</param>
        public static void IterateLines( this TextReader reader, Action<string> action )
        {
            foreach( var line in reader.IterateLines() )
            {
                action( line );
            }
        }

        /// <summary>Gets the reader.</summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static StreamReader GetReader( this Stream stream )
        {
            return stream.GetReader( null );
        }

        /// <summary>Gets the reader.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Stream does not support reading.</exception>
        public static StreamReader GetReader( this Stream stream, Encoding encoding )
        {
            if( stream.CanRead == false )
            {
                throw new InvalidOperationException( "Stream does not support reading." );
            }

            encoding ??= Encoding.Default;
            return new StreamReader( stream, encoding );
        }

        /// <summary>Gets the writer.</summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static StreamWriter GetWriter( this Stream stream )
        {
            return stream.GetWriter( null );
        }

        /// <summary>Gets the writer.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Stream does not support writing.</exception>
        public static StreamWriter GetWriter( this Stream stream, Encoding encoding )
        {
            if( stream.CanWrite == false )
            {
                throw new InvalidOperationException( "Stream does not support writing." );
            }

            encoding ??= Encoding.Default;
            return new StreamWriter( stream, encoding );
        }

        /// <summary>Reads to end.</summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static string ReadToEnd( this Stream stream )
        {
            return stream.ReadToEnd( null );
        }

        /// <summary>Reads to end.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static string ReadToEnd( this Stream stream, Encoding encoding )
        {
            using var _reader = stream.GetReader( encoding );
            return _reader.ReadToEnd();
        }

        /// <summary>Seeks the beginning.</summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Stream does not support seeking.</exception>
        public static Stream SeekBeginning( this Stream stream )
        {
            if( stream.CanSeek == false )
            {
                throw new InvalidOperationException( "Stream does not support seeking." );
            }

            stream.Seek( 0, SeekOrigin.Begin );
            return stream;
        }

        /// <summary>Seeks the ending.</summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Stream does not support seeking.</exception>
        public static Stream SeekEnding( this Stream stream )
        {
            if( stream.CanSeek == false )
            {
                throw new InvalidOperationException( "Stream does not support seeking." );
            }

            stream.Seek( 0, SeekOrigin.End );
            return stream;
        }

        /// <summary>Copies to.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="targetstream">The targetstream.</param>
        /// <param name="buffersize">The buffersize.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// Source stream does not support reading.
        /// or
        /// Target stream does not support writing.
        /// </exception>
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

            var _buffer = new byte[ buffersize ];
            int _count;

            while( ( _count = stream.Read( _buffer, 0, buffersize ) ) > 0 )
            {
                targetstream.Write( _buffer, 0, _count );
            }

            return stream;
        }

        /// <summary>Copies to memory.</summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static MemoryStream CopyToMemory( this Stream stream )
        {
            using var _memory = new MemoryStream( (int)stream.Length );
            stream.CopyTo( _memory );
            return _memory;
        }

        /// <summary>Reads all bytes.</summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static IEnumerable<byte> ReadAllBytes( this Stream stream )
        {
            using var _memory = stream.CopyToMemory();
            return _memory.ToArray();
        }

        /// <summary>Reads the fixedbuffersize.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="bufsize">The bufsize.</param>
        /// <returns></returns>
        public static IEnumerable<byte> ReadFixedbuffersize( this Stream stream, int bufsize )
        {
            var _buffer = new byte[ bufsize ];
            var _offset = 0;

            do
            {
                var _read = stream.Read( _buffer, _offset, bufsize - _offset );

                if( _read == 0 )
                {
                    return null;
                }

                _offset += _read;
            }
            while( _offset < bufsize );

            return _buffer;
        }

        /// <summary>Writes the specified bytes.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="bytes">The bytes.</param>
        public static void Write( this Stream stream, byte[ ] bytes )
        {
            stream.Write( bytes, 0, bytes.Length );
        }
    }
}
