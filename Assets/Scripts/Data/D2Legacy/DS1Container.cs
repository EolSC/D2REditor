/*
 * Storage class for various DS1 tile data
 * Can store 1 to N layers of specified tile type
 */
public class DS1Container<T> where T: new()
{
    public T[,,] data;   // data array
    public uint layers;   // # of layers in data buffer

    public void Resize(int oldX, int oldY, int x, int y)
    {
        T[,,] newData = new T[layers, y, x];
        for (int n = 0; n < layers; n++)
        {
            for (int i = 0; i < y; ++i)
            {
                for (int j = 0; j < x; ++j)
                {
                    if (j < oldX && i < oldY)
                    {
                        newData[n, i, j] = data[n, i, j];
                    }
                    else
                    {
                        newData[n, i, j] = new T();
                    }
                }
            }
        }

        data = newData;

    }
}