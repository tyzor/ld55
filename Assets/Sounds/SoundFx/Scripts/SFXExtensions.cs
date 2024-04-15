using UnityEngine;
using UnityEngine.Assertions;

namespace Audio.SoundFX
{
    public static class SFXExtensions
    {
        public static void PlaySoundAtLocation(this SFX sfx, Vector3 worldPosition)
        {
            var sfxManager = SfxManager;
            
            Assert.IsNotNull(sfxManager, $"Missing the {nameof(SfxManager)} in the Scene!!");
            sfxManager.PlaySoundAtLocation(sfx, worldPosition);
        }
        
        public static void PlaySound(this SFX sfx, float volume = 1f)
        {
            var sfxManager = SfxManager;
            
            Assert.IsNotNull(sfxManager, $"Missing the {nameof(SfxManager)} in the Scene!!");
            sfxManager.PlaySoundAtLocation(sfx, volume);
        }

        public static void PlaySoundDelayed(this SFX sfx, float delay = 0)
        {
            var sfxManager = SfxManager;
            
            Assert.IsNotNull(sfxManager, $"Missing the {nameof(SfxManager)} in the Scene!!");
            sfxManager.PlaySoundDelayed(sfx, delay);
        }

        public static void PlaySoundDelayedRandom(this SFX sfx, float minRange = 0f, float maxRange = 1f)
        {
            float delay = UnityEngine.Random.Range(minRange,maxRange);
            PlaySoundDelayed(sfx, delay);
        }

        private static SFXManager SfxManager => SFXManager.Instance;
    }
}