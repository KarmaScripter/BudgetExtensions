// <copyright file = "DateTimeExtensions.cs" company = "Terry D. Eppler">
// Copyright (c) Terry D. Eppler. All rights reserved.
// </copyright>

namespace BudgetExecution
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "ConvertSwitchStatementToSwitchExpression" ) ]
    public static class DateTimeExtensions
    {
        /// <summary>Determines whether this instance is date.</summary>
        /// <param name="dt">The dt.</param>
        /// <returns>
        ///   <c>true</c> if the specified dt is date; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDate( this object dt )
        {
            return DateTime.TryParse( dt.ToString(), out _ );
        }

        /// <summary>Dayses the ago.</summary>
        /// <param name="days">The days.</param>
        /// <returns></returns>
        public static DateTime DaysAgo( this int days )
        {
            var _timespan = new TimeSpan(  days, 0, 0, 0 );
            return DateTime.Now.Subtract( _timespan );
        }

        /// <summary>Dayses from now.</summary>
        /// <param name="days">The days.</param>
        /// <returns></returns>
        public static DateTime DaysFromNow( this int days )
        {
            var _timespan = new TimeSpan( days, 0, 0, 0 );
            return DateTime.Now.Add( _timespan );
        }

        /// <summary>Hourses the ago.</summary>
        /// <param name="hours">The hours.</param>
        /// <returns></returns>
        public static DateTime HoursAgo( this int hours )
        {
            var _timespan = new TimeSpan( hours, 0, 0 );
            return DateTime.Now.Subtract( _timespan );
        }

        /// <summary>Hourses from now.</summary>
        /// <param name="hours">The hours.</param>
        /// <returns></returns>
        public static DateTime HoursFromNow( this int hours )
        {
            var _timespan = new TimeSpan( hours, 0, 0 );
            return DateTime.Now.Add( _timespan );
        }

        /// <summary>Minuteses the ago.</summary>
        /// <param name="minutes">The minutes.</param>
        /// <returns></returns>
        public static DateTime MinutesAgo( this int minutes )
        {
            var _timespan = new TimeSpan( 0, minutes, 0 );
            return DateTime.Now.Subtract( _timespan );
        }

        /// <summary>Minuteses from now.</summary>
        /// <param name="minutes">The minutes.</param>
        /// <returns></returns>
        public static DateTime MinutesFromNow( this int minutes )
        {
            var _timespan = new TimeSpan( 0, minutes, 0 );
            return DateTime.Now.Add( _timespan );
        }

        /// <summary>Secondses the ago.</summary>
        /// <param name="seconds">The seconds.</param>
        /// <returns></returns>
        public static DateTime SecondsAgo( this int seconds )
        {
            var _timespan = new TimeSpan( 0, 0, seconds );
            return DateTime.Now.Subtract( _timespan );
        }

        /// <summary>Secondses from now.</summary>
        /// <param name="seconds">The seconds.</param>
        /// <returns></returns>
        public static DateTime SecondsFromNow( this int seconds )
        {
            var _timespan = new TimeSpan( 0, 0, seconds );
            return DateTime.Now.Add( _timespan );
        }

        /// <summary>
        /// Determines whether [is week day].
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>
        ///   <c>true</c> if [is week day] [the specified date time]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWeekDay( this DateTime dateTime )
        {
            return dateTime.DayOfWeek != DayOfWeek.Saturday 
                && dateTime.DayOfWeek != DayOfWeek.Sunday;
        }

        /// <summary>
        /// Determines whether [is week end].
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>
        ///   <c>true</c> if [is week end] [the specified date time]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWeekEnd( this DateTime dateTime )
        {
            return dateTime.DayOfWeek == DayOfWeek.Saturday 
                || dateTime.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>Counts the week days.</summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public static int CountWeekDays( this DateTime startDate, DateTime endDate )
        {
            var _timespan = endDate - startDate;
            var _days = 0;

            for( var i = 0; i < _timespan.Days; i++ )
            {
                var _time = startDate.AddDays( i );

                if( _time.IsWeekDay() )
                {
                    _days++;
                }
            }

            return _days;
        }

        /// <summary>Counts the week ends.</summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public static int CountWeekEnds( this DateTime startDate, DateTime endDate )
        {
            var _timespan = endDate - startDate;
            Console.WriteLine( _timespan.Days );
            var _days = 0;

            for( var i = 0; i < _timespan.Days; i++ )
            {
                var _time = startDate.AddDays( i );

                if( _time.IsWeekEnd() )
                {
                    _days++;
                }
            }

            return _days;
        }

        /// <summary>Differences the specified date two.</summary>
        /// <param name="dateOne">The date one.</param>
        /// <param name="dateTwo">The date two.</param>
        /// <returns></returns>
        public static TimeSpan Diff( this DateTime dateOne, DateTime dateTwo )
        {
            var _timespan = dateOne.Subtract( dateTwo );
            return _timespan;
        }

        /// <summary>Differences the days.</summary>
        /// <param name="dateOne">The date one.</param>
        /// <param name="dateTwo">The date two.</param>
        /// <returns></returns>
        public static double DiffDays( this string dateOne, string dateTwo )
        {
            return DateTime.TryParse( dateOne, out var dtone ) 
                && DateTime.TryParse( dateTwo, out var dttwo )
                    ? dtone.Diff( dttwo ).TotalDays
                    : 0;
        }

        /// <summary>Differences the days.</summary>
        /// <param name="dateOne">The date one.</param>
        /// <param name="dateTwo">The date two.</param>
        /// <returns></returns>
        public static double DiffDays( this DateTime dateOne, DateTime dateTwo )
        {
            return dateOne.Diff( dateTwo ).TotalDays;
        }

        /// <summary>Differences the hours.</summary>
        /// <param name="dateOne">The date one.</param>
        /// <param name="dateTwo">The date two.</param>
        /// <returns></returns>
        public static double DiffHours( this string dateOne, string dateTwo )
        {
            return DateTime.TryParse( dateOne, out var dtone ) && DateTime.TryParse( dateTwo, out var dttwo )
                ? dtone.Diff( dttwo ).TotalHours
                : 0;
        }

        /// <summary>Differences the hours.</summary>
        /// <param name="dateOne">The date one.</param>
        /// <param name="dateTwo">The date two.</param>
        /// <returns></returns>
        public static double DiffHours( this DateTime dateOne, DateTime dateTwo )
        {
            return dateOne.Diff( dateTwo ).TotalHours;
        }

        /// <summary>Differences the minutes.</summary>
        /// <param name="dateOne">The date one.</param>
        /// <param name="dateTwo">The date two.</param>
        /// <returns></returns>
        public static double DiffMinutes( this string dateOne, string dateTwo )
        {
            return DateTime.TryParse( dateOne, out var dtone ) 
                && DateTime.TryParse( dateTwo, out var dttwo )
                    ? dtone.Diff( dttwo ).TotalMinutes
                    : 0;
        }

        /// <summary>Differences the minutes.</summary>
        /// <param name="dateOne">The date one.</param>
        /// <param name="dateTwo">The date two.</param>
        /// <returns></returns>
        public static double DiffMinutes( this DateTime dateOne, DateTime dateTwo )
        {
            return dateOne.Diff( dateTwo ).TotalMinutes;
        }

        /// <summary>
        /// Determines whether [is federal holiday].
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        ///   <c>true</c> if [is federal holiday] [the specified date]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFederalHoliday( this DateTime date )
        {
            // to ease typing
            var _nthweekday = (int)Math.Ceiling( date.Day / 7.0d );
            var _dayName = date.DayOfWeek;
            var _isThursday = _dayName == DayOfWeek.Thursday;
            var _isFriday = _dayName == DayOfWeek.Friday;
            var _isMonday = _dayName == DayOfWeek.Monday;
            var _isWeekend = _dayName == DayOfWeek.Saturday || _dayName == DayOfWeek.Sunday;

            switch( date.Month )
            {
                // New Years Day (Jan 1, or preceding Friday/following Monday if weekend)
                case 12 when date.Day == 31 && _isFriday:
                case 1 when date.Day == 1 && !_isWeekend:
                case 1 when date.Day == 2 && _isMonday:

                // MLK day (3rd monday in January)
                case 1 when _isMonday && _nthweekday == 3:

                // President’s Day (3rd Monday in February)
                case 2 when _isMonday && _nthweekday == 3:

                // Memorial Day (Last Monday in May)
                case 5 when _isMonday && date.AddDays( 7 ).Month == 6:

                // Independence Day (July 4, or preceding Friday/following Monday if weekend)
                case 7 when date.Day == 3 && _isFriday:
                case 7 when date.Day == 4 && !_isWeekend:
                case 7 when date.Day == 5 && _isMonday:

                // Labor Day (1st Monday in September)
                case 9 when _isMonday && _nthweekday == 1:

                // Columbus Day (2nd Monday in October)
                case 10 when _isMonday && _nthweekday == 2:

                // Veteran’s Day (November 11, or preceding Friday/following Monday if weekend))
                case 11 when date.Day == 10 && _isFriday:
                case 11 when date.Day == 11 && !_isWeekend:
                case 11 when date.Day == 12 && _isMonday:

                // Thanksgiving Day (4th Thursday in November)
                case 11 when _isThursday && _nthweekday == 4:

                // Christmas Day (December 25, or preceding Friday/following Monday if weekend))
                case 12 when date.Day == 24 && _isFriday:
                case 12 when date.Day == 25 && !_isWeekend:
                case 12 when date.Day == 26 && _isMonday:
                    return true;

                default:
                    return false;
            }
        }
    }
}
