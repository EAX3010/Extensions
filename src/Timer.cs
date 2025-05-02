namespace Extensions
{
    public class Timer : IDisposable
    {
        private Action _action;
        private bool _isBeingHandled;
        private IDisposable _timerHandle;

        public Timer(Action action, uint period, uint dueTime = 0)
        {
            this._action = action;
            this._timerHandle = (IDisposable)new System.Threading.Timer(new TimerCallback(Timer.timerCallback), (object)this, dueTime, period);
        }

        ~Timer()
        {
            this._timerHandle.Dispose();
            this._action = (Action)null;
        }

        private static void timerCallback(object obj)
        {
            Timer timer = obj as Timer;
            if (timer._isBeingHandled)
                return;
            timer._isBeingHandled = true;
            try
            {
                timer._action();
            }
            catch
            {
                throw;
            }
            finally
            {
                timer._isBeingHandled = false;
            }
        }

        void IDisposable.Dispose()
        {
            this._timerHandle.Dispose();
            this._action = (Action)null;
        }
    }
    public class Timer<T> : IDisposable
    {
        private Action<T> _action;
        private bool _isBeingHandled;
        private IDisposable _timerHandle;

        public Timer(Action<T> action, T param, uint period, uint dueTime = 0)
        {
            if (action == null)
                Console.WriteLine("Null actions");
            this._action = action;
            this._timerHandle = (IDisposable)new System.Threading.Timer(new TimerCallback(Timer<T>.timerCallback), (object)new Tuple<T, Timer<T>>(param, this), dueTime, period);
        }

        private static void timerCallback(object obj)
        {
            try
            {
                Tuple<T, Timer<T>> tuple = obj as Tuple<T, Timer<T>>;
                T obj1 = tuple.Item1;
                Timer<T> timer = tuple.Item2;
                if (timer._isBeingHandled)
                    return;
                timer._isBeingHandled = true;
                try
                {
                    if (timer == null || timer._action == null || (object)obj1 == null)
                        return;
                    timer._action(obj1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }
                finally
                {
                    timer._isBeingHandled = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        void IDisposable.Dispose()
        {
            this._timerHandle.Dispose();
            this._action = null;
        }
    }
}
