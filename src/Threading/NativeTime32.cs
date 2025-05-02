// Decompiled with JetBrains decompiler
// Type: Extensions.Threading.NativeTime32
// Assembly: Extensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 635FAC6D-CDE5-48E5-A8FB-F155E2899BE4
// Assembly location: C:\Users\x3010\Desktop\LordsUpdate\LordsSource\bin\Debug\Extensions.dll

namespace Extensions.Threading
{
    public struct NativeTime32
  {
    public static readonly NativeTime32 NULL = new NativeTime32(0);
    private uint value;

    public static NativeTime32 Now => new NativeTime32(Time32.Now.Value);

    public uint Value => this.value;

    public NativeTime32(uint Value) => this.value = Value;

    public NativeTime32(int Value) => this.value = (uint) Value;

    public NativeTime32(long Value) => this.value = (uint) Value;

    public static bool operator ==(NativeTime32 t1, NativeTime32 t2) => (int) t1.value == (int) t2.value;

    public static bool operator !=(NativeTime32 t1, NativeTime32 t2) => (int) t1.value != (int) t2.value;

    public static bool operator >(NativeTime32 t1, NativeTime32 t2) => t1.value > t2.value;

    public static bool operator <(NativeTime32 t1, NativeTime32 t2) => t1.value < t2.value;

    public static bool operator >=(NativeTime32 t1, NativeTime32 t2) => t1.value >= t2.value;

    public static bool operator <=(NativeTime32 t1, NativeTime32 t2) => t1.value <= t2.value;

    public static NativeTime32 operator -(NativeTime32 t1, NativeTime32 t2) => new NativeTime32(t1.value - t2.value);

    public NativeTime32 AddMilliseconds(int Amount) => new NativeTime32((long) this.value + (long) Amount);

    public NativeTime32 AddMilliseconds(uint Amount) => new NativeTime32(this.value + Amount);

    public uint AllMilliseconds() => this.value;

    public NativeTime32 AddSeconds(int Amount) => this.AddMilliseconds(Amount * 1000);

    public NativeTime32 AddSeconds(uint Amount) => this.AddMilliseconds(Amount * 1000U);

    public uint AllSeconds() => this.AllMilliseconds() / 1000U;

    public NativeTime32 AddMinutes(int Amount) => this.AddSeconds(Amount * 60);

    public NativeTime32 AddMinutes(uint Amount) => this.AddSeconds(Amount * 60U);

    public uint AllMinutes() => this.AllSeconds() / 60U;

    public NativeTime32 AddHours(int Amount) => this.AddMinutes(Amount * 60);

    public NativeTime32 AddHours(uint Amount) => this.AddMinutes(Amount * 60U);

    public uint AllHours() => this.AllMinutes() / 60U;

    public NativeTime32 AddDays(int Amount) => this.AddHours(Amount * 24);

    public NativeTime32 AddDays(uint Amount) => this.AddHours(Amount * 24U);

    public uint AllDays() => this.AllHours() / 24U;

    public bool Next(uint due = 0, uint time = 0)
    {
      if (time == 0U)
        time = (uint) Environment.TickCount;
      return this.value + due <= time;
    }

    public void Set(uint due, uint time = 0)
    {
      if (time == 0U)
        time = (uint) Environment.TickCount;
      this.value = time + due;
    }

    public void SetSeconds(uint due, uint time = 0) => this.Set(due * 1000U, time);

    public override bool Equals(object obj) => obj is NativeTime32 nativeTime32 ? nativeTime32 == this : base.Equals(obj);

    public override string ToString() => this.value.ToString();

    public override int GetHashCode() => (int) this.value;
  }
}
