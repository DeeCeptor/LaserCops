using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PixelAsset), true)]
public class PixelAssetEditor : Editor {

	public override void OnInspectorGUI(){
		PixelAsset gen = (PixelAsset)target;

		DrawDefaultInspector();

		ShowGenerationButtons(gen);
	}

	protected void ShowGenerationButtons(PixelAsset gen){

		if(GUILayout.Button("Generate Sprite")){
			gen.GenerateAsset();
		}

	}
}
