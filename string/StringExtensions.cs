// <copyright file = "StringExt.cs " company = "Terry D. Eppler">
// Copyright (c) Terry Eppler. All rights reserved.
// </copyright>

namespace BudgetExecution
{
    // ******************************************************************************************************************************
    // ******************************************************   ASSEMBLIES   ********************************************************
    // ******************************************************************************************************************************

    using System;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    public static class StringExtensions
    {
        // ***************************************************************************************************************************
        // ************************************************  METHODS   ***************************************************************
        // ***************************************************************************************************************************

        /// <summary>
        /// The SplitPascal
        /// </summary>
        /// <returns>
        /// The <see cref = "string"/>
        /// </returns>
        /// <param name = "text" >
        /// The text.
        /// </param>
        /// <returns>
        /// </returns>
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
                return default;
            }
        }

        /// <summary>
        /// The ToProperCase
        /// </summary>
        /// <param name = "str" >
        /// The str <see cref = "string"/>
        /// </param>
        /// <returns>
        /// The <see cref = "string"/>
        /// </returns>
        [ SuppressMessage( "ReSharper", "UnusedMethodReturnValue.Global" ) ]
        public static string ToProperCase( this string str )
        {
            if( !string.IsNullOrEmpty( str ) )
            {
                try
                {
                    var cultureinfo = Thread.CurrentThread.CurrentCulture;
                    var textinfo = cultureinfo.TextInfo;

                    return !string.IsNullOrEmpty( textinfo.ToTitleCase( str ) )
                        ? textinfo.ToTitleCase( str )
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
        /// The IfNullThen
        /// </summary>
        /// <param name = "str" >
        /// The str <see cref = "string"/>
        /// </param>
        /// <param name = "alt" >
        /// The alt <see cref = "string"/>
        /// </param>
        /// <returns>
        /// The <see cref = "string"/>
        /// </returns>
        public static string IfNullThen( this string str, string alt )
        {
            try
            {
                return str ?? alt ?? string.Empty;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary>
        /// The ToEnum
        /// </summary>
        /// <typeparam name = "T" >
        /// </typeparam>
        /// <param name = "str" >
        /// The str <see cref = "string"/>
        /// </param>
        /// <returns>
        /// The <see cref = "T"/>
        /// </returns>
        public static T ToEnum<T>( this string str ) where T : struct
        {
            try
            {
                return (T)Enum.Parse( typeof( T ), str, true );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary>
        /// The Right
        /// </summary>
        /// <param name = "str" >
        /// The str <see cref = "string"/>
        /// </param>
        /// <param name = "length" >
        /// The length <see cref = "int"/>
        /// </param>
        /// <returns>
        /// The <see cref = "string"/>
        /// </returns>
        public static string Last( this string str, int length )
        {
            try
            {
                return str != null && str.Length > length
                    ? str.Substring( str.Length - length )
                    : str;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary>
        /// The Left
        /// </summary>
        /// <param name = "str" >
        /// The str <see cref = "string"/>
        /// </param>
        /// <param name = "length" >
        /// The length <see cref = "int"/>
        /// </param>
        /// <returns>
        /// The <see cref = "string"/>
        /// </returns>
        public static string First( this string str, int length )
        {
            try
            {
                return str != null && str.Length > length
                    ? str.Substring( 0, length )
                    : str;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary>
        /// The FirstToUpper
        /// </summary>
        /// <param name = "str" >
        /// The theString <see cref = "string"/>
        /// </param>
        /// <returns>
        /// The <see cref = "string"/>
        /// </returns>
        public static string FirstToUpper( this string str )
        {
            try
            {
                if( !string.IsNullOrEmpty( str ) )
                {
                    var letters = str.ToCharArray();
                    letters[ 0 ] = char.ToUpper( letters[ 0 ] );
                    return new string( letters );
                }
                else
                {
                    return default;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary>
        /// The ToDateTime
        /// </summary>
        /// <param name = "str" >
        /// The str <see cref = "string"/>
        /// </param>
        /// <returns>
        /// The <see/>
        /// </returns>
        public static DateTime ToDateTime( this string str )
        {
            try
            {
                return DateTime.Parse( str );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary>
        /// The ToStream
        /// </summary>
        /// <param name = "str" >
        /// The source <see cref = "string"/>
        /// </param>
        /// <returns>
        /// The <see cref = "MemoryStream"/>
        /// </returns>
        public static MemoryStream GetMemoryStream( this string str )
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes( str );
                return new MemoryStream( bytes );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary>
        /// The WordCount
        /// </summary>
        /// <param name = "str" >
        /// The input <see cref = "string"/>
        /// </param>
        /// <returns>
        /// The <see cref = "int"/>
        /// </returns>
        public static int WordCount( this string str )
        {
            var count = 0;

            try
            {
                // Exclude whitespaces, Tabs and line breaks
                var re = new Regex( @"[^\s]+" );
                var matches = re.Matches( str );
                count = matches.Count;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return count;
            }

            return count;
        }

        /// <summary>
        /// Read a str file and obtain it's contents.
        /// </summary>
        /// <param name = "str" >
        /// The complete file path to write to.
        /// </param>
        /// <returns>
        /// String containing the content of the file.
        /// </returns>
        public static StreamReader GetStreamReader( this string str )
        {
            try
            {
                return !string.IsNullOrEmpty( str )
                    ? new StreamReader( str )
                    : default;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default;
            }
        }

        /// <summary>
        /// Writes out a str to a file.
        /// </summary>
        /// <param name = "str" >
        /// The complete file path to write to.
        /// </param>
        /// <param name = "filetext" >
        /// A String containing str to be written to the file.
        /// </param>
        public static void WriteToFile( this string str, string filetext )
        {
            if( !string.IsNullOrEmpty( str )
                && !string.IsNullOrEmpty( filetext ) )
            {
                try
                {
                    using var sw = new StreamWriter( str, false );
                    sw.Write( filetext );
                }
                catch( Exception ex )
                {
                    Fail( ex );
                }
            }
        }

        /// <summary>
        /// Send an email using the supplied string.
        /// </summary>
        /// <param name = "body" >
        /// String that will be used i the body of the email.
        /// </param>
        /// <param name = "subject" >
        /// Subject of the email.
        /// </param>
        /// <param name = "sender" >
        /// The email address from which the message was sent.
        /// </param>
        /// <param name = "recipient" >
        /// The receiver of the email.
        /// </param>
        /// <param name = "server" >
        /// The server from which the email will be sent.
        /// </param>
        /// <returns>
        /// A boolean value indicating the success of the email send.
        /// </returns>
        public static bool Email( this string body, string subject, string sender,
            string recipient, string server )
        {
            try
            {
                var message = new MailMessage();
                message.To.Add( recipient );
                var address = new MailAddress( sender );
                message.From = address;
                message.Subject = subject;
                message.Body = body;
                var client = new SmtpClient( server );
                var credentials = new NetworkCredential();
                client.Credentials = credentials;
                client.Send( message );
                return true;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return false;
            }
        }

        /// <summary>
        /// remove white space, not line end Useful when parsing user input such phone,
        /// price int.Parse("1 000 000".RemoveSpaces(),.....
        /// </summary>
        /// <param name = "s" >
        /// </param>
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