// <copyright file = "DateTimeExtensions.cs" company = "Terry D. Eppler">
// Copyright (c) Terry D. Eppler. All rights reserved.
// </copyright>

namespace BudgetExecution
{
    // ******************************************************************************************************************************
    // ******************************************************   ASSEMBLIES   ********************************************************
    // ******************************************************************************************************************************

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;

    [ SuppressMessage( "ReSharper", "ConvertSwitchStatementToSwitchExpression" ) ]
    public static class DateTimeExtensions
    {
        // ***************************************************************************************************************************
        // ************************************************  METHODS   ***************************************************************
        // ***************************************************************************************************************************

        /// <summary>
        /// Verifies if the object is a date
        /// </summary>
        /// <param name = "dt" >
        /// The dt.
        /// </param>
        /// <returns>
        /// The <see cref = "bool"/>
        /// </returns>
        public static bool IsDate( this object dt )
        {
            return DateTime.TryParse( dt.ToString(), out _ );
        }

        /// <summary>
        /// Returns a date in the past by days.
        /// </summary>
        /// <param name = "days" >
        /// The days.
        /// </param>
        /// <returns>
        /// </returns>
        public static DateTime DaysAgo( this int days )
        {
            var t = new TimeSpan( days, 0, 0, 0 );
            return DateTime.Now.Subtract( t );
        }

        /// <summary>
        /// Returns a date in the future by days.
        /// </summary>
        /// <param name = "days" >
        /// The days.
        /// </param>
        /// <returns>
        /// </returns>
        public static DateTime DaysFromNow( this int days )
        {
            var t = new TimeSpan( days, 0, 0, 0 );
            return DateTime.Now.Add( t );
        }

        /// <summary>
        /// Returns a date in the past by hours.
        /// </summary>
        /// <param name = "hours" >
        /// The hours.
        /// </param>
        /// <returns>
        /// </returns>
        public static DateTime HoursAgo( this int hours )
        {
            var t = new TimeSpan( hours, 0, 0 );
            return DateTime.Now.Subtract( t );
        }

        /// <summary>
        /// Returns a date in the future by hours.
        /// </summary>
        /// <param name = "hours" >
        /// The hours.
        /// </param>
        /// <returns>
        /// </returns>
        public static DateTime HoursFromNow( this int hours )
        {
            var t = new TimeSpan( hours, 0, 0 );
            return DateTime.Now.Add( t );
        }

        /// <summary>
        /// Returns a date in the past by minutes
        /// </summary>
        /// <param name = "minutes" >
        /// The minutes.
        /// </param>
        /// <returns>
        /// </returns>
        public static DateTime MinutesAgo( this int minutes )
        {
            var t = new TimeSpan( 0, minutes, 0 );
            return DateTime.Now.Subtract( t );
        }

        /// <summary>
        /// Returns a date in the future by minutes.
        /// </summary>
        /// <param name = "minutes" >
        /// The minutes.
        /// </param>
        /// <returns>
        /// </returns>
        public static DateTime MinutesFromNow( this int minutes )
        {
            var t = new TimeSpan( 0, minutes, 0 );
            return DateTime.Now.Add( t );
        }

        /// <summary>
        /// Gets a date in the past according to seconds
        /// </summary>
        /// <param name = "seconds" >
        /// The seconds.
        /// </param>
        /// <returns>
        /// </returns>
        public static DateTime SecondsAgo( this int seconds )
        {
            var t = new TimeSpan( 0, 0, seconds );
            return DateTime.Now.Subtract( t );
        }

        /// <summary>
        /// Gets a date in the future by seconds.
        /// </summary>
        /// <param name = "seconds" >
        /// The seconds.
        /// </param>
        /// <returns>
        /// </returns>
        public static DateTime SecondsFromNow( this int seconds )
        {
            var t = new TimeSpan( 0, 0, seconds );
            return DateTime.Now.Add( t );
        }

