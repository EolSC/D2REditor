/*
 * Storage class for various DS1 tile data
 * Can store 1 to N layers of specified tile type
 */
public class DS1Container<T> 
{
    public T[,,] data;   // data array
    public uint layers;   // # of layers in data buffer
}