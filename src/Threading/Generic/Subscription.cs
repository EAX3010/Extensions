// Decompiled with JetBrains decompiler
// Type: Extensions.Threading.Generic.Subscription`1
// Assembly: Extensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 635FAC6D-CDE5-48E5-A8FB-F155E2899BE4
// Assembly location: C:\Users\x3010\Desktop\LordsUpdate\LordsSource\bin\Debug\Extensions.dll

using System.Reflection;

namespace Extensions.Threading.Generic
{
    internal class Subscription<T> : ISubscription
  {
    private TimerRule<T> Instruction;
    private T Param;

    public Subscription(TimerRule<T> instruction, T param)
    {
      this.Instruction = instruction;
      this.Priority = this.Instruction.Priority;
      this.Param = param;
    }

    internal override void Invoke()
    {
      if (this.Instruction == null)
        return;
      this.Instruction.Action(this.Param);
      if (this.Instruction == null)
        return;
      if (!this.Instruction.Repeat)
        this.Dispose();
      else
        this.Set(this.Instruction.Period);
    }

    internal override void CleanUp()
    {
      this.Instruction = (TimerRule<T>) null;
      this.Param = default (T);
    }

    internal override MethodInfo GetMethodInfo() => this.Instruction.Action.Method;

    internal override ThreadPriority GetPriority() => this.Priority;
  }
}
