public class Cell
{
    public bool Visited { get; set; }
    public bool[] Connections { get; } = new bool[4];
}