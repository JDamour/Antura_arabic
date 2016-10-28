﻿using System;

namespace EA4S
{
    public static class GenericUtilities
    {
        public static string ReverseText(string text)
        {
            var cArray = text.ToCharArray();
            var reverse = string.Empty;
            for (var i = cArray.Length - 1; i > -1; i--) {
                reverse += cArray[i];
            }
            return reverse;
        }

        #region DateTime
        private static DateTime TIME_START = new DateTime(1970, 1, 1, 0, 0, 0);
        public static int GetRelativeTimestampFromNow(int deltaDays)
        {
            var timeSpan = new TimeSpan(deltaDays, 0, 0, 0, 0);
            return GetTimestampForNow() + (int)timeSpan.TotalSeconds;
        }
        public static int GetTimestampForNow()
        {
            var timeSpan = (DateTime.UtcNow - TIME_START);
            return (int)timeSpan.TotalSeconds;
        }
        public static DateTime FromTimestamp(int timestamp)
        {
            var span = TimeSpan.FromSeconds(timestamp);
            return TIME_START + span;
        }
        public static TimeSpan GetTimeSpanBetween(int timestamp_from, int timestamp_to)
        {
            return FromTimestamp(timestamp_to) - FromTimestamp(timestamp_from);
        }

        #endregion
    }
}
