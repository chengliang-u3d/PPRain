using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class PPRain : MonoBehaviour
{

    public static PPRain Instance { get; private set; }
    [Header("Setup")]
    public Shader rainShader;
    public Mesh rainMesh;

    [Header("Virtual Planes")]
    public Texture2D rainTexture = null;
    public Vector4 rainData0 = new Vector4(1f, 1f, 0f, 4f);

    [Range(0f, 2f)]
    public float rainColorScale = 0.5f;
    public Color rainColor0 = Color.gray;


    [Range(0f, 10f)]
    public float layerDistance0 = 2f;
    [Range(1f, 20f)]
    public float layerDistance1 = 6f;


    [MinValue(-1f)]
    public float forceLayerDistance0 = -1f;


    [Range(0.25f, 4f)]
    public float lightExponent = 1f;
    [Range(0.25f, 4f)]
    public float lightIntensity1 = 1f;
    [Range(0.25f, 4f)]
    public float lightIntensity2 = 1f;




    Material m_RainMaterial;

	void OnEnable()
    {
        m_RainMaterial = new Material(rainShader ?? Shader.Find("Hidden/PPRain"));
        m_RainMaterial.hideFlags = HideFlags.DontSave;



        Debug.Assert(Instance == null);
        Instance = this;
    }
	
	
	void OnDisable()
    {
        DestroyImmediate(m_RainMaterial);
        m_RainMaterial = null;



        Debug.Assert(Instance == this);
        Instance = null;
    }
	
	public void Render(Camera cam, RenderTargetIdentifier src, RenderTargetIdentifier dst, CommandBuffer deferredCmds)
    {
        deferredCmds.SetGlobalTexture("_MainTex", src);

        deferredCmds.SetGlobalTexture("_RainTexture", rainTexture);
        deferredCmds.SetGlobalVector("_UVData0", rainData0);

        deferredCmds.SetGlobalVector("_RainColor0", rainColor0 * rainColorScale);

        deferredCmds.SetGlobalVector("_LayerDistances0", new Vector4(layerDistance0, layerDistance1 - layerDistance0, 0, 0));


        deferredCmds.SetGlobalVector("_ForcedLayerDistances", new Vector4(forceLayerDistance0, -1, -1, -1));

        deferredCmds.SetGlobalFloat("_LightExponent", lightExponent);
        deferredCmds.SetGlobalFloat("_LightIntensity1", lightIntensity1);
        deferredCmds.SetGlobalFloat("_LightIntensity2", lightIntensity2);

        var xform = Matrix4x4.TRS(cam.transform.position, transform.rotation, new Vector3(1f, 1f, 1f) );

        deferredCmds.SetRenderTarget(dst);
        deferredCmds.DrawMesh(rainMesh, xform, m_RainMaterial);
    }
    



    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.blue;

        Gizmos.DrawLine(Vector3.up * 0.5f, Vector3.zero);

    

        //Gizmos.DrawMesh(rainMesh, transform.position);
    }
}
