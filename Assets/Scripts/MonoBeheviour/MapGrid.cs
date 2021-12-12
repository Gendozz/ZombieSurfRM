using System.Collections;

public struct MapGrid : IEnumerable
{
    private int width;

    private int length;

    public Cell[,] cells;
    
    public MapGrid (int width, int length)
    {
        cells = new Cell[width, length];
        this.width = width;
        this.length = length;
    }

    public IEnumerator GetEnumerator()
    {
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                yield return cells[j, i];
            }
        }
    }    
    
    //public IEnumerator GetEnumerator()
    //{
    //    for (int i = 0; i < width; i++)
    //    {
    //        for (int j = 0; j < length; j++)
    //        {
    //            yield return cells[i, j];
    //        }
    //    }
    //}
}