        /// <summary>
        /// Checks to see if the date is a week day (Mon - Fri)
        /// </summary>
        /// <param name = "datetime" >
        /// The dt.
        /// </param>
        /// <returns>
        /// The <see cref = "bool"/>
        /// </returns>
        public static bool IsWeekDay( this DateTime datetime )
        {
            return datetime.DayOfWeek != DayOfWeek.Saturday && datetime.DayOfWeek != DayOfWeek.Sunday;
        }

        /// <summary>
        /// Checks to see if the date is Saturday or Sunday
        /// </summary>
        /// <returns>
        /// The <see cref = "bool"/>
        /// </returns>
        public static bool IsWeekEnd( this DateTime datetime )
        {
            return datetime.DayOfWeek == DayOfWeek.Saturday || datetime.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Counts the number of weekdays between two dates.
        /// </summary>
        /// <param name = "startdate" >
        /// The start time.
        /// </param>
        /// <param name = "enddate" >
        /// The end time.
        /// </param>
        /// <returns>
        /// </returns>
        public static int CountWeekDays( this DateTime startdate, DateTime enddate )
        {
            var ts = enddate - startdate;
            Console.WriteLine( ts.Days );
            var cnt = 0;

            for( var i = 0; i < ts.Days; i++ )
            {
                var dt = startdate.AddDays( i );

                if( dt.IsWeekDay() )
                {
                    cnt++;
                }
            }

            return cnt;
        }

        /// <summary>
        /// Counts the number of weekends between two dates.
        /// </summary>
        /// <param name = "startDate" >
        /// The start time.
        /// </param>
        /// <param name = "endDate" >
        /// The end time.
        /// </param>
        /// <returns>
        /// </returns>
        public static int CountWeekEnds( this DateTime startDate, DateTime endDate )
        {
            var _span = endDate - startDate;
            Console.WriteLine( _span.Days );
            var cnt = 0;

            for( var i = 0; i < _span.Days; i++ )
            {
                var _time = startDate.AddDays( i );

                if( _time.IsWeekEnd() )
                {
                    cnt++;
                }
            }

            return cnt;
        }

        /// <summary>
        /// Diffs the specified date.
        /// </summary>
        /// <param name = "firstDate" >
        /// The date one.
        /// </param>
        /// <param name = "secondDate" >
        /// The date two.
        /// </param>
        /// <returns>
        /// </returns>
        public static TimeSpan Diff( this DateTime firstDate, DateTime secondDate )
        {
            var _span = firstDate.Subtract( secondDate );
            return _span;
        }

        /// <summary>
        /// Returns a double indicating the number of days between two dates (past is
        /// negative)
        /// </summary>
        /// <param name = "firstDate" >
        /// The date one.
        /// </param>
        /// <param name = "secondDate" >
        /// The date two.
        /// </param>
        /// <returns>
        /// </returns>
        public static double DiffDays( this string firstDate, string secondDate )
        {
            return DateTime.TryParse( firstDate, out var dtone ) 
                && DateTime.TryParse( secondDate, out var dttwo )
                    ? dtone.Diff( dttwo ).TotalDays
                    : 0;
        }

        /// <summary>
        /// Returns a double indicating the number of days between two dates (past is
        /// negative)
        /// </summary>
        /// <param name = "firstDate" >
        /// The date one.
        /// </param>
        /// <param name = "datetwo" >
        /// The date two.
        /// </param>
        /// <returns>
        /// </returns>
        public static double DiffDays( this DateTime firstDate, DateTime datetwo )
        {
            return firstDate.Diff( datetwo ).TotalDays;
        }

        /// <summary>
        /// Returns a double indicating the number of days between two dates (past is
        /// negative)
        /// </summary>
        /// <param name = "firstDate" >
        /// The date one.
        /// </param>
        /// <param name = "datetwo" >
        /// The date two.
        /// </param>
        /// <returns>
        /// </returns>
        public static double DiffHours( this string firstDate, string datetwo )
        {
            return DateTime.TryParse( firstDate, out var dtone ) && DateTime.TryParse( datetwo, out var dttwo )
                ? dtone.Diff( dttwo ).TotalHours
                : 0;
        }

        /// <summary>
        /// Returns a double indicating the number of days between two dates (past is
        /// negative)
        /// </summary>
        /// <param name = "firstDate" >
        /// The date one.
        /// </param>
        /// <param name = "datetwo" >
        /// The date two.
        /// </param>
        /// <returns>
        /// </returns>
        public static double DiffHours( this DateTime firstDate, DateTime datetwo )
        {
            return firstDate.Diff( datetwo ).TotalHours;
        }

        /// <summary>
        /// Returns a double indicating the number of days between two dates (past is
        /// negative)
        /// </summary>
        /// <param name = "firstDate" >
        /// The date one.
        /// </param>
        /// <param name = "datetwo" >
        /// The date two.
        /// </param>
        /// <returns>
        /// </returns>
        public static double DiffMinutes( this string firstDate, string datetwo )
        {
            return DateTime.TryParse( firstDate, out var dtone ) && DateTime.TryParse( datetwo, out var dttwo )
                ? dtone.Diff( dttwo ).TotalMinutes
                : 0;
        }

        /// <summary>
        /// Returns a double indicating the number of days between two dates (past is
        /// negative)
        /// </summary>
        /// <param name = "firstDate" >
        /// The date one.
        /// </param>
        /// <param name = "datetwo" >
        /// The date two.
        /// </param>
        /// <returns>
        /// </returns>
        public static double DiffMinutes( this DateTime firstDate, DateTime datetwo )
        {
            return firstDate.Diff( datetwo ).TotalMinutes;
        }

        /// <summary>
        /// The IsFederalHoliday
        /// </summary>
        /// <param name = "date" >
        /// The date <see cref = "System.DateTime"/>
        /// </param>
        /// <returns>
        /// The <see cref = "bool"/>
        /// </returns>
        public static bool IsFederalHoliday( this DateTime date )
        {
            // to ease typing
            var _nthDay = (int)Math.Ceiling( date.Day / 7.0d );
            var _dayOfWeek = date.DayOfWeek;
            var _thursday = _dayOfWeek == DayOfWeek.Thursday;
            var _friday = _dayOfWeek == DayOfWeek.Friday;
            var _monday = _dayOfWeek == DayOfWeek.Monday;
            var _weekend = _dayOfWeek == DayOfWeek.Saturday || _dayOfWeek == DayOfWeek.Sunday;

            switch( date.Month )
            {
                // New Years Day (Jan 1, or preceding Friday/following Monday if weekend)
                case 12 when date.Day == 31 && _friday:
                case 1 when date.Day == 1 && !_weekend:
                case 1 when date.Day == 2 && _monday:

                // MLK day (3rd monday in January)
                case 1 when _monday && _nthDay == 3:

                // President’s Day (3rd Monday in February)
                case 2 when _monday && _nthDay == 3:

                // Memorial Day (Last Monday in May)
                case 5 when _monday && date.AddDays( 7 ).Month == 6:

                // Independence Day (July 4, or preceding Friday/following Monday if weekend)
                case 7 when date.Day == 3 && _friday:
                case 7 when date.Day == 4 && !_weekend:
                case 7 when date.Day == 5 && _monday:

                // Labor Day (1st Monday in September)
                case 9 when _monday && _nthDay == 1:

                // Columbus Day (2nd Monday in October)
                case 10 when _monday && _nthDay == 2:

                // Veteran’s Day (November 11, or preceding Friday/following Monday if weekend))
                case 11 when date.Day == 10 && _friday:
                case 11 when date.Day == 11 && !_weekend:
                case 11 when date.Day == 12 && _monday:

                // Thanksgiving Day (4th Thursday in November)
                case 11 when _thursday && _nthDay == 4:

                // Christmas Day (December 25, or preceding Friday/following Monday if weekend))
                case 12 when date.Day == 24 && _friday:
                case 12 when date.Day == 25 && !_weekend:
                case 12 when date.Day == 26 && _monday:
                    return true;

                default:
                    return false;
            }
        }
    }
}
