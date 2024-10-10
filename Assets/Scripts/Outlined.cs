using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Outlined : MonoBehaviour
{
	Material outlineMat;
    // Start is called before the first frame update
    void Start()
    {
       outlineMat = Resources.Load("OutlineShader/Outline", typeof(Material)) as Material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void TurnOnShader(){
		MeshRenderer[] meshRenderers = GetComponents<MeshRenderer>();
		for(int i = 0; i < meshRenderers.Length; i++){
			List<Material> tempList = meshRenderers[i].materials.ToList();
			tempList.Add(outlineMat);
			meshRenderers[i].materials = tempList.ToArray();
		}

		MeshRenderer[] meshRenderersInChildren = GetComponentsInChildren<MeshRenderer>();
		for(int i = 0; i < meshRenderersInChildren.Length; i++){
			List<Material> tempList = meshRenderersInChildren[i].materials.ToList();
			tempList.Add(outlineMat);
			meshRenderersInChildren[i].materials = tempList.ToArray();
		}
	}

	public void TurnOffShader(){
		
		MeshRenderer[] meshRenderers = GetComponents<MeshRenderer>();
		for(int i = 0; i < meshRenderers.Length; i++){
			List<Material> tempList = meshRenderers[i].materials.ToList();
			for(int o = 0; o < tempList.Count; o++){
				if(tempList[o].name.Split(' ')[0] == outlineMat.name){
					tempList.RemoveAt(o);
					o--;
				}
			}
			meshRenderers[i].materials = tempList.ToArray();
		}

		MeshRenderer[] meshRenderersInChildren = GetComponentsInChildren<MeshRenderer>();
		for(int i = 0; i < meshRenderersInChildren.Length; i++){
			List<Material> tempList = meshRenderersInChildren[i].materials.ToList();
			for(int o = 0; o < tempList.Count; o++){
				if(tempList[o].name.Split(' ')[0] == outlineMat.name){
					tempList.RemoveAt(o);
					o--;
				}
			}
			meshRenderersInChildren[i].materials = tempList.ToArray();
		}
	}
}
