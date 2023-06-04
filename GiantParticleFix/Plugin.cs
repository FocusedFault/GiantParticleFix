using BepInEx;
using RoR2;
using UnityEngine;
using System;
using BepInEx.Configuration;
using System.Runtime.CompilerServices;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;

namespace GiantParticleFix
{
    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin("com.Moffein.GiantParticleFix", "GiantParticleFix", "1.0.0")]
    public class GiantParticleFix : BaseUnityPlugin
    {
        public void Awake()
        {
            On.RoR2.BurnEffectController.AddFireParticles += BurnEffectController_AddFireParticles;
        }

        private GameObject BurnEffectController_AddFireParticles(On.RoR2.BurnEffectController.orig_AddFireParticles orig, BurnEffectController self, Renderer modelRenderer, Transform targetParentTransform)
        {
            GameObject toReturn =  orig(self, modelRenderer, targetParentTransform);
            if (toReturn && toReturn.transform && modelRenderer && modelRenderer.transform)
            {
                float max = Mathf.Max(modelRenderer.transform.localScale.x , modelRenderer.transform.localScale.y, modelRenderer.transform.localScale.z);
                if (max > 1f)
                {
                    NormalizeParticleScale nps = toReturn.GetComponent<NormalizeParticleScale>();
                    if (nps)
                    {
                        ParticleSystem ps = toReturn.GetComponent<ParticleSystem>();
                        if (ps)
                        {
                            ParticleSystem.MainModule main = ps.main;
                            ParticleSystem.MinMaxCurve startSize = main.startSize;

                            //Todo: figure out how to downscale spread range as well
                            startSize.constantMin /= max;
                            startSize.constantMax /= max;
                            main.startSize = startSize; //need to do this to make changes actually apply
                        }
                    }
                }
            }
            return toReturn;
        }
    }
}

namespace R2API.Utils
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ManualNetworkRegistrationAttribute : Attribute
    {
    }
}