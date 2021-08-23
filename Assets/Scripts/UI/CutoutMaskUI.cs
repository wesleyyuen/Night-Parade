using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

/*
    From Code Monkey
    https://www.youtube.com/watch?v=XJJl19N2KFM&t=3s&ab_channel=CodeMonkey
*/
public class CutoutMaskUI : Image
{
    public override Material materialForRendering
    {
        get
        {
            Material material = new Material(base.materialForRendering);
            material.SetInt("_StencilComp", (int) CompareFunction.NotEqual);
            return material;
        }
    }
}
