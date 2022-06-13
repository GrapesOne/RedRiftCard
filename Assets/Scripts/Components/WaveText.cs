using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveText : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;

    [SerializeField] private float time = 1, scaleX = 0.3f, scaleSin = 0.3f;

    void OnEnable()
    {
        textComponent.OnPreRenderText += Inner;
    }

    void OnDisable()
    {
        textComponent.OnPreRenderText -= Inner;
    }

    private void Inner(TMP_TextInfo textInfo)
    {
        if (textInfo == null) return;
        for (var i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            var verticles = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            for (var j = 0; j < 4; j++)
            {
                var v = verticles[charInfo.vertexIndex + j];
                verticles[charInfo.vertexIndex + j] = v + new Vector3(0, Mathf.Sin(time + v.x * scaleX) * scaleSin, 0);
            }
        }
    }

    void OnValidate()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;
        if (textComponent.textInfo == null) return;
        Inner(textInfo);

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            if (meshInfo.mesh == null) continue;
            meshInfo.mesh.vertices = meshInfo.vertices;
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}