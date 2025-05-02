namespace Extensions
{
    public class MyList<T>
  {
    private T[] objects = new T[0];
    public List<T> m_List = new List<T>();
    public object SyncRoot = new object();
    public bool Update;

    public int Count => this.GetValues().Length;

    public T this[int key]
    {
      get
      {
        try
        {
          return this.GetValues().Length <= key ? default (T) : this.GetValues()[key];
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.ToString());
          Console.WriteLine(key.ToString() + " " + (object) this.objects.Length + " " + (object) this.GetValues().Length);
        }
        return default (T);
      }
    }

    public void Add(T obj)
    {
      lock (this.SyncRoot)
      {
        if (!this.m_List.Contains(obj))
          this.m_List.Add(obj);
        this.Update = true;
      }
    }

    public T[] GetValues()
    {
      if (this.Update)
      {
        lock (this.SyncRoot)
        {
          this.objects = this.m_List.ToArray();
          this.Update = false;
        }
      }
      return this.objects;
    }

    public void Remove(T obj)
    {
      lock (this.SyncRoot)
      {
        this.m_List.Remove(obj);
        this.Update = true;
      }
    }

    public void Clear()
    {
      lock (this.SyncRoot)
      {
        this.m_List.Clear();
        this.Update = true;
      }
    }
  }
}
