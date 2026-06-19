using System;
using static System.TimeSpan;

namespace UniExtension
{
    public class DateTimeUnix
    {
        public static readonly long BASETICK = DateTime.UnixEpoch.Ticks;

        public static DateTimeUnix UtcNow => new DateTimeUnix(DateTime.UtcNow);
        public static DateTimeUnix Now => new DateTimeUnix(DateTime.Now);

        public readonly long Ticks;

        public DateTimeUnix(long ticks)
        {
            Ticks = ticks;
        }
        public DateTimeUnix(DateTime utc)
        {
            Ticks = Math.Max(0L, (utc.Ticks - BASETICK) / TicksPerMillisecond);
        }

        public long ToDateTimeTicks() => Ticks * TicksPerMillisecond + BASETICK;
        public DateTime ToDateTime() => new DateTime(ToDateTimeTicks(), DateTimeKind.Local);
        public DateTime ToUtcDateTime() => new DateTime(ToDateTimeTicks(), DateTimeKind.Utc);

        //public static implicit operator DateTime(UnixDateTime v) => v.ToDateTime();
        //public static implicit operator UnixDateTime(DateTime v) => new UnixDateTime(v);
        public static TimeSpan operator +(DateTimeUnix a, DateTimeUnix b) => new TimeSpan((a.Ticks + b.Ticks) * TicksPerMillisecond);
        public static TimeSpan operator -(DateTimeUnix a, DateTimeUnix b) => new TimeSpan((a.Ticks - b.Ticks) * TicksPerMillisecond);
    }
}
