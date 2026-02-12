using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FlaskLiquid : MonoBehaviour
{
    private Color color;

    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();

        rend.material.color = new Color(0, 0, 0, 0);
    }

    public void setColor(Color color)
    {
        if (!rend)
        {
            rend = GetComponent<Renderer>();
        }

        this.color = color;
        rend.material.color = color;

        if(color == new Color(0, 0, 0, 0))
        {
            SetTransparent(rend.material);
            return;
        }


        SetOpaque(rend.material);
    }

    public Color getColor()
    {
        return color;
    }

    void SetTransparent(Material mat)
    {
        mat.SetFloat("_Surface", 1);
        mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        mat.SetOverrideTag("RenderType", "Transparent");

        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);

        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }

    void SetOpaque(Material mat)
    {
        mat.SetFloat("_Surface", 0);
        mat.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
        mat.SetOverrideTag("RenderType", "Opaque");

        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mat.SetInt("_ZWrite", 1);

        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
    }




}
