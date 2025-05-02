namespace Extensions
{
    public class SafeDictionary<T1, T2> : Dictionary<T1, T2>
    {
        public object SyncRoot;
        public bool Update;
        public T2[] MyArray = new T2[0];

        public new T2 this[T1 key]
        {
            get => this.ContainsKey(key) ? base[key] : default(T2);
            set => base[key] = value;
        }

        public SafeDictionary()
        {
            this.MyArray = new T2[0];
            this.SyncRoot = new object();
        }

        public SafeDictionary(int cap)
          : base(cap)
        {
            this.SyncRoot = new object();
        }

        public new void Add(T1 key, T2 value)
        {
            try
            {
                Monitor.Enter(this.SyncRoot);
                base[key] = value;
                this.Update = true;
            }
            finally
            {
                Monitor.Exit(this.SyncRoot);
            }
        }

        public void Remove(T1 key)
        {
            try
            {
                Monitor.Enter(this.SyncRoot);
                base.Remove(key);
                this.Update = true;
            }
            finally
            {
                Monitor.Exit(this.SyncRoot);
            }
        }

        public T2[] GetValues()
        {
            if (!this.Update)
                return this.MyArray;
            try
            {
                Monitor.Enter(this.SyncRoot);
                this.Update = false;
                this.MyArray = this.Values.ToArray<T2>();
            }
            finally
            {
                Monitor.Exit(this.SyncRoot);
            }
            return this.MyArray;
        }

        public new void Clear()
        {
            lock (this.SyncRoot)
                base.Clear();
            this.Update = true;
        }
    }
}
