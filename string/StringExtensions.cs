// <copyright file = "StringExtensions.cs" company = "Terry D. Eppler">
// Copyright (c) Terry D. Eppler. All rights reserved.
// </copyright>

namespace BudgetExecution
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ] 
    public static class StringExtensions
    {
        /// <summary>Splits the pascal.</summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string SplitPascal( this string text )
        {
            try
            {
                return string.IsNullOrEmpty( text ) || text.Length < 5
                    ? text
                    : Regex.Replace( text, "([A-Z])", " $1", RegexOptions.Compiled ).Trim();
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( string );
            }
        }

        /// <summary>Converts to propercase.</summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        [ SuppressMessage( "ReSharper", "UnusedMethodReturnValue.Global" ) ]
        public static string ToProperCase( this string str )
        {
            if( !string.IsNullOrEmpty( str ) )
            {
                try
                {
                    var _cultureInfo = Thread.CurrentThread.CurrentCulture;
                    var _textInfo = _cultureInfo.TextInfo;

                    return !string.IsNullOrEmpty( _textInfo.ToTitleCase( str ) )
                        ? _textInfo.ToTitleCase( str )
                        : default( string );
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return default( string );
                }
            }

            return default( string );
        }

        /// <summary>Ifs the null then.</summary>
        /// <param name="str">The string.</param>
        /// <param name="alt">The alt.</param>
        /// <returns></returns>
        public static string IfNullThen( this string str, string alt )
        {
            try
            {
                return str ?? alt ?? string.Empty;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( string );
            }
        }

        /// <summary>Converts to enum.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static T ToEnum<T>( this string str ) where T : struct
        {
            try
            {
                return (T)Enum.Parse( typeof( T ), str, true );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( T );
            }
        }

        /// <summary>Lasts the specified length.</summary>
        /// <param name="str">The string.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string Last( this string str, int length )
        {
            try
            {
                return str?.Length > length
                    ? str.Substring( str.Length - length )
                    : str;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( string );
            }
        }

        /// <summary>Firsts the specified length.</summary>
        /// <param name="str">The string.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string First( this string str, int length )
        {
            try
            {
                return str?.Length > length
                    ? str.Substring( 0, length )
                    : str;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( string );
            }
        }

        /// <summary>Firsts to upper.</summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string FirstToUpper( this string str )
        {
            try
            {
                if( !string.IsNullOrEmpty( str ) )
                {
                    var _chars = str.ToCharArray();
                    _chars[ 0 ] = char.ToUpper( _chars[ 0 ] );
                    return new string( _chars );
                }
                else
                {
                    return default( string );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( string );
            }
        }

        /// <summary>Converts to datetime.</summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static DateTime ToDateTime( this string str )
        {
            try
            {
                return DateTime.Parse( str );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( DateTime );
            }
        }

        /// <summary>Gets the memory stream.</summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static MemoryStream GetMemoryStream( this string str )
        {
            try
            {
                var _buffer = Encoding.UTF8.GetBytes( str );
                return new MemoryStream( _buffer );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( MemoryStream );
            }
        }

        /// <summary>Words the count.</summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static int WordCount( this string str )
        {
            var _count = 0;

            try
            {
                // Exclude whitespaces, Tabs and line breaks
                var _regex = new Regex( @"[^\s]+" );
                var _matches = _regex.Matches( str );
                _count = _matches.Count;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return _count;
            }

            return _count;
        }

        /// <summary>Gets the stream reader.</summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static StreamReader GetStreamReader( this string str )
        {
            try
            {
                return !string.IsNullOrEmpty( str )
                    ? new StreamReader( str )
                    : default( StreamReader );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( StreamReader );
            }
        }

        /// <summary>Writes to file.</summary>
        /// <param name="str">The string.</param>
        /// <param name="filetext">The filetext.</param>
        public static void WriteToFile( this string str, string filetext )
        {
            if( !string.IsNullOrEmpty( str )
                && !string.IsNullOrEmpty( filetext ) )
            {
                try
                {
                    using var _writer = new StreamWriter( str, false );
                    _writer.Write( filetext );
                }
                catch( Exception ex )
                {
                    Fail( ex );
                }
            }
        }

        /// <summary>Emails the specified subject.</summary>
        /// <param name="body">The body.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="server">The server.</param>
        /// <returns></returns>
        public static bool Email( this string body, string subject, string sender,
            string recipient, string server )
        {
            try
            {
                var _message = new MailMessage();
                _message.To.Add( recipient );
                var _address = new MailAddress( sender );
                _message.From = _address;
                _message.Subject = subject;
                _message.Body = body;
                var _client = new SmtpClient( server );
                var _credentials = new NetworkCredential();
                _client.Credentials = _credentials;
                _client.Send( _message );
                return true;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return false;
            }
        }

        /// <summary>Removes the spaces.</summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string RemoveSpaces( this string s )
        {
            if( !string.IsNullOrEmpty( s )
                && s.Contains( " " ) )
            {
                try
                {
                    return s.Replace( " ", "" );
                }
                catch( Exception ex )
                {
                    Fail( ex );
                    return s;
                }
            }

            return s;
        }

        /// <summary>Fails the specified ex.</summary>
        /// <param name="ex">The ex.</param>
        private static void Fail( Exception ex )
        {
            using var _error = new Error( ex );
            _error?.SetText();
            _error?.ShowDialog();
        }
    }
}
