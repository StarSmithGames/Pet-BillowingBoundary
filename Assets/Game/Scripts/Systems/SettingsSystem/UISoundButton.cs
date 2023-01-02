using Game.Managers.StorageManager;
using Game.Systems.AnalyticsSystem;
using Game.UI;
using Zenject;

namespace Game.Systems.SettingsSystem
{
	public class UISoundButton : UIButtonToggle
	{
		private ISaveLoad saveLoad;
		private AnalyticsSystem.AnalyticsSystem analyticsSystem;

		[Inject]
		private void Construct(ISaveLoad saveLoad, AnalyticsSystem.AnalyticsSystem analyticsSystem)
		{
			this.saveLoad = saveLoad;
			this.analyticsSystem = analyticsSystem;
		}

		protected override void Start()
		{
			base.Start();

			Enable(saveLoad.GetStorage().IsSound.GetData());
		}

		protected override void OnClick()
		{
			Enable(!IsEnable);
			saveLoad.GetStorage().IsSound.SetData(IsEnable);
			base.OnClick();

			analyticsSystem.LogEvent_settings_sound();
		}
	}
}