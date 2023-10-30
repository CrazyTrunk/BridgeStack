using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorManager : Singleton<ColorManager>
{
    private List<int> availableColorIndices;
    [SerializeField] public ColorDataSO colorDataSO;
    private void Awake()
    {
        InitializeColors();
    }
    public void InitializeColors()
    {
        availableColorIndices = Enumerable.Range(0, colorDataSO.ColorDatas.Count).ToList();
        ShuffleColors();
    }

    private void ShuffleColors()
    {
        availableColorIndices = availableColorIndices.OrderBy(x => Random.value).ToList();
    }

    public int GetNextColorIndex()
    {
        if (availableColorIndices.Count == 0)
        {
            return -1;
        }

        int colorIndex = availableColorIndices[0];
        availableColorIndices.RemoveAt(0);
        return colorIndex;
    }
}
