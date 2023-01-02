using Game.Managers.StorageManager;
using Game.UI;
using Zenject;

namespace Game.Systems.SettingsSystem
{
	public class UIVibrationButton : UIButtonToggle
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

			Enable(saveLoad.GetStorage().IsVibration.GetData());
		}

		protected override void OnClick()
		{
			Enable(!IsEnable);
			saveLoad.GetStorage().IsVibration.SetData(IsEnable);

			base.OnClick();

			analyticsSystem.LogEvent_settings_vibration();
		}
	}
}