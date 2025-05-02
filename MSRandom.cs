namespace Extensions
{
  public class MSRandom
  {
    public const int RAND_MAX = 32767;
    private uint Seed = 1;

    public MSRandom() => this.Seed = 1U;

    public MSRandom(uint seed) => this.Seed = seed;

    public int Next() => (int) ((this.Seed = (uint) ((int) this.Seed * 214013 + 2531011)) >> 16) & (int) short.MaxValue;
  }
}
