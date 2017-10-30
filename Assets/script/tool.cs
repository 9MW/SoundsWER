using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using UnityEngine.UI;
//[CustomEditor(typeof(T))]//T is gameobject component,muet be full else if the extension can't work
//out texture will present under assets fold
public class tool : EditorWindow
{
    [MenuItem("Window/My Tool Window")]
    public static void ShowWindow()
    {
      var window=  GetWindow(typeof(tool));
        window.maximized = true;
        //window.position =new Rect(0, 0, 400, 200);
        //window.Show();
    }
   // [ColorUsageAttribute(true,true,)]
    Color SelectedColor;//The color will be instead by 0,0,0,0,
    Texture2D outTex;
   public Texture2D inTex;
    Texture2D NowInTex;
    bool inimageChange=false;
    Texture guiImage;
    public void OnSelectionChange()
    {

    }
    Color[] inTexPixels;
    public  void OnGUI()
    {
        guiImage = new Texture2D(30, 30);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Transparency Color", GUILayout.Width(130));
        SelectedColor = EditorGUILayout.ColorField(SelectedColor);
        //Transparency.a = 1;//necessary for JPG
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginScrollView(new Vector2(500,500));
        //EditorGUILayout.
        inTex = EditorGUI.ObjectField(new Rect(0, 0, 600, 600),
                "In Texture:",
                inTex,
                typeof(Texture2D),false) as Texture2D;
        outTex=EditorGUI.ObjectField(new Rect(650, 0, 600, 600),
                "Out Texture:",
                outTex,
                typeof(Texture2D), false) as Texture2D;
        EditorGUILayout.EndScrollView();
        listExistColor();
        if (EditorGUILayout.DropdownButton(new GUIContent("Generate"), FocusType.Keyboard))
        {
            outTex = new Texture2D(inTex.width, inTex.height);
            ConvertImagePixel(new Color(0,0,0,0), ref inTexPixels, alreadyexist[SelectedColor]);
           
            outTex.SetPixels(inTexPixels);
            outTex.Apply();
            File.WriteAllBytes(Application.dataPath + "/SavedScreen.png",
            outTex.EncodeToPNG());
            Debug.Log("already accomplish,Transparencycolor="+SelectedColor.ToString());
        }
    }
    Dictionary<Color, List<int>> alreadyexist= new Dictionary<Color, List<int>>(10);
    Texture2D[] presentImColor;
    void listExistColor()
    {
        outTex = new Texture2D(inTex.width, inTex.height);
        inTexPixels = inTex.GetPixels();
        if (alreadyexist.Count == 0)
        {
            for (int i = 0; i < inTexPixels.Length; i++)
            {
                if (!alreadyexist.ContainsKey(inTexPixels[i]))
                {
                    alreadyexist.Add(inTexPixels[i], new List<int> { i });
                }
                else
                {
                    alreadyexist[inTexPixels[i]].Add(i);
                }
            }
            Color[] CArray = alreadyexist.Keys.ToArray();
            presentImColor = new Texture2D[alreadyexist.Count];
            for (int i = 0; i < CArray.Length; i++)
            {
                Texture2D img = Instantiate(guiImage) as Texture2D;
                img.SetPixels(setSameColor(img.width * img.height, CArray[i]));
                presentImColor[i] = img;
            }
        }
        Selectedindex = GUILayout.SelectionGrid(-1, presentImColor, presentImColor.Length / 2);
        if (Selectedindex != -1&& Selectedindex != lastIndex)
        {
            
            Debug.Log(Selectedindex + "has been chose");
            SelectedColor = presentImColor[0].GetPixel(0, 0);
            lastIndex = Selectedindex;
            Color[] c1 = outTex.GetPixels();
            ConvertImagePixel(SelectedColor, ref c1, alreadyexist[SelectedColor]);
            outTex.SetPixels(ConvertImagePixel(SelectedColor, ref c1, alreadyexist[SelectedColor]));
            outTex.Apply();
        }
    }
    int Selectedindex=-1;
    int lastIndex=-2;
    Color[]  ConvertImagePixel(Color col,ref Color[] CA,List<int> position)
    {
        for (int i = 0; i < position.Count; i++)
        {
            CA[position[i]] = col;
        }
        return CA;
    }
    Color[] setSameColor(int length,Color col)
    {
        Color[] CA = new Color[length];
        for (int i = 0; i < length; i++)
        {
            CA[i] = col;
        }
        return CA;
    }
}



