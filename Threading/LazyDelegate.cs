// Decompiled with JetBrains decompiler
// Type: Extensions.Threading.LazyDelegate
// Assembly: Extensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 635FAC6D-CDE5-48E5-A8FB-F155E2899BE4
// Assembly location: C:\Users\x3010\Desktop\LordsUpdate\LordsSource\bin\Debug\Extensions.dll

namespace Extensions.Threading
{
    public class LazyDelegate : TimerRule
  {
    public LazyDelegate(Action action, uint dueTime, ThreadPriority priority = ThreadPriority.Normal)
      : base(action, dueTime, priority)
    {
      this.Repeat = false;
    }
  }
}
