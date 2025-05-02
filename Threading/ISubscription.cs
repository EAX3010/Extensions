// Decompiled with JetBrains decompiler
// Type: Extensions.Threading.ISubscription
// Assembly: Extensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 635FAC6D-CDE5-48E5-A8FB-F155E2899BE4
// Assembly location: C:\Users\x3010\Desktop\LordsUpdate\LordsSource\bin\Debug\Extensions.dll

using System.Reflection;

namespace Extensions.Threading
{
    internal abstract class ISubscription : IDisposable
  {
    internal static volatile int counter = int.MinValue;
    internal bool Viable;
    internal bool Enqueued;
    internal NativeTime32 NextInvokation;
    internal ThreadPriority Priority;
    protected int hashCode;

    internal bool Next => NativeTime32.Now > this.NextInvokation;

    public ISubscription()
    {
      ++ISubscription.counter;
      this.hashCode = ISubscription.counter;
      this.Viable = true;
      this.Enqueued = false;
      this.Set(0U);
    }

    ~ISubscription() => this.Dispose();

    internal abstract void Invoke();

    internal void Set(uint dueTime) => this.NextInvokation = NativeTime32.Now.AddMilliseconds(dueTime);

    public void Dispose()
    {
      this.Viable = false;
      this.CleanUp();
    }

    internal abstract void CleanUp();

    internal abstract MethodInfo GetMethodInfo();

    internal abstract ThreadPriority GetPriority();

    public override int GetHashCode() => this.hashCode;
  }
}
