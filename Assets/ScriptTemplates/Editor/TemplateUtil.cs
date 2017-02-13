using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace ScriptTemplates
{
	static public class TemplateUtil
	{
		static public void CreateTemplateDialog(string Name, Dialogs.InputDialog.Finished Callback)
		{
			Dialogs.InputDialog.Info info = new Dialogs.InputDialog.Info();
			info.title = "Create";
			info.header = "Name:";
			info.text = Name;
			info.buttons = new string[] { "Okay", "Cancel" };
			Dialogs.InputDialog.Create(info, Callback);
		}

		static public void CreateFileFromTemplate(string Name, string TemplateFolder, string TemplateFileName)
		{
			if (Directory.Exists(TemplateFolder) == false)
				return;

			string templatePath = string.Format("{0}/{1}.txt", TemplateFolder, TemplateFileName);
			StreamReader reader = new StreamReader(templatePath);
			string template = reader.ReadToEnd();
			template = template.Replace("#SCRIPTNAME#", Name);
			reader.Close();
			reader.Dispose();

			string filePath = GetValidFilePath(GetSelectionPath(), Name);
			StreamWriter writer = new StreamWriter(filePath);
			writer.Write(template);
			writer.Close();
			writer.Dispose();

			AssetDatabase.Refresh();
		}

		static private string GetSelectionPath()
		{
			Object selected = Selection.activeObject;
			string path = AssetDatabase.GetAssetPath(selected);
			if (path == null || path == "")
				path = "Assets";
			if (path.Contains(".") == true)
				return Path.GetDirectoryName(path);
			return string.Format("{0}/", path);
		}

		static private string GetValidFilePath(string FolderPath, string Name, int Index = 0)
		{
			string path;
			if (Index == 0) { path = string.Format("{0}/{1}.cs", FolderPath, Name); }
			else { path = string.Format("{0}/{1}{2}.cs", FolderPath, Name, Index); }

			if (System.IO.File.Exists(path) == true)
				return GetValidFilePath(FolderPath, Name, Index + 1);
			return path;
		}

		static public T[] AddElement<T>(T[] array)
		{
			T[] old = array;
			array = new T[old.Length + 1];
			for (int i = 0; i < old.Length; ++i)
			{
				array[i] = old[i];
			}
			array[old.Length] = default(T);
			return array;
		}

		static public T[] DeleteElement<T>(T[] array)
		{
			T[] old = array;
			array = new T[old.Length - 1];
			for (int i = 0; i < old.Length - 1; ++i)
			{
				array[i] = old[i];
			}
			return array;
		}

		static public T[] ShiftElement<T>(T[] array, int current, int target)
		{
			if (target < 0 || target >= array.Length)
				return array;

			List<T> templates = new List<T>();// templateInfo.templates;
			templates.AddRange(array);

			T item = templates[current];
			templates.RemoveAt(current);
			templates.Insert(target, item);

			return templates.ToArray();
		}
	}
}