using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.IO;

namespace ScriptTemplates
{
	[System.Serializable]
	public class TemplateCategory
	{
		public string name = "Category";
		public int priority = 81;
		public TextAsset[] templates = new TextAsset[1];

		public bool debug_IsFoldout = false;
	}

	public class TemplateEditor : ScriptableObject
	{
		[MenuItem("Window/Templates/Template Editor")]
		static private void CreateTemplateEditor()
		{
			string filePath = "Assets/ScriptTemplates/Editor/TemplateEditor.asset";
			TemplateEditor asset = AssetDatabase.LoadAssetAtPath<TemplateEditor>(filePath);
			if (asset == null)
			{
				asset = ScriptableObject.CreateInstance<TemplateEditor>();
				AssetDatabase.CreateAsset(asset, filePath);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}

			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;
		}

		public TemplateCategory[] categories = new TemplateCategory[1];

		public void GenerateContextMenus()
		{
			// load the template
			StreamReader reader = new StreamReader("Assets/ScriptTemplates/Editor/Templates/TemplateContextMenus.cs.txt");
			string template = reader.ReadToEnd();
			reader.Close();
			reader.Dispose();

			// insert context menus
			string contextMenus = "";
			foreach (TemplateCategory category in categories)
			{
				foreach (TextAsset asset in category.templates)
				{
					string categoryTemplate =
@"		[MenuItem(""Assets/Create/%CATEGORY%/%TEMPLATE%"", false, %PRIORITY%)]
		static private void %TEMPLATE%()
		{
			TemplateUtil.CreateTemplateDialog(""%TEMPLATE%"", %TEMPLATE%_Callback);
		}

		static private void %TEMPLATE%_Callback(int button, string Name)
		{
			if (button == 0)
			{
				TemplateUtil.CreateFileFromTemplate(Name, ""%TEMPLATE_FOLDER%"", ""%TEMPLATE_FILENAME%"");
			}
		}";
					string templateFolder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(asset));
					string templateNameWithoutExtension = Path.GetFileNameWithoutExtension(asset.name);
					categoryTemplate = categoryTemplate.Replace("%CATEGORY%", category.name);
					categoryTemplate = categoryTemplate.Replace("%TEMPLATE%", templateNameWithoutExtension);
					categoryTemplate = categoryTemplate.Replace("%PRIORITY%", category.priority.ToString());
					categoryTemplate = categoryTemplate.Replace("%TEMPLATE_FOLDER%", templateFolder);
					categoryTemplate = categoryTemplate.Replace("%TEMPLATE_FILENAME%", asset.name);
					contextMenus += categoryTemplate;
				}

				contextMenus += "";
			}

			template = template.Replace("%BODY%", contextMenus);

			// write the script
			string filePath = "Assets/ScriptTemplates/Editor/TemplateContextMenus.cs";
			System.IO.File.Delete(filePath);
			StreamWriter writer = new StreamWriter(filePath);
			writer.Write(template);
			writer.Close();
			writer.Dispose();
			AssetDatabase.Refresh();
		}
	}
}