using UnityEditor;
using UnityEngine;

namespace ScriptTemplates
{
	[CustomEditor(typeof(TemplateEditor))]
	public class TemplateEditorInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			TemplateEditor templateEditor = target as TemplateEditor;
			Undo.RecordObject(templateEditor, "TemplateEditor");
			OnInspectorGUI_TemplateEditor(templateEditor);
		}

		private void OnInspectorGUI_TemplateEditor(TemplateEditor templateEditor)
		{
			EditorGUILayout.BeginVertical();

			string info = "--► Information about the Editor goes here. ◄--";
			EditorGUILayout.LabelField(info);

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Add Category", EditorStyles.miniButtonLeft))
				templateEditor.categories = TemplateUtil.AddElement(templateEditor.categories);
			if (GUILayout.Button("Remove Category", EditorStyles.miniButtonRight))
				templateEditor.categories = TemplateUtil.DeleteElement(templateEditor.categories);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Priority", EditorStyles.boldLabel);
			EditorGUILayout.LabelField("Category", EditorStyles.boldLabel);
			EditorGUILayout.EndHorizontal();

			foreach (TemplateCategory templateInfo in templateEditor.categories)
			{
				OnInspectorGUI_TemplateInfo(templateInfo);
			}

			EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);

			if (GUILayout.Button("Generate Context Menus"))
				templateEditor.GenerateContextMenus();

			EditorGUILayout.EndVertical();
		}

		private void OnInspectorGUI_TemplateInfo(TemplateCategory templateInfo)
		{
			if (templateInfo == null)
				return;

			EditorGUILayout.BeginVertical();
			templateInfo.debug_IsFoldout = EditorGUILayout.Foldout(templateInfo.debug_IsFoldout, templateInfo.name);

			if (templateInfo.debug_IsFoldout)
			{
				Rect controlRect = EditorGUILayout.GetControlRect(false);
				Rect priorityRect = new Rect(controlRect.x + 10, controlRect.y, 30, controlRect.height);
				templateInfo.priority = EditorGUI.IntField(priorityRect, templateInfo.priority);

				Rect categoryRect = new Rect(controlRect.x + 45, controlRect.y, controlRect.width - 45, controlRect.height);
				templateInfo.name = EditorGUI.TextField(categoryRect, templateInfo.name);

				EditorGUI.indentLevel++;
				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Templates:");
				if (GUILayout.Button("ADD", EditorStyles.miniButton))
					templateInfo.templates = TemplateUtil.AddElement(templateInfo.templates);
				EditorGUILayout.EndHorizontal();

				TextAsset[] templates = templateInfo.templates;
				for (int i = 0; i < templates.Length; ++i)
				{
					EditorGUILayout.BeginHorizontal();
					templates[i] = EditorGUILayout.ObjectField(templates[i], typeof(TextAsset), false) as TextAsset;
					if (GUILayout.Button("▲", EditorStyles.miniButtonLeft))
						templateInfo.templates = TemplateUtil.ShiftElement(templateInfo.templates, i, i-1);
					if (GUILayout.Button("▼", EditorStyles.miniButtonMid))
						templateInfo.templates = TemplateUtil.ShiftElement(templateInfo.templates, i, i+1);
					if (GUILayout.Button("DEL", EditorStyles.miniButtonRight))
						templateInfo.templates = TemplateUtil.DeleteElement(templateInfo.templates);
					EditorGUILayout.EndHorizontal();
				}
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.EndVertical();
		}
	}
}