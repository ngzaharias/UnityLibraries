using UnityEditor;
using UnityEngine;
using System.IO;

namespace Templates
{
	public class ContextMenu
	{
		[MenuItem("Assets/Create/C# Script/MonoBehaviour", false, 80)]
		static private void MonoBehaviour()
		{
			CopyTemplate("Assets/Templates/ScriptTemplates", "NewMonoBehaviour.cs");
		}

		static private void CopyTemplate(string Folder, string File)
		{
			if (Directory.Exists(Folder) == false)
				return;

			StreamReader reader = new StreamReader(string.Format("{0}/{1}.txt", Folder, File));
			string template = "";// reader.ReadToEnd();
			reader.Close();
			reader.Dispose();

			string filePath = string.Format("{0}/{1}", SelectionPath(), File);
			StreamWriter writer = new StreamWriter(filePath);
			writer.Write(template);
			writer.Close();
			writer.Dispose();

			Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(filePath);
			//UnityEditorInternal.InternalEditorUtility.

			EditorUtility.RevealInFinder("");

			AssetDatabase.Refresh();
		}

		static private string SelectionPath()
		{
			Object selected = Selection.activeObject;
			string path = AssetDatabase.GetAssetPath(selected);
			if (path != null && path != "")
				return Path.GetDirectoryName(path);
			return "Assets";
		}
	}
}