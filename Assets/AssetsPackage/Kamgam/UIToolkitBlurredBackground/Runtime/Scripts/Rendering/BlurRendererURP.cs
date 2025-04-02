#if KAMGAM_RENDER_PIPELINE_URP && !KAMGAM_RENDER_PIPELINE_HDRP
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace Kamgam.UIToolkitBlurredBackground
{
    public class BlurRendererURP : IBlurRenderer
    {
        protected bool _active = true;

        /// <summary>
        /// Activate or deactivate the renderer. Disable to save performance (no rendering will be done).
        /// </summary>
        public bool Active
        {
            get
            {
                return _active;
            }

            set
            {
                if (_active != value)
                {
                    _active = value;
                    Pass.Active = value;
                }
            }
        }

        protected int _iterations;
        public int Iterations
        {
            get => _iterations;
            set
            {
                if (_iterations != value)
                {
                    _iterations = value;
                    Active = _iterations > 0;
                }
            }
        }

        protected float _offset = 15f;
        public float Offset
        {
            get => _offset;
            set
            {
                _offset = value;
                setOffset(value);
            }
        }

        protected Vector2Int _resolution = new Vector2Int(512, 512);
        /// <summary>
        /// The texture resolution of the blurred image. Default is 512 x 512. Please use 2^n values like 256, 512, 1024, 2048. Reducing this will increase performance but decrease quality. Every frame your rendered image will be copied, resized and then blurred [BlurStrength] times.
        /// </summary>
        public Vector2Int Resolution
        {
            get => _resolution;
            set
            {
                _resolution = value;
                updateRenderTextureResolutions();
            }
        }

        void updateRenderTextureResolutions()
        {
            if (_renderTargetBlurredA != null)
            {
                _renderTargetBlurredA.Release();
                _renderTargetBlurredA.width = _resolution.x;
                _renderTargetBlurredA.height = _resolution.y;
                _renderTargetBlurredA.Create();
            }

            if (_renderTargetBlurredB != null)
            {
                _renderTargetBlurredB.Release();
                _renderTargetBlurredB.width = _resolution.x;
                _renderTargetBlurredB.height = _resolution.y;
                _renderTargetBlurredB.Create();
            }
        }

        public const string ShaderName = "Kamgam/UI Toolkit/URP/Blur Shader";

        protected ShaderQuality _quality = ShaderQuality.Medium;
        public ShaderQuality Quality
        {
            get => _quality;
            set
            {
                _quality = value;
                _material = null;
            }
        }

        [System.NonSerialized]
        protected Material _material;
        public Material Material
        {
            get
            {
                if (_material == null)
                {
                    // Create material with shader
                    var shader = Shader.Find(ShaderName);
                    if (shader != null)
                    {
                        _material = new Material(shader);
                        _material.color = Color.white;

                        switch (_quality)
                        {
                            case ShaderQuality.Low:
                                _material.SetKeyword(new LocalKeyword(shader, "_SAMPLES_LOW"), true);
                                _material.SetKeyword(new LocalKeyword(shader, "_SAMPLES_MEDIUM"), false);
                                _material.SetKeyword(new LocalKeyword(shader, "_SAMPLES_HIGH"), false);
                                break;

                            case ShaderQuality.Medium:
                                _material.SetKeyword(new LocalKeyword(shader, "_SAMPLES_LOW"), false);
                                _material.SetKeyword(new LocalKeyword(shader, "_SAMPLES_MEDIUM"), true);
                                _material.SetKeyword(new LocalKeyword(shader, "_SAMPLES_HIGH"), false);
                                break;

                            case ShaderQuality.High:
                                _material.SetKeyword(new LocalKeyword(shader, "_SAMPLES_LOW"), false);
                                _material.SetKeyword(new LocalKeyword(shader, "_SAMPLES_MEDIUM"), false);
                                _material.SetKeyword(new LocalKeyword(shader, "_SAMPLES_HIGH"), true);
                                break;

                            default:
                                break;
                        }

                        setOffset(_offset);
                    }
                }
                return _material;
            }

            set
            {
                _material = value;
            }
        }

        void setOffset(float value)
        {
            if (_material != null)
                _material.SetVector("_BlurOffset", new Vector4(value, value, 0f, 0f));
        }

        [System.NonSerialized]
        protected RenderTexture _renderTargetBlurredA;
        public RenderTexture RenderTargetBlurredA
        {
            get
            {
                CreateRenderTextureTargetBlurredAIfNeeded();
                return _renderTargetBlurredA;
            }
        }

        public void CreateRenderTextureTargetBlurredAIfNeeded()
        {
            if (_renderTargetBlurredA == null || !_renderTargetBlurredA.IsCreated())
            {
                _renderTargetBlurredA = createRenderTexture();

                if (_renderTargetHandleA != null)
                {
                    _renderTargetHandleA.Release();
                    _renderTargetHandleA = null;
                }
            }
        }

        [System.NonSerialized]
        protected RenderTexture _renderTargetBlurredB;
        public RenderTexture RenderTargetBlurredB
        {
            get
            {
                CreateRenderTextureTargetBlurredBIfNeeded();
                return _renderTargetBlurredB;
            }
        }

        public void CreateRenderTextureTargetBlurredBIfNeeded()
        {
            if (_renderTargetBlurredB == null || !_renderTargetBlurredB.IsCreated())
            {
                _renderTargetBlurredB = createRenderTexture();

                if (_renderTargetHandleB != null)
                {
                    _renderTargetHandleB.Release();
                    _renderTargetHandleB = null;
                }
            }
        }

        [System.NonSerialized]
        protected RTHandle _renderTargetHandleA;
        public RTHandle RenderTargetHandleA
        {
            get
            {
                if (_renderTargetHandleA == null)
                    _renderTargetHandleA = RTHandles.Alloc(RenderTargetBlurredA);

                return _renderTargetHandleA;
            }
        }

        [System.NonSerialized]
        protected RTHandle _renderTargetHandleB;
        public RTHandle RenderTargetHandleB
        {
            get
            {
                if (_renderTargetHandleB == null)
                    _renderTargetHandleB = RTHandles.Alloc(RenderTargetBlurredB);

                return _renderTargetHandleB;
            }
        }

        RenderTexture createRenderTexture()
        {
            var texture = new RenderTexture(Resolution.x, Resolution.y, 16);
            texture.filterMode = FilterMode.Bilinear;

            return texture;
        }

        public bool AreTexturesSwapped;

        public Texture GetBlurredTexture()
        {
            return AreTexturesSwapped ? RenderTargetBlurredB : RenderTargetBlurredA;
        }

        public BlurRendererURP()
        {
            RenderPipelineManager.beginCameraRendering += onBeginCameraRendering;

            // Needed to avoid "Render Pipeline error : the XR layout still contains active passes. Executing XRSystem.EndLayout() right" Errors in Unity 2023
            // Also needed in normal URP to reset the render textures after play mode.
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += onPlayModeChanged;
            UnityEditor.SceneManagement.EditorSceneManager.sceneOpened += onSceneOpened;
#endif
        }

        public void ClearRenderTargets()
        {
            if (_renderTargetHandleA != null)
            {
                _renderTargetHandleA.Release();
                _renderTargetHandleA = null;
            }
            if (_renderTargetBlurredA != null)
            {
                _renderTargetBlurredA.Release();
                _renderTargetBlurredA = null;
            }

            if (_renderTargetHandleB != null)
            {
                _renderTargetHandleB.Release();
                _renderTargetHandleB = null;
            }
            if (_renderTargetBlurredB != null)
            {
                _renderTargetBlurredB.Release();
                _renderTargetBlurredB = null;
            }
        }

#if UNITY_EDITOR
        void onPlayModeChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.ExitingPlayMode || obj == PlayModeStateChange.EnteredEditMode)
            {
                ClearRenderTargets();
            }
        }

        void onSceneOpened(Scene scene, OpenSceneMode mode)
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                ClearRenderTargets();
            }
        }
