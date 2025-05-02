// Decompiled with JetBrains decompiler
// Type: Extensions.Threading.Subscription
// Assembly: Extensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 635FAC6D-CDE5-48E5-A8FB-F155E2899BE4
// Assembly location: C:\Users\x3010\Desktop\LordsUpdate\LordsSource\bin\Debug\Extensions.dll

using System.Reflection;

namespace Extensions.Threading
{
    internal class Subscription : ISubscription
  {
    private TimerRule Instruction;

    public Subscription(TimerRule instruction)
    {
      this.Instruction = instruction;
      this.Priority = instruction.Priority;
    }

    internal override void Invoke()
    {
      if (this.Instruction == null)
        return;
      this.Instruction.Action();
      if (this.Instruction == null)
        return;
      if (!this.Instruction.Repeat)
        this.Dispose();
      else
        this.Set(this.Instruction.Period);
    }

    internal override void CleanUp() => this.Instruction = (TimerRule) null;

    internal override MethodInfo GetMethodInfo() => this.Instruction.Action.Method;

    internal override ThreadPriority GetPriority() => this.Priority;
  }
}
