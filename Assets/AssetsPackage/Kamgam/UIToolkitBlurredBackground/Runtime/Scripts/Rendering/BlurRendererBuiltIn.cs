#if !KAMGAM_RENDER_PIPELINE_HDRP && !KAMGAM_RENDER_PIPELINE_URP
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Kamgam.UIToolkitBlurredBackground
{
    public class BlurRendererBuiltIn : IBlurRenderer
    {
        /// <summary>
        /// Activate or deactivate the renderer. Disable to save performance (no rendering will be done).
        /// </summary>
        public bool Active
        {
            get => BlurRenderEventForwarder.Active;
            set { BlurRenderEventForwarder.Active = value; }
        }

        protected int _iterations = 1;
        public int Iterations
        {
            get => _iterations;
            set => _iterations = value; 
        }

        protected float _offset = 10f;
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

        public const string ShaderName = "Kamgam/UI Toolkit/BuiltIn/Blur Shader";

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
        protected RenderTexture renderTargetBlurredA
        {
            get
            {
                if (_renderTargetBlurredA == null)
                    _renderTargetBlurredA = createRenderTexture();

                return _renderTargetBlurredA;
            }
        }

        [System.NonSerialized]
        protected RenderTexture _renderTargetBlurredB;
        protected RenderTexture renderTargetBlurredB
        {
            get
            {
                if (_renderTargetBlurredB == null)
                    _renderTargetBlurredB = createRenderTexture();

                return _renderTargetBlurredB;
            }
        }

        RenderTexture createRenderTexture()
        {
            var texture = new RenderTexture(Resolution.x, Resolution.y, 16);
            texture.filterMode = FilterMode.Bilinear;

            return texture;
        }

        bool _texturesAreSwapped;

        public Texture GetBlurredTexture()
        {
            return _texturesAreSwapped ? renderTargetBlurredB : renderTargetBlurredA;
        }

        public BlurRendererBuiltIn()
        {
            BlurRenderEventForwarder.OnPreRenderFunc += OnPreRender;
            BlurRenderEventForwarder.OnRenderImageFunc += OnRenderImage;
        }

        /// <summary>
        /// Returns true if the a new main camera was detected.
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            return BlurRenderEventForwarder.Update();
        }

        // This (see OnPreRender) is done due to an ancient (2016) Unity forum thread which mentions that
        // if no render texture is assigned Unity will copy the texture pixel by pixel (which is slow).
        // See: https://forum.unity.com/threads/post-process-mobile-performance-alternatives-to-graphics-blit-onrenderimage.414399/#post-2759255
        RenderTexture _rt;

        void OnPreRender(Camera cam)
        {
            _rt = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);
            cam.targetTexture = _rt;
        }

        void OnRenderImage(Camera cam, RenderTexture src, RenderTexture dest)
        {
            // See OnPreRender
            cam.targetTexture = null;
            RenderTexture.ReleaseTemporary(_rt);

            // If BlurIterations is <= 0 then we will do nothing.
            if (Iterations > 0 && Offset > 0)
            {
                // Copy from source to A (sadly this is needed because otherwise it sometimes does not execute on or the other pass, TODO: investigate)
                Graphics.Blit(src, renderTargetBlurredA); 
                _texturesAreSwapped = false;

                // All other blur passes play ping pong between A and B
                for (int i = 0; i < Iterations; i++)
                {
                    if (_texturesAreSwapped)
                    {
                        Graphics.Blit(renderTargetBlurredB, renderTargetBlurredA, Material, 0);
                        Graphics.Blit(renderTargetBlurredA, renderTargetBlurredB, Material, 1);
                    }
                    else
                    {
                        Graphics.Blit(renderTargetBlurredA, renderTargetBlurredB, Material, 0);
                        Graphics.Blit(renderTargetBlurredB, renderTargetBlurredA, Material, 1);
                    }
                }
            }

            // Since we did render into a render texture (see OnPreRender) we now
            // have to blit the contents to the frame buffer.
            Graphics.Blit(src, null as RenderTexture);

            // Otherwise we will get a "OnRenderImage() possibly didn't write anything
            // to the destination texture!" warning.
            Graphics.Blit(src, dest);
        }
    }
}
#endif