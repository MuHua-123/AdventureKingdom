#if KAMGAM_RENDER_PIPELINE_URP && !KAMGAM_RENDER_PIPELINE_HDRP
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Kamgam.UIToolkitBlurredBackground
{
    public class BlurredBackgroundPassURP : ScriptableRenderPass
    {
        public ScriptableRenderer ScriptableRenderer;
        public BlurRendererURP BlurRenderer;
        public bool Active = true;

        ProfilingSampler _profilingSampler = new ProfilingSampler("UIToolkit Blurred Background Pass");

#if KAMGAM_RENDER_PIPELINE_URP_13
        RTHandle _cameraColorTarget;

        public void SetTarget(RTHandle colorHandle)
        {
            _cameraColorTarget = colorHandle;
        }
#endif

        public BlurredBackgroundPassURP(BlurRendererURP blurRenderer)
        {
            BlurRenderer = blurRenderer;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            ConfigureInput(ScriptableRenderPassInput.Color);

            base.OnCameraSetup(cmd, ref renderingData);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!Active)
                return;

#if !KAMGAM_RENDER_PIPELINE_URP_13
            var source = renderingData.cameraData.renderer.cameraColorTarget;
            //var source = ScriptableRenderer.cameraColorTarget;
#else
            var source = renderingData.cameraData.renderer.cameraColorTargetHandle;

            // Check if source is null, if yes then try to fetch it from the set target. Otherwise abort.
            if (renderingData.cameraData.cameraType != CameraType.Game || source == null)
            {
                source = _cameraColorTarget;
                if (source == null)
                {
#if UNITY_EDITOR
                    // TODO: Investigate: This is happening in URP 14 though it has no effect (everything works).
                    // Logger.LogWarning("Camera color target source is null. Will skip blur rendering. Please investigate this issue.");
#endif
                    return;
                }
            }
#endif

            CommandBuffer cmd = CommandBufferPool.Get(name: "UIToolkit Blurred Background Pass");
            cmd.Clear();

            using (new ProfilingScope(cmd, _profilingSampler))
            {
                // Notice: Do not use cmd.Blit() in SPRs, see:
                // https://forum.unity.com/threads/how-to-blit-in-urp-documentation-unity-blog-post-on-every-blit-function.1211508/#post-7735527
                // Blit Implementation can be found here:
                // https://github.com/Unity-Technologies/Graphics/blob/b57fcac51bb88e1e589b01e32fd610c991f16de9/Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blitter.cs#L221

                //Debug.Log(BlurRenderer.RenderTargetHandleA + " > " + BlurRenderer.RenderTargetBlurredA);
                //Debug.Log(BlurRenderer.RenderTargetHandleB + " > " + BlurRenderer.RenderTargetBlurredB);

                BlurRenderer.CreateRenderTextureTargetBlurredAIfNeeded();
                BlurRenderer.CreateRenderTextureTargetBlurredBIfNeeded();

                // First blur pass takes image from source
                // TODO: Ugly "fix" by catching the exception. Find out why this is happening in URP.
                try
                {
                    Blit(cmd, source, BlurRenderer.RenderTargetHandleA, BlurRenderer.Material, 0);
                }
                catch (MissingReferenceException e)
                {
                    if (e.Message.Contains("RenderTexture"))
                    {
#if UNITY_EDITOR
                        Logger.LogWarning(e.Message);
#endif

                        // Textures are null. Clear them and then try again.
                        BlurRenderer.ClearRenderTargets();
                        Blit(cmd, source, BlurRenderer.RenderTargetHandleA, BlurRenderer.Material, 0);
                    }
                    else
                    {
                        throw e;
                    }
                }
                Blit(cmd, BlurRenderer.RenderTargetHandleA, BlurRenderer.RenderTargetHandleB, BlurRenderer.Material, 1);
                BlurRenderer.AreTexturesSwapped = true;

                // All other blur passes play ping pong between A and B
                for (int i = 1; i < BlurRenderer.Iterations; i++)
                {
                    if (BlurRenderer.AreTexturesSwapped)
                    {
                        Blit(cmd, BlurRenderer.RenderTargetHandleB, BlurRenderer.RenderTargetHandleA, BlurRenderer.Material, 0);
                        Blit(cmd, BlurRenderer.RenderTargetHandleA, BlurRenderer.RenderTargetHandleB, BlurRenderer.Material, 1);
                    }
                    else
                    {
                        Blit(cmd, BlurRenderer.RenderTargetHandleA, BlurRenderer.RenderTargetHandleB, BlurRenderer.Material, 0);
                        Blit(cmd, BlurRenderer.RenderTargetHandleB, BlurRenderer.RenderTargetHandleA, BlurRenderer.Material, 1);
                    }
                }
            }
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            base.OnCameraCleanup(cmd);
        }
    }
}
#endif