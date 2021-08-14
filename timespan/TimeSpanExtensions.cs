// <copyright file = "TimeSpanExtensions.cs" company = "Terry D. Eppler">
// Copyright (c) Terry D. Eppler. All rights reserved.
// </copyright>

namespace BudgetExecution
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;

    /// <summary>
    /// Defines the <see cref = "TimeSpanExtensions"/> .
    /// </summary>
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "IntroduceOptionalParameters.Global" ) ]
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Defines the AvgDaysInAYear.
        /// </summary>
        public const double AvgDaysInAYear = 365.2425d;

        /// <summary>
        /// Defines the AvgDaysInAMonth.
        /// </summary>
        public const double AvgDaysInAMonth = 30.436875d;

        /// <summary>
        /// The GetYears.
        /// </summary>
        /// <param name = "timespan" >
        /// The timespan <see cref = "System.TimeSpan"/> .
        /// </param>
        /// <returns>
        /// The <see cref = "int"/> .
        /// </returns>
        public static int GetYears( this TimeSpan timespan )
        {
            return (int)( timespan.TotalDays / AvgDaysInAYear );
        }

        /// <summary>
        /// The GetTotalYears.
        /// </summary>
        /// <param name = "timespan" >
        /// The timespan <see cref = "System.TimeSpan"/> .
        /// </param>
        /// <returns>
        /// The <see cref = "double"/> .
        /// </returns>
        public static double GetTotalYears( this TimeSpan timespan )
        {
            return timespan.TotalDays / AvgDaysInAYear;
        }

        /// <summary>
        /// The GetMonths.
        /// </summary>
        /// <param name = "timespan" >
        /// The timespan <see cref = "System.TimeSpan"/> .
        /// </param>
        /// <returns>
        /// The <see cref = "int"/> .
        /// </returns>
        public static int GetMonths( this TimeSpan timespan )
        {
            return (int)( timespan.TotalDays % AvgDaysInAYear / AvgDaysInAMonth );
        }

        /// <summary>
        /// The GetTotalMonths.
        /// </summary>
        /// <param name = "timespan" >
        /// The timespan <see cref = "System.TimeSpan"/> .
        /// </param>
        /// <returns>
        /// The <see cref = "double"/> .
        /// </returns>
        public static double GetTotalMonths( this TimeSpan timespan )
        {
            return timespan.TotalDays / AvgDaysInAMonth;
        }

        /// <summary>
        /// The GetWeeks.
        /// </summary>
        /// <param name = "timespan" >
        /// The timespan <see cref = "System.TimeSpan"/> .
        /// </param>
        /// <returns>
        /// The <see cref = "int"/> .
        /// </returns>
        public static int GetWeeks( this TimeSpan timespan )
        {
            return (int)( timespan.TotalDays % AvgDaysInAYear % AvgDaysInAMonth / 7d );
        }

        /// <summary>
        /// The GetTotalWeeks.
        /// </summary>
        /// <param name = "timespan" >
        /// The timespan <see cref = "System.TimeSpan"/> .
        /// </param>
        /// <returns>
        /// The <see cref = "double"/> .
        /// </returns>
        public static double GetTotalWeeks( this TimeSpan timespan )
        {
            return timespan.TotalDays / 7d;
        }

        /// <summary>
        /// The GetDays.
        /// </summary>
        /// <param name = "timespan" >
        /// The timespan <see cref = "System.TimeSpan"/> .
        /// </param>
        /// <returns>
        /// The <see cref = "int"/> .
        /// </returns>
        public static int GetDays( this TimeSpan timespan )
        {
            return (int)( timespan.TotalDays % 7d );
        }

        /// <summary>
        /// The GetMicroseconds.
        /// </summary>
        /// <param name = "span" >
        /// The span <see cref = "System.TimeSpan"/> .
        /// </param>
        /// <returns>
        /// The <see cref = "double"/> .
        /// </returns>
        public static double GetMicroseconds( this TimeSpan span )
        {
            return span.Ticks / 10d;
        }

        /// <summary>
        /// The GetNanoseconds.
        /// </summary>
        /// <param name = "span" >
        /// The span <see cref = "System.TimeSpan"/> .
        /// </param>
        /// <returns>
        /// The <see cref = "double"/> .
        /// </returns>
        public static double GetNanoseconds( this TimeSpan span )
        {
            return span.Ticks / 100d;
        }

        /// <summary>
        /// The Round.
        /// </summary>
        /// <param name = "time" >
        /// The time <see cref = "System.TimeSpan"/> .
        /// </param>
        /// <param name = "roundinginterval" >
        /// The roundingInterval <see cref = "System.TimeSpan"/> .
        /// </param>
        /// <param name = "roundingtype" >
        /// The roundingType <see cref = "MidpointRounding"/> .
        /// </param>
        /// <returns>
        /// The <see cref = "System.TimeSpan"/> .
        /// </returns>
        public static TimeSpan Round( this TimeSpan time, TimeSpan roundinginterval,
            MidpointRounding roundingtype = MidpointRounding.ToEven )
        {
            return new TimeSpan( Convert.ToInt64( Math.Round( time.Ticks / (double)roundinginterval.Ticks,
                    roundingtype ) )
                * roundinginterval.Ticks );
        }
    }
}
