namespace Extensions
{
  public class Counter
  {
    private uint val;

    public uint Next => ++this.val;

    public uint Count => this.val;

    public Counter(uint start) => this.val = start;

    public void Set(uint start) => this.val = start;
  }
}
