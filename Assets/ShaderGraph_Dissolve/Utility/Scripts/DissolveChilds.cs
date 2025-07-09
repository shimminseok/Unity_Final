using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace DissolveExample
{
    public class DissolveChilds : MonoBehaviour
    {
        [SerializeField] private Material dissolveMaterial;
        private Renderer renderer;
        private Material[] originalMaterial;

        void Start()
        {
            renderer = GetComponent<Renderer>();
            int index             = 0;
            var dissolveMaterials = new Material[renderer.materials.Length];
            foreach (var material in renderer.materials)
            {
                var originalTexture     = material.GetTexture("_MainTex");
                var newDissolveMaterial = new Material(dissolveMaterial);
                newDissolveMaterial.SetTexture("_BaseMap", originalTexture);
                dissolveMaterials[index++] = newDissolveMaterial;
            }

            renderer.materials = dissolveMaterials;
        }

        public void PlayDissolve(float delayTime = 1.5f)
        {
            foreach (var mat in renderer.materials)
            {
                // 초기값 설정 (선택 사항)
                mat.SetFloat("_Dissolve", 0f);

                // Dissolve 값을 1로 애니메이션
                mat.DOFloat(1f, "_Dissolve", 1.5f).SetDelay(delayTime)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        // 연출 끝났을 때 처리 (예: 오브젝트 비활성화)
                        gameObject.transform.parent.gameObject.SetActive(false);
                        mat.SetFloat("_Dissolve", 1f);
                    });
            }
        }
    }
}