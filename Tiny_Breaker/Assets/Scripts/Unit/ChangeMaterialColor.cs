using UnityEngine;

[System.Serializable]
public class LevelMaterial
{
    public int level;
    public Material material;
}

public class ChangeMaterialColor : MonoBehaviour
{
    [SerializeField]
    LevelMaterial[] data = new LevelMaterial[5];

    [SerializeField, TooltipAttribute("変更するマテリアルの番号")]
    int changeMatNum = 0;

    [SerializeField]
    Unit unit = null;

    Color outlinecolor;

    void Start()
    {
        if (unit == null)
            return;

        SkinnedMeshRenderer mesh = GetComponent<SkinnedMeshRenderer>();

        for (int i = 0; i < data.Length; i++)
            if (unit.level >= data[i].level)
            {
                Material[] mats = mesh.materials;
                foreach (Material e in mesh.materials)
                    if (e.HasProperty("_OutlineColor"))
                        outlinecolor = e.GetColor("_OutlineColor");

                mats[changeMatNum] = data[i].material;
                mesh.materials = mats;

                foreach (Material e in mesh.materials)
                    if (e.HasProperty("_OutlineColor"))
                        e.SetColor("_OutlineColor", outlinecolor);
            }
    }
}