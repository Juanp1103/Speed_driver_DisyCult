using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CRTRendererFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public Material crtMaterial;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public Settings settings = new Settings();

    class CRTPass : ScriptableRenderPass
    {
        public Material material;
        private ScriptableRenderer rendererRef;
        private int tempTexId = Shader.PropertyToID("_CRTTempTex");

        public void SetRenderer(ScriptableRenderer renderer)
        {
            rendererRef = renderer;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null || rendererRef == null) return;

            RTHandle source = rendererRef.cameraColorTargetHandle;
            if (source == null) return;

            CommandBuffer cmd = CommandBufferPool.Get("CRT Effect");

            // Crear textura temporal
            RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.depthBufferBits = 0;
            desc.msaaSamples = 1;
            cmd.GetTemporaryRT(tempTexId, desc, FilterMode.Bilinear);

            // 1) Copiar source -> temp aplicando el material (efecto CRT)
            cmd.Blit(source.rt, tempTexId, material, 0);

            // 2) Copiar temp -> source (la imagen ya con efecto vuelve a la camara)
            cmd.Blit(tempTexId, source.rt);

            // Liberar la temporal
            cmd.ReleaseTemporaryRT(tempTexId);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    CRTPass pass;

    public override void Create()
    {
        pass = new CRTPass
        {
            material = settings.crtMaterial,
            renderPassEvent = settings.renderPassEvent
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.crtMaterial == null) return;
        pass.SetRenderer(renderer);
        renderer.EnqueuePass(pass);
    }
}