using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Extensions
{
    public struct Time32
    {
        public static bool isCreate = false;
        private const string nowDebug = "The last value of Time32.Now was greater than the current value generated during this call. This is likely due to a reset in the 49.71 days period. See http://msdn.microsoft.com/en-us/library/dd757629(VS.85).aspx for more information.";
        public static Stopwatch Clock;
        private long value;
        public static long ElapsedMilliseconds => Time32.Clock.ElapsedMilliseconds;

        public static long GetClock
        {
            get
            {
                return Time32.Clock.ElapsedMilliseconds;
            }
        }

        public static void Create()
        {
            Time32.isCreate = true;
            Time32.Clock = new Stopwatch();
            Time32.Clock.Start();
        }

        public long Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        public static Time32 Now
        {
            get
            {
                if (!Time32.isCreate)
                    Time32.Create();
                return new Time32(Time32.GetClock);
            }
        }

        public Time32(int Value)
        {
            this.value = (uint)Value;
        }

        public Time32(uint Value)
        {
            this.value = Value;
        }

        public Time32(long Value)
        {
            this.value = (uint)Value;
        }

        public Time32 AddMilliseconds(int Amount) => new Time32((long)this.value + (long)Amount);

        public int AllMilliseconds
        {
            get
            {
                return this.GetHashCode();
            }
        }

        public Time32 AddSeconds(int Amount) => this.AddMilliseconds(Amount * 1000);

        public int AllSeconds
        {
            get
            {
                return this.AllMilliseconds / 1000;
            }
        }

        public Time32 AddMinutes(int Amount) => this.AddSeconds(Amount * 60);

        public int AllMinutes
        {
            get
            {
                return this.AllSeconds / 60;
            }
        }

        public Time32 AddHours(int Amount) => this.AddMinutes(Amount * 60);

        public int AllHours
        {
            get
            {
                return this.AllMinutes / 60;
            }
        }

        public Time32 AddDays(int Amount) => this.AddHours(Amount * 24);

        public int AllDays
        {
            get
            {
                return this.AllHours / 24;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Time32)
                return (Time32)obj == this;
            return base.Equals(obj);
        }

        public override string ToString() => this.value.ToString();

        public override int GetHashCode() => (int)this.value;

        public static bool operator ==(Time32 t1, Time32 t2)
        {
            return (int)t1.value == (int)t2.value;
        }

        public static bool operator !=(Time32 t1, Time32 t2)
        {
            return (int)t1.value != (int)t2.value;
        }

        public static bool operator >(Time32 t1, Time32 t2)
        {
            return t1.value > t2.value;
        }

        public static bool operator <(Time32 t1, Time32 t2)
        {
            return t1.value < t2.value;
        }

        public static bool operator >=(Time32 t1, Time32 t2)
        {
            return t1.value >= t2.value;
        }

        public static bool operator <=(Time32 t1, Time32 t2)
        {
            return t1.value <= t2.value;
        }

        public static Time32 operator -(Time32 t1, Time32 t2)
        {
            return new Time32(t1.value - t2.value);
        }

        public bool Next(int due = 0, int time = 0)
        {
            if (time == 0)
                time = (int)Time32.timeGetTime().Value;
            return (long)this.Value + (long)due <= (long)time;
        }

        [DllImport("winmm.dll")]
        public static extern Time32 timeGetTime();

        public Time32 Add(int addedValue)
        {
            return new(this.value + addedValue);
        }
    }
}