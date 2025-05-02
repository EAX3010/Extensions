// Decompiled with JetBrains decompiler
// Type: Extensions.Threading.Generic.LazyDelegate`1
// Assembly: Extensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 635FAC6D-CDE5-48E5-A8FB-F155E2899BE4
// Assembly location: C:\Users\x3010\Desktop\LordsUpdate\LordsSource\bin\Debug\Extensions.dll

namespace Extensions.Threading.Generic
{
    public class LazyDelegate<T> : TimerRule<T>
  {
    public LazyDelegate(Action<T> action, uint dueTime, ThreadPriority priority = ThreadPriority.Normal)
      : base(action, dueTime, priority)
    {
      this.Repeat = false;
    }
  }
}
