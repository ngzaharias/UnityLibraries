using UnityEditor;
using UnityEngine;
using System.IO;

namespace ScriptTemplates
{
	public class TemplateContextMenus
	{
		[MenuItem("Assets/Create/C# Script/NewMonoBehaviour", false, 81)]
		static private void NewMonoBehaviour()
		{
			TemplateUtil.CreateTemplateDialog("NewMonoBehaviour", NewMonoBehaviour_Callback);
		}

		static private void NewMonoBehaviour_Callback(int button, string Name)
		{
			if (button == 0)
			{
				TemplateUtil.CreateFileFromTemplate(Name, "Assets/ScriptTemplates/Templates/CSharp", "NewMonoBehaviour.cs");
			}
		}		[MenuItem("Assets/Create/C# Script/NewSingletonBehaviour", false, 81)]
		static private void NewSingletonBehaviour()
		{
			TemplateUtil.CreateTemplateDialog("NewSingletonBehaviour", NewSingletonBehaviour_Callback);
		}

		static private void NewSingletonBehaviour_Callback(int button, string Name)
		{
			if (button == 0)
			{
				TemplateUtil.CreateFileFromTemplate(Name, "Assets/ScriptTemplates/Templates/CSharp", "NewSingletonBehaviour.cs");
			}
		}		[MenuItem("Assets/Create/C# Script/NewStateMachineBehaviour", false, 81)]
		static private void NewStateMachineBehaviour()
		{
			TemplateUtil.CreateTemplateDialog("NewStateMachineBehaviour", NewStateMachineBehaviour_Callback);
		}

		static private void NewStateMachineBehaviour_Callback(int button, string Name)
		{
			if (button == 0)
			{
				TemplateUtil.CreateFileFromTemplate(Name, "Assets/ScriptTemplates/Templates/CSharp", "NewStateMachineBehaviour.cs");
			}
		}
	}
}