#endif

        void onBeginCameraRendering(ScriptableRenderContext contest, Camera cam)
        {
            if (cam == null)
                return;

            // All of this is only to support multiple-camera setups with render textures.
            // The blur only needs to be done on one camera (usually the main camera). That's
            // why the stop on all other cameras.
            var mainCam = Camera.main;
            if (mainCam != null)
            {
                if (cam != mainCam)
                    return;
            }
            else
            {
                // No main camera -> let's check if there are cameras that are NOT rendering into render textures.
                Camera firstCamWithoutRenderTexture = null;
                int camCout = Camera.allCamerasCount;
                for (int i = 0; i < camCout; i++)
                {
                    var cCam = Camera.allCameras[i];
                    if (cCam != null && cCam.targetTexture == null)
                    {
                        firstCamWithoutRenderTexture = cCam;
                        break;
                    }
                }

                // If there are some then use the first we an find. Which means we abort the blur pass on all others.
                if (firstCamWithoutRenderTexture != null && cam != firstCamWithoutRenderTexture)
                    return;

                // If there are only cameras with render textures then we ignore it.
                // This means that in setups with cameras that are only rendered in to textures
                // no blur will occur.
                if (firstCamWithoutRenderTexture == null)
                    return;
            }

            var data = cam.GetUniversalAdditionalCameraData();

            if (data == null)
                return;

            // Turns out the list is always empty and the enqueing is a per frame action (TODO: investigate)
            Pass.ScriptableRenderer = data.scriptableRenderer;
            data.scriptableRenderer.EnqueuePass(Pass);

            // Old code which is unoptimized (reflection caching) and we do not need to check the list anyways it seems.
            /*
            var activeRenderPassQueueProp = typeof(ScriptableRenderer).GetProperty("activeRenderPassQueue", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            var list = activeRenderPassQueueProp.GetValue(data.scriptableRenderer) as List<ScriptableRenderPass>;
            if (!list.Contains(Pass))
            {
                Pass.ScriptableRenderer = data.scriptableRenderer;
                data.scriptableRenderer.EnqueuePass(Pass);
            }
            */
        }

        protected BlurredBackgroundPassURP _pass;
        public BlurredBackgroundPassURP Pass
        {
            get
            {
                if (_pass == null)
                {
                    _pass = new BlurredBackgroundPassURP(this);
                    _pass.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
                }
                return _pass;
            }
        }

        /// <summary>
        /// Not needed in SRPs. Returns false.
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            return false;
        }
    }
}
#endif