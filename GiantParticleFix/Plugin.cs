using BepInEx;
using RoR2;

namespace GiantParticleFix
{
    [BepInPlugin("com.Moffein.GiantParticleFix", "GiantParticleFix", "1.0.1")]
    public class GiantParticleFix : BaseUnityPlugin
    {
        public void Awake()
        {
            On.RoR2.NormalizeParticleScale.OnEnable += ApplyEnableOnce;
        }

        private void ApplyEnableOnce(On.RoR2.NormalizeParticleScale.orig_OnEnable orig, NormalizeParticleScale self)
        {
            if (!(bool)self.particleSystem)
                orig(self);
        }
    }
}