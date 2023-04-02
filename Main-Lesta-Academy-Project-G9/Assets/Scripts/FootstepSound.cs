using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [System.Serializable]
    public struct SoundTemplates
    {
        public string[] textureNames;
        public AudioClip[] audioClips;
    }
    [SerializeField] private SoundTemplates[] soundTemplates;
    [SerializeField] private AudioClip[] defaultStep;
    [SerializeField] private AudioSource audioSource;

    public void PlayFootstep()
    {
        RaycastHit hit;
        AudioClip clip = defaultStep[Random.Range(0, defaultStep.Length)];
        if (Physics.Raycast(transform.position + new Vector3(0f,0.1f,0f), Vector3.down, out hit, 0.2f))
        {
            Material mat = findRaycastMaterial(hit);
            if (mat != null)
            {
                if (mat.GetTexture("_MainTex") == null) return;
                string type = mat.GetTexture("_MainTex").name;
                foreach(SoundTemplates step in soundTemplates)
                {
                    if(step.textureNames.Contains(type))
                    {
                        clip = step.audioClips[Random.Range(0, step.audioClips.Length)];
                        break;
                    }
                }
            }
        }

        audioSource.PlayOneShot(clip);
    }

    Material findRaycastMaterial(RaycastHit hit)
    {
        if (hit.collider == null)
            return null;
        if (hit.collider.TryGetComponent(out MeshRenderer mr))
        {
            return mr.sharedMaterial;
        }
        /*MeshFilter mf = hit.collider.GetComponent<MeshFilter>();
        if (mf == null)
            return null;
        Mesh mesh = mf.sharedMesh;
        var triangles = mesh.triangles;

        var v0 = triangles[hit.triangleIndex * 3 + 0];
        var v1 = triangles[hit.triangleIndex * 3 + 1];
        var v2 = triangles[hit.triangleIndex * 3 + 2];

        Material[] materials = null;
        if (hit.collider.TryGetComponent(out MeshRenderer mr))
        {
            materials = mr.materials;
        }
        if (materials == null)
            return null;

        for (var m = 0; m < materials.Length; m++)
        {
            int[] mts = mesh.GetTriangles(m);
            for (var i = 0; i < mts.Length; i += 3)
            {
                if (mts[i] == v0 && mts[i + 1] == v1 && mts[i + 2] == v2)
                {
                    return materials[m];
                }
            }
        }*/
        return null;
    }
}