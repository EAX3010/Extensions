// Decompiled with JetBrains decompiler
// Type: Extensions.Threading.StaticPool
// Assembly: Extensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 635FAC6D-CDE5-48E5-A8FB-F155E2899BE4
// Assembly location: C:\Users\x3010\Desktop\LordsUpdate\LordsSource\bin\Debug\Extensions.dll

using Extensions.Threading.Generic;
using System.Security;

namespace Extensions.Threading
{
    public class StaticPool : IDisposable
  {
    public static int SleepTime = 1;
    private static bool IsHandlerCreated = false;
    internal ThreadPriority basePriority = ThreadPriority.Normal;
    internal object qSyncRoot;
    internal object dSyncRoot;
    internal Dictionary<int, ISubscription> subscribers;
    internal Queue<ISubscription> queue;
    internal List<Thread> pool;
    protected internal Thread propagationThread;
    internal volatile bool doWork;
    internal volatile bool disposed;
    internal int threads;
    internal int idleThreads;
    internal int inUseThreads;
    internal int minimumThreadCount;
    internal int maximumThreadCount;

    public int Threads => this.threads;

    public int IdleThreads => this.idleThreads;

    public int InUseThreads => this.inUseThreads;

    public int Treshold => this.queue.Count;

    public StaticPool(int maximumPoolSize = 32, ThreadPriority basePriority = ThreadPriority.Normal)
    {
      if (!StaticPool.IsHandlerCreated)
      {
        StaticPool.IsHandlerCreated = true;
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.CurrentDomain_UnhandledException);
      }
      this.disposed = false;
      this.dSyncRoot = new object();
      this.qSyncRoot = new object();
      this.subscribers = new Dictionary<int, ISubscription>();
      this.queue = new Queue<ISubscription>();
      this.pool = new List<Thread>();
      this.minimumThreadCount = maximumPoolSize;
      this.maximumThreadCount = maximumPoolSize;
      this.basePriority = basePriority;
    }

    ~StaticPool() => this.cleanUp(false);

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
    }

    void IDisposable.Dispose() => this.cleanUp(true);

    [SecuritySafeCritical]
    internal void enrollWorker()
    {
      if (this.disposed)
        return;
      Interlocked.Increment(ref this.threads);
      Interlocked.Increment(ref this.idleThreads);
      Thread thread = new Thread(new ThreadStart(this.work), 1048576);
      this.pool.Add(thread);
      thread.Priority = this.basePriority;
      thread.Start();
    }

    internal void cleanUp(bool forcefully)
    {
      if (this.disposed)
        return;
      this.disposed = true;
      this.doWork = false;
      if (forcefully)
      {
        foreach (Thread thread in this.pool)
          thread.Abort();
      }
      this.subscribers.Clear();
      this.subscribers = (Dictionary<int, ISubscription>) null;
      this.queue = (Queue<ISubscription>) null;
      this.pool = (List<Thread>) null;
    }

    internal void work()
    {
      Thread currentThread = Thread.CurrentThread;
      while (currentThread == null)
        currentThread = Thread.CurrentThread;
      while (this.doWork)
      {
        Thread.Sleep(StaticPool.SleepTime);
        ISubscription sub;
        if (this.tryDequeue(out sub))
        {
          if (sub.Viable)
          {
            Interlocked.Decrement(ref this.idleThreads);
            Interlocked.Increment(ref this.inUseThreads);
            currentThread.Priority = sub.GetPriority();
            try
            {
              sub.Invoke();
            }
            catch (Exception ex)
            {
              Console.WriteLine((object) ex);
            }
            finally
            {
              sub.Enqueued = false;
            }
            currentThread.Priority = this.basePriority;
            Interlocked.Decrement(ref this.inUseThreads);
            Interlocked.Increment(ref this.idleThreads);
          }
          else
            this.removeSubscriber(sub.GetHashCode());
        }
        sub = (ISubscription) null;
      }
      Interlocked.Decrement(ref this.idleThreads);
    }

    internal bool tryDequeue(out ISubscription sub)
    {
      sub = (ISubscription) null;
      lock (this.qSyncRoot)
      {
        if (this.queue.Count != 0)
        {
          ISubscription isubscription = this.queue.Dequeue();
          sub = isubscription;
        }
      }
      return sub != null;
    }

    internal void removeSubscriber(int hash)
    {
      lock (this.dSyncRoot)
        this.subscribers.Remove(hash);
    }

    public IDisposable Subscribe(TimerRule instruction)
    {
      ISubscription isubscription = (ISubscription) null;
      lock (this.dSyncRoot)
      {
        isubscription = (ISubscription) new Subscription(instruction);
        if (instruction is LazyDelegate)
          isubscription.Set(instruction.Period);
        this.subscribers[isubscription.GetHashCode()] = isubscription;
      }
      return (IDisposable) isubscription;
    }

    public IDisposable Subscribe<T>(TimerRule<T> instruction, T param)
    {
      ISubscription isubscription = (ISubscription) null;
      lock (this.dSyncRoot)
      {
        isubscription = (ISubscription) new Subscription<T>(instruction, param);
        if (instruction is LazyDelegate<T>)
          isubscription.Set(instruction.Period);
        this.subscribers[isubscription.GetHashCode()] = isubscription;
      }
      return (IDisposable) isubscription;
    }

    public StaticPool Run()
    {
      this.doWork = true;
      for (int index = 0; index < this.minimumThreadCount; ++index)
        this.enrollWorker();
      this.propagationThread = new Thread(new ThreadStart(this.propagate));
      this.propagationThread.Start();
      return this;
    }

    private void propagate()
    {
      while (this.doWork)
      {
        Queue<ISubscription> isubscriptionQueue = new Queue<ISubscription>();
        Queue<int> intQueue = new Queue<int>();
        bool lockTaken1 = false;
        object obj = (object) null;
        try
        {
          Monitor.Enter(obj = this.dSyncRoot, ref lockTaken1);
          foreach (ISubscription isubscription in this.subscribers.Values)
          {
            if (isubscription.Viable)
            {
              if (!isubscription.Enqueued && isubscription.Next)
              {
                isubscription.Enqueued = true;
                isubscriptionQueue.Enqueue(isubscription);
              }
            }
            else
              intQueue.Enqueue(isubscription.GetHashCode());
          }
          while (intQueue.Count != 0)
            this.subscribers.Remove(intQueue.Dequeue());
        }
        finally
        {
          if (lockTaken1)
            Monitor.Exit(obj);
        }
        if (isubscriptionQueue.Count != 0)
        {
          bool lockTaken2 = false;
          try
          {
            Monitor.Enter(obj = this.qSyncRoot, ref lockTaken2);
            while (isubscriptionQueue.Count != 0)
              this.queue.Enqueue(isubscriptionQueue.Dequeue());
          }
          finally
          {
            if (lockTaken2)
              Monitor.Exit(obj);
          }
        }
        Thread.Sleep(StaticPool.SleepTime);
      }
    }

    public void Clear()
    {
      lock (this.qSyncRoot)
        this.queue.Clear();
    }

    public override string ToString() => string.Format("{0} waiting exec, {1} subscriptions, {2} threads: {3} in use, {4} idle", (object) this.queue.Count, (object) this.subscribers.Count, (object) this.threads, (object) this.inUseThreads, (object) this.idleThreads);
  }
}
