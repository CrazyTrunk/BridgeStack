using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "_Game/ScriptableObjects/ColorDataSO", order = 0)]

public class ColorDataSO : ScriptableObject
{
    public List<ColorData> ColorDatas;
}
