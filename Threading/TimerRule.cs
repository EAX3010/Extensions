// Decompiled with JetBrains decompiler
// Type: Extensions.Threading.TimerRule
// Assembly: Extensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 635FAC6D-CDE5-48E5-A8FB-F155E2899BE4
// Assembly location: C:\Users\x3010\Desktop\LordsUpdate\LordsSource\bin\Debug\Extensions.dll

namespace Extensions.Threading
{
    public class TimerRule
  {
    internal Action Action;
    internal uint Period;
    internal bool Repeat;
    internal ThreadPriority Priority;

    public TimerRule(Action action, uint period, ThreadPriority priority = ThreadPriority.Normal)
    {
      this.Action = action;
      this.Period = period;
      this.Repeat = true;
      this.Priority = priority;
    }

    ~TimerRule() => this.Action = (Action) null;
  }
}
