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

			GUI.enabled = true;

			if (GUILayout.Button("Очистить PlayerPrefs"))
			{
				PlayerPrefs.DeleteAll();
				PlayerPrefs.Save();

				EditorGUI.FocusTextInControl(null);
			}

			GUILayout.EndVertical();

			GUILayout.FlexibleSpace();

			string saveKey = AssetDatabaseExtensions.LoadAsset<StorageManagerInstaller>().playerPrefsSettings.preferenceName;

			GUI.enabled = PlayerPrefs.HasKey(saveKey);

			if (GUILayout.Button("PlayerPrefs Save"))
			{
				string json = PlayerPrefs.GetString(saveKey);

				var jsonWindow = GetWindow<JsonText>(title: "Json");
				jsonWindow.minSize = new Vector2(400, 700);
				jsonWindow.text = json;
			}
		}
	}

	public class JsonText : EditorWindow
	{
		public string text;
		public Vector2 scroll = new Vector2(0, 0);

		private void OnGUI()
		{
			scroll = EditorGUILayout.BeginScrollView(scroll, true, true);
			EditorGUILayout.TextArea(text);
			EditorGUILayout.EndScrollView();
		}
	}
}