using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor {
    FieldOfView fow;
    Color lineColor;
    void OnSceneGUI() {
		 fow= (FieldOfView)target;
		Handles.color= lineColor;
		Handles.DrawWireArc (fow.transform.position, Vector3.forward, Vector3.up,  360, fow.viewRadius);
		Vector3 viewAngleA = fow.DirFromAngle (-fow.viewAngle / 2, false);
		Vector3 viewAngleB = fow.DirFromAngle (fow.viewAngle / 2, false);

        Handles.color = Color.red;
        Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
		Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

		foreach (Transform visibleTarget in fow.visibleTargets) {
			Handles.DrawLine (fow.transform.position, visibleTarget.position);
		}
    }
    Color Transparency;
    Texture2D outTex;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Transparency Color", GUILayout.Width(130));
        Transparency = EditorGUILayout.ColorField(Transparency);
        if(Transparency.a==0)
            Transparency.a = 255;//necessary for JPG
        EditorGUILayout.LabelField("lineColor",GUILayout.Width(80));
        lineColor = EditorGUILayout.ColorField(lineColor);
        EditorGUILayout.EndHorizontal();
        if (EditorGUILayout.DropdownButton(new GUIContent("Generate"), FocusType.Keyboard))
        {
            Texture2D t = fow.Iin;
            outTex = new Texture2D(t.width, t.height);
            Color[] c = t.GetPixels();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i]==Transparency)
                {
                    c[i] = Color.clear;
                }
            }
            
            outTex.SetPixels(c);
            outTex.Apply();
            File.WriteAllBytes(Application.dataPath + "/"+fow.Iin.name+ "Transparency.png",
            outTex.EncodeToPNG());
            Debug.Log("already accomplish");
        }
    }
}
