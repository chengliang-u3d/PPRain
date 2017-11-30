using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
[PostProcess(typeof(PPRainFXRenderer), PostProcessEvent.BeforeStack, "Custom/PP Rain FX")]

public sealed class PPRainFX : PostProcessEffectSettings
{
    public override bool IsEnabledAndSupported(PostProcessRenderContext context)
    {
        return base.IsEnabledAndSupported(context) && PPRain.Instance;
    }


}

public sealed class PPRainFXRenderer : PostProcessEffectRenderer<PPRainFX>
{
    public override void Render(PostProcessRenderContext context)
    {
        PPRain.Instance.Render(context.camera, context.source, context.destination, context.command);
    }
}