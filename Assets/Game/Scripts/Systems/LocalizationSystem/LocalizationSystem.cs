using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;
using System.Linq;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using Game.Managers.AsyncManager;
using Game.Managers.StorageManager;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

namespace Game.Systems.LocalizationSystem
{
	public partial class LocalizationSystem : IDisposable
	{
		public bool IsLocaleProcess => localeCoroutine != null;
		private Coroutine localeCoroutine;

		private SignalBus signalBus;
		private AsyncManager asyncManager;
		private ISaveLoad saveLoad;

		public LocalizationSystem(SignalBus signalBus, AsyncManager asyncManager, ISaveLoad saveLoad)
		{
			this.signalBus = signalBus;
			this.asyncManager = asyncManager;
			this.saveLoad = saveLoad;

			LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;

			if (saveLoad.GetStorage().IsFirstTime.GetData() == false)
			{
				ChangeLocale(saveLoad.GetStorage().LanguageIndex.GetData());
			}
		}

		public void Dispose()
		{
			LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
		}

		public string Translate(string id)
		{
			return LocalizationSettings.StringDatabase.GetLocalizedString(GetTableById(id), id);
		}

		public void TranslateAsync(string id, UnityAction<string> callback)
		{
			AsyncOperationHandle<string> load = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(GetTableById(id), id);

			if (load.IsDone)
			{
				callback?.Invoke(load.Result);
			}
			else
			{
				load.Completed += (o) => callback?.Invoke(o.Result);
			}
		}


		public void ChangeLocale(int local)
		{
			if (!IsLocaleProcess)
			{
				localeCoroutine = asyncManager.StartCoroutine(SetLocale(local));
			}
		}

		private IEnumerator SetLocale(int locale)
		{
			yield return LocalizationSettings.InitializationOperation;
			LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[locale];

			localeCoroutine = null;
		}


		private static string GetTableById(string id)
		{
			if (id.StartsWith("ui"))
			{
				return "UI";
			}

			return "";
		}

		private void OnLocaleChanged(Locale locale)
		{
			signalBus?.Fire(new SignalLocalizationChanged());

			saveLoad.GetStorage().LanguageIndex.SetData(CurrentLocaleIndex);
		}
	}

	//Static
	public partial class LocalizationSystem
	{
#if UNITY_EDITOR
		public static string CurrentLocaleStatic => LocalizationEditorSettings.GetLocales().First().name;

		public static string TranslateStatic(string id, string language)
		{
			var tableCollection = LocalizationEditorSettings.GetStringTableCollection(GetTableById(id));
			Locale locale = LocalizationEditorSettings.GetLocale(LocalizationEditorSettings.GetLocales().First((x) => x.name == language).Identifier);
			var table = (StringTable)tableCollection.GetTable(locale.Identifier);

			return table.GetEntry(id).LocalizedValue;
		}

		public static string[] GetLocales()
		{
			return LocalizationEditorSettings.GetLocales().Select((x) => x.name).ToArray();
		}
#endif
	}

	//Also
	public partial class LocalizationSystem
	{
		public string CurrentLocale => LocalizationSettings.SelectedLocale.name;
		public string CurrentLocaleCode => LocalizationSettings.SelectedLocale.Identifier.Code;
		public int CurrentLocaleIndex => LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);


		public string[] GetAllLanguageNames()
		{
			return LocalizationSettings.AvailableLocales.Locales.Select((x) => x.name).ToArray();
		}

		public string[] GetAllLanguageNativeNames()
		{
			return LocalizationSettings.AvailableLocales.Locales.Select((x) => x.Identifier.CultureInfo.NativeName).ToArray();
		}
	}
}