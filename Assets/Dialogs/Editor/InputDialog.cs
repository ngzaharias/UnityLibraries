using UnityEditor;
using UnityEngine;

namespace Dialogs
{
	public class InputDialog : EditorWindow
	{
		public delegate void Finished(int button, string text);
		private event Finished OnFinished;

		public struct Info
		{
			public string title;
			public string header;
			public string text;
			public string[] buttons;
		};
		private Info info;

		static public void Create(Info Info, Finished Callback)
		{
			InputDialog dialog = ScriptableObject.CreateInstance<InputDialog>();
			dialog.info.header = Info.header;
			dialog.info.text = Info.text;
			dialog.info.buttons = Info.buttons;
			dialog.OnFinished += Callback;

			dialog.titleContent = new GUIContent(Info.title);
			dialog.ShowUtility();

			Vector2 size = new Vector2(dialog.position.width, dialog.position.height);
			Vector2 position = new Vector2(Screen.width / 2, Screen.width / 2);
			dialog.position = new Rect(position + size, size);
			dialog.minSize = new Vector2(200, 100);
		}

		private void OnGUI()
		{
			EditorGUILayout.BeginVertical();
			GUILayout.Space(10);
			OnGUI_Header();
			GUILayout.Space(10);
			OnGUI_Buttons();
			EditorGUILayout.EndVertical();
		}

		private void OnGUI_Header()
		{
			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField(info.header);
			info.text = EditorGUILayout.TextField(info.text);
			EditorGUILayout.EndVertical();
		}

		private void OnGUI_Buttons()
		{
			if (info.buttons == null)
				return;

			EditorGUILayout.BeginHorizontal();
			for (int i = 0; i < info.buttons.Length; ++i)
			{
				if (GUILayout.Button(info.buttons[i]))
				{
					OnDialogClosed(i, info.text);
					this.Close();
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		private void OnDialogClosed(int index, string text)
		{
			Debug.LogFormat("OnDialogClosed: {0} | {1}", index, text);
			if (OnFinished != null)
			{
				OnFinished(index, text);
			}
		}

		private void OnLostFocus()
		{
			Debug.Log("OnLostFocus");
			this.Close();
		}

		private void OnDestroy()
		{
			Debug.Log("OnDestroy");
			OnDialogClosed(-1, null);
		}

		// DEBUGGING
		//[MenuItem("Testing/Dialog/InputDialog")]
		static private void Test()
		{
			Info info = new Info();
			info.title = "Test Title";
			info.header = "Test Header";
			info.text = "Test Text";
			info.buttons = new string[] { "Test Button 0", "Test Button 1" };

			Create(info, null);
		}
	}
}