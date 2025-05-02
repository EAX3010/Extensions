using System.Runtime.InteropServices;

namespace Extensions
{
    public static class BaseFunc
  {
    [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern long srand(ulong seed);

    [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern long rand();

    public static int RandGet(int nMax, bool bRealRand)
    {
      if (nMax <= 0)
        nMax = 1;
      if (bRealRand)
        BaseFunc.srand((ulong) Time32.Now.Value);
      return (int) (BaseFunc.rand() % (long) nMax);
    }

    public static double RandomRateGet(double dRange)
    {
      double num1 = 3.1415926;
      int num2 = BaseFunc.RandGet(999, false) + 1;
      double d = Math.Sin((double) num2 * num1 / 1000.0);
      return num2 >= 90 ? 1.0 + dRange - Math.Sqrt(Math.Sqrt(d)) * dRange : 1.0 - dRange + Math.Sqrt(Math.Sqrt(d)) * dRange;
    }
  }
}
