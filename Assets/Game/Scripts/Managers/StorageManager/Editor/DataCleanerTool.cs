using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;
using UnityEngine;

namespace Game.Managers.StorageManager.Editor
{
	public class DataCleanerTool : EditorWindow
	{
		private static DataCleanerTool window;

		[MenuItem("Tools/Data Cleaner", priority = 0)]
		public static void ManageData()
		{
			//Window
			window = GetWindow<DataCleanerTool>(title: "Data Cleaner", focus: true, utility: true);
			window.maxSize = new Vector2(250, 120);
			window.minSize = new Vector2(250, 120);
			window.ShowUtility();
		}

		private void OnGUI()
		{
			GUILayout.BeginVertical();

			var files = Directory.GetFiles(Application.persistentDataPath).Where((x) => !x.EndsWith(".log") && !x.EndsWith("log.txt")).ToArray();
			var directories = Directory.GetDirectories(Application.persistentDataPath);

			GUI.enabled = files.Length != 0 || directories.Length != 0;

			if (GUILayout.Button("Очистить AppData"))
			{
				foreach (var directory in directories)
				{
					new DirectoryInfo(directory).Delete(true);
				}

				foreach (string filePath in files)
				{
					File.Delete(filePath);
				}

				EditorGUI.FocusTextInControl(null);
			}

			if (GUILayout.Button("Очистить PersistentDataPath"))
			{
				JsonSerializator.ClearPersistentPath();

				EditorGUI.FocusTextInControl(null);
			}

			GUI.enabled = true;

			if (GUILayout.Button("Очистить PlayerPrefs"))
			{
				PlayerPrefs.DeleteAll();
				PlayerPrefs.Save();

				EditorGUI.FocusTextInControl(null);
			}

			GUILayout.EndVertical();

			GUILayout.FlexibleSpace();

			var saveKeys = AssetDatabaseExtensions.LoadAsset<StorageManagerInstaller>().playerPrefsSettings.GetKeyList();

			bool anyHas = false;

			for (int i = 0; i < saveKeys.Count; i++)
			{
				if (PlayerPrefs.HasKey(saveKeys[i]))
				{
					anyHas = true;
					break;
				}
			}

			GUI.enabled = anyHas;

			if (GUILayout.Button("PlayerPrefs Save"))
			{
				Dictionary<string, string> texts = new();

				for (int i = 0; i < saveKeys.Count; i++)
				{
					if (PlayerPrefs.HasKey(saveKeys[i]))
					{
						texts.Add(saveKeys[i], PlayerPrefs.GetString(saveKeys[i]));
					}
				}
				
				var jsonWindow = GetWindow<JsonText>(title: "Json");
				jsonWindow.minSize = new Vector2(400, 700);
				jsonWindow.texts = texts;
			}
		}
	}

	public class JsonText : EditorWindow
	{
		public Dictionary<string, string> texts;
		public Vector2 scroll = new Vector2(0, 0);

		private void OnGUI()
		{
			scroll = EditorGUILayout.BeginScrollView(scroll, true, true);
			foreach (var item in texts)
			{
				EditorGUILayout.LabelField(item.Key);
				EditorGUILayout.TextArea(item.Value);
			}
			EditorGUILayout.EndScrollView();
		}
	}
}