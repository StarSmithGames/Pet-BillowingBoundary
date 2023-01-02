using Game.Managers.AudioManager;
using Game.Managers.StorageManager;
using Game.UI;
using Zenject;

namespace Game.Systems.SettingsSystem
{
	public class UIMusicButton : UIButtonToggle
	{
		private SignalBus signalBus;
		private ISaveLoad saveLoad;
		private AnalyticsSystem.AnalyticsSystem analyticsSystem;

		[Inject]
		private void Construct(SignalBus signalBus, ISaveLoad saveLoad, AnalyticsSystem.AnalyticsSystem analyticsSystem)
		{
			this.signalBus = signalBus;
			this.saveLoad = saveLoad;
			this.analyticsSystem = analyticsSystem;
		}

		protected override void Start()
		{
			base.Start();

			Enable(saveLoad.GetStorage().IsMusic.GetData());
		}

		protected override void OnClick()
		{
			Enable(!IsEnable);
			saveLoad.GetStorage().IsMusic.SetData(IsEnable);

			base.OnClick();

			signalBus?.Fire(new SignalMusicChanged());

			analyticsSystem.LogEvent_settings_music();
		}
	}
}