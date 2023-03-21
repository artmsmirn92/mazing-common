/// Credit glennpow, Zarlang
/// Sourced from - http://forum.unity3d.com/threads/free-script-particle-systems-in-ui-screen-space-overlay.406862/
/// Updated by Zarlang with a more robust implementation, including TextureSheet annimation support

using UnityEngine;
using UnityEngine.UI;

namespace mazing.common.Runtime
{
#if UNITY_5_3_OR_NEWER
    [ExecuteInEditMode]
    [RequireComponent(typeof(CanvasRenderer), typeof(ParticleSystem))]
    [AddComponentMenu("UI/Effects/Extensions/UIParticleSystem")]
    public class UIParticleSystem : MaskableGraphic
    {
        [Tooltip("Having this enabled run the system in LateUpdate rather than in Update making it faster but less precise (more clunky)")]
        public bool fixedTime = true;

        [Tooltip("Enables 3d rotation for the particles")]
        public bool use3dRotation;

        [SerializeField] private Texture currentTexture;
        
        private readonly UIVertex[] m_Quad = new UIVertex[4];

        private ParticleSystem.TextureSheetAnimationModule m_TextureSheetAnimation;
        private ParticleSystem.Particle[]                  m_Particles;
        
        private Transform              m_Transform;
        private ParticleSystem         m_PSystem;
        private Vector4                m_ImageUV = Vector4.zero;
        private int                    m_TextureSheetAnimationFrames;
        private Vector2                m_TextureSheetAnimationFrameSize;
        private ParticleSystemRenderer m_PRenderer;
        private bool                   m_IsInitialised;
        private Material               m_CurrentMaterial;


#if UNITY_5_5_OR_NEWER
        private ParticleSystem.MainModule m_MainModule;
#endif

        public override Texture mainTexture => currentTexture;

        private bool Initialize()
        {
            // initialize members
            if (m_Transform == null)
            {
                m_Transform = transform;
            }
            if (m_PSystem == null)
            {
                m_PSystem = GetComponent<ParticleSystem>();

                if (m_PSystem == null)
                {
                    return false;
                }

#if UNITY_5_5_OR_NEWER
                m_MainModule = m_PSystem.main;
                if (m_PSystem.main.maxParticles > 14000)
                {
                    m_MainModule.maxParticles = 14000;
                }
#else
                    if (pSystem.maxParticles > 14000)
                        pSystem.maxParticles = 14000;
#endif

                m_PRenderer = m_PSystem.GetComponent<ParticleSystemRenderer>();
                if (m_PRenderer != null)
                    m_PRenderer.enabled = false;

                if (material == null)
                {
                    var foundShader = Shader.Find("UI Extensions/Particles/Additive");
                    if (foundShader)
                    {
                        material = new Material(foundShader);
                    }
                }

                m_CurrentMaterial = material;
                if (m_CurrentMaterial && m_CurrentMaterial.HasProperty("_MainTex"))
                {
                    currentTexture = m_CurrentMaterial.mainTexture;
                    if (currentTexture == null)
                        currentTexture = Texture2D.whiteTexture;
                }
                material = m_CurrentMaterial;
                // automatically set scaling
#if UNITY_5_5_OR_NEWER
                m_MainModule.scalingMode = ParticleSystemScalingMode.Hierarchy;
#else
                    pSystem.scalingMode = ParticleSystemScalingMode.Hierarchy;
#endif

                m_Particles = null;
            }
#if UNITY_5_5_OR_NEWER
            if (m_Particles == null)
                m_Particles = new ParticleSystem.Particle[m_PSystem.main.maxParticles];
#else
                if (particles == null)
                    particles = new ParticleSystem.Particle[pSystem.maxParticles];
#endif

            m_ImageUV = new Vector4(0, 0, 1, 1);

            // prepare texture sheet animation
            m_TextureSheetAnimation = m_PSystem.textureSheetAnimation;
            m_TextureSheetAnimationFrames = 0;
            m_TextureSheetAnimationFrameSize = Vector2.zero;
            if (m_TextureSheetAnimation.enabled)
            {
                m_TextureSheetAnimationFrames = m_TextureSheetAnimation.numTilesX * m_TextureSheetAnimation.numTilesY;
                m_TextureSheetAnimationFrameSize = new Vector2(1f / m_TextureSheetAnimation.numTilesX, 1f / m_TextureSheetAnimation.numTilesY);
            }

            return true;
        }

        protected override void Awake()
        {
            base.Awake();
            if (!Initialize())
                enabled = false;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (!Initialize())
                {
                    return;
                }
            }
#endif
            // prepare vertices
            vh.Clear();

            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            if (!m_IsInitialised && !m_PSystem.main.playOnAwake)
            {
                m_PSystem.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
                m_IsInitialised = true;
            }

            Vector2 temp = Vector2.zero;
            Vector2 corner1 = Vector2.zero;
            Vector2 corner2 = Vector2.zero;
            // iterate through current particles
            int count = m_PSystem.GetParticles(m_Particles);

            for (int i = 0; i < count; ++i)
            {
                ParticleSystem.Particle particle = m_Particles[i];

                // get particle properties
#if UNITY_5_5_OR_NEWER
                Vector2 position = (m_MainModule.simulationSpace == ParticleSystemSimulationSpace.Local ? particle.position : m_Transform.InverseTransformPoint(particle.position));
#else
                    Vector2 position = (pSystem.simulationSpace == ParticleSystemSimulationSpace.Local ? particle.position : _transform.InverseTransformPoint(particle.position));
#endif
                float rotation = -particle.rotation * Mathf.Deg2Rad;
                float rotation90 = rotation + Mathf.PI / 2;
                Color32 color = particle.GetCurrentColor(m_PSystem);
                float size = particle.GetCurrentSize(m_PSystem) * 0.5f;

                // apply scale
#if UNITY_5_5_OR_NEWER
                if (m_MainModule.scalingMode == ParticleSystemScalingMode.Shape)
                    position /= canvas.scaleFactor;
#else
                    if (pSystem.scalingMode == ParticleSystemScalingMode.Shape)
                        position /= canvas.scaleFactor;
#endif

                // apply texture sheet animation
                Vector4 particleUV = m_ImageUV;
                if (m_TextureSheetAnimation.enabled)
                {
#if UNITY_5_5_OR_NEWER
                    float frameProgress = 1 - (particle.remainingLifetime / particle.startLifetime);

                    if (m_TextureSheetAnimation.frameOverTime.curveMin != null)
                    {
                        frameProgress = m_TextureSheetAnimation.frameOverTime.curveMin.Evaluate(1 - (particle.remainingLifetime / particle.startLifetime));
                    }
                    else if (m_TextureSheetAnimation.frameOverTime.curve != null)
                    {
                        frameProgress = m_TextureSheetAnimation.frameOverTime.curve.Evaluate(1 - (particle.remainingLifetime / particle.startLifetime));
                    }
                    else if (m_TextureSheetAnimation.frameOverTime.constant > 0)
                    {
                        frameProgress = m_TextureSheetAnimation.frameOverTime.constant - (particle.remainingLifetime / particle.startLifetime);
                    }
#else
                    float frameProgress = 1 - (particle.lifetime / particle.startLifetime);
#endif

                    frameProgress = Mathf.Repeat(frameProgress * m_TextureSheetAnimation.cycleCount, 1);
                    int frame = 0;

                    switch (m_TextureSheetAnimation.animation)
                    {

                        case ParticleSystemAnimationType.WholeSheet:
                            frame = Mathf.FloorToInt(frameProgress * m_TextureSheetAnimationFrames);
                            break;

                        case ParticleSystemAnimationType.SingleRow:
                            frame = Mathf.FloorToInt(frameProgress * m_TextureSheetAnimation.numTilesX);

                            int row = m_TextureSheetAnimation.rowIndex;
                            //                    if (textureSheetAnimation.useRandomRow) { // FIXME - is this handled internally by rowIndex?
                            //                        row = Random.Range(0, textureSheetAnimation.numTilesY, using: particle.randomSeed);
                            //                    }
                            frame += row * m_TextureSheetAnimation.numTilesX;
                            break;

                    }

                    frame %= m_TextureSheetAnimationFrames;

                    particleUV.x = (frame % m_TextureSheetAnimation.numTilesX) * m_TextureSheetAnimationFrameSize.x;
                    particleUV.y = 1.0f - Mathf.FloorToInt(frame / m_TextureSheetAnimation.numTilesX) * m_TextureSheetAnimationFrameSize.y;
                    particleUV.z = particleUV.x + m_TextureSheetAnimationFrameSize.x;
                    particleUV.w = particleUV.y + m_TextureSheetAnimationFrameSize.y;
                }

                temp.x = particleUV.x;
                temp.y = particleUV.y;

                m_Quad[0] = UIVertex.simpleVert;
                m_Quad[0].color = color;
                m_Quad[0].uv0 = temp;

                temp.x = particleUV.x;
                temp.y = particleUV.w;
                m_Quad[1] = UIVertex.simpleVert;
                m_Quad[1].color = color;
                m_Quad[1].uv0 = temp;

                temp.x = particleUV.z;
                temp.y = particleUV.w;
                m_Quad[2] = UIVertex.simpleVert;
                m_Quad[2].color = color;
                m_Quad[2].uv0 = temp;

                temp.x = particleUV.z;
                temp.y = particleUV.y;
                m_Quad[3] = UIVertex.simpleVert;
                m_Quad[3].color = color;
                m_Quad[3].uv0 = temp;

                if (rotation == 0)
                {
                    // no rotation
                    corner1.x = position.x - size;
                    corner1.y = position.y - size;
                    corner2.x = position.x + size;
                    corner2.y = position.y + size;

                    temp.x = corner1.x;
                    temp.y = corner1.y;
                    m_Quad[0].position = temp;
                    temp.x = corner1.x;
                    temp.y = corner2.y;
                    m_Quad[1].position = temp;
                    temp.x = corner2.x;
                    temp.y = corner2.y;
                    m_Quad[2].position = temp;
                    temp.x = corner2.x;
                    temp.y = corner1.y;
                    m_Quad[3].position = temp;
                }
                else
                {
                    if (use3dRotation)
                    {
                        // get particle properties
#if UNITY_5_5_OR_NEWER
                        Vector3 pos3d = (m_MainModule.simulationSpace == ParticleSystemSimulationSpace.Local ? particle.position : m_Transform.InverseTransformPoint(particle.position));
#else
                        Vector3 pos3d = (pSystem.simulationSpace == ParticleSystemSimulationSpace.Local ? particle.position : _transform.InverseTransformPoint(particle.position));
#endif

                        // apply scale
#if UNITY_5_5_OR_NEWER
                        if (m_MainModule.scalingMode == ParticleSystemScalingMode.Shape)
                            position /= canvas.scaleFactor;
#else
                        if (pSystem.scalingMode == ParticleSystemScalingMode.Shape)
                            position /= canvas.scaleFactor;
#endif

                        Vector3[] verts = new Vector3[4]
                        {
                            new Vector3(-size, -size, 0),
                            new Vector3(-size, size, 0),
                            new Vector3(size, size, 0),
                            new Vector3(size, -size, 0)
                        };

                        Quaternion particleRotation = Quaternion.Euler(particle.rotation3D);

                        m_Quad[0].position = pos3d + particleRotation * verts[0];
                        m_Quad[1].position = pos3d + particleRotation * verts[1];
                        m_Quad[2].position = pos3d + particleRotation * verts[2];
                        m_Quad[3].position = pos3d + particleRotation * verts[3];
                    }
                    else
                    {
                        // apply rotation
                        Vector2 right = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation)) * size;
                        Vector2 up = new Vector2(Mathf.Cos(rotation90), Mathf.Sin(rotation90)) * size;

                        m_Quad[0].position = position - right - up;
                        m_Quad[1].position = position - right + up;
                        m_Quad[2].position = position + right + up;
                        m_Quad[3].position = position + right - up;
                    }
                }

                vh.AddUIVertexQuad(m_Quad);
            }
        }

        private void Update()
        {
            if (!fixedTime && Application.isPlaying)
            {
                m_PSystem.Simulate(Time.unscaledDeltaTime, false, false, true);
                SetAllDirty();

                if ((m_CurrentMaterial != null && currentTexture != m_CurrentMaterial.mainTexture) ||
                    (material != null && m_CurrentMaterial != null && material.shader != m_CurrentMaterial.shader))
                {
                    m_PSystem = null;
                    Initialize();
                }
            }
        }

        private void LateUpdate()
        {
            if (!Application.isPlaying)
            {
                SetAllDirty();
            }
            else
            {
                if (fixedTime)
                {
                    m_PSystem.Simulate(Time.unscaledDeltaTime, false, false, true);
                    SetAllDirty();
                    if ((m_CurrentMaterial != null && currentTexture != m_CurrentMaterial.mainTexture) ||
                        (material != null && m_CurrentMaterial != null && material.shader != m_CurrentMaterial.shader))
                    {
                        m_PSystem = null;
                        Initialize();
                    }
                }
            }
            if (material == m_CurrentMaterial)
                return;
            m_PSystem = null;
            Initialize();
        }

        protected override void OnDestroy()
        {
            m_CurrentMaterial = null;
            currentTexture = null;
        }

        public void StartParticleEmission()
        {
            m_PSystem.Play();
        }

        public void StopParticleEmission()
        {
            m_PSystem.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        public void PauseParticleEmission()
        {
            m_PSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }
    }
#endif
}