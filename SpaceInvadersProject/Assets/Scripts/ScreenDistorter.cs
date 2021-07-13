using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace SpaceInvaders
{
    [DisallowMultipleComponent]
    public class ScreenDistorter : MonoBehaviour
    {
        [SerializeField, Range(0f, 10f)]
        private float distortionStrength = 2f;
        [SerializeField]
        private AnimationCurve distortionCurve;

        private ChromaticAberration chromaticAberration;
        private float defaultDistortion;

        private float Distortion
        {
            get => chromaticAberration.intensity.value;
            set => chromaticAberration.intensity.value = value;
        }

        public void DistortScreen(float duration)
        {
            _ = StartCoroutine(ScreenDistortionCoroutine(duration));
        }

        public void Abort()
        {
            StopAllCoroutines();
            Distortion = defaultDistortion;
        }

        private void Awake()
        {
            Camera mainCamera = Camera.main;
            PostProcessVolume volume = mainCamera.GetComponent<PostProcessVolume>();
            chromaticAberration = volume.profile.settings.Find(effect => effect is ChromaticAberration) as ChromaticAberration;
            defaultDistortion = Distortion;
        }

        private IEnumerator ScreenDistortionCoroutine(float duration)
        {
            defaultDistortion = Distortion;

            float time = 0f;
            while (time < duration)
            {
                float t = time / duration;
                time += Time.deltaTime;

                Distortion = distortionCurve.Evaluate(t) * distortionStrength;

                yield return null;
            }

            Distortion = defaultDistortion;
        }
    }
}