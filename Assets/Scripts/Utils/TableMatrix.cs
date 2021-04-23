using System;
using UnityEngine;

[Serializable]
public class TableMatrix
{
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    private int[] data;

    public int Width => width;
    public int Height => height;

    public TableMatrix(int width, int height)
    {
        this.width = width;
        this.height = height;
        data = new int[width * height];
    }

    public int this[int x, int y] { get => Get(x, y); set => Set(x, y, value); }

    private int Get(int x, int y) => data[y * width + x];
    private void Set(int x, int y, int value) => data[y * width + x] = value;
}