using Game.Managers.StorageManager;
using Game.UI;
using Zenject;

namespace Game.Systems.SettingsSystem
{
	public class UIVibrationButton : UIButtonToggle
	{
		private ISaveLoad saveLoad;

		[Inject]
		private void Construct(ISaveLoad saveLoad)
		{
			this.saveLoad = saveLoad;
		}

		protected override void Start()
		{
			base.Start();

			Enable(saveLoad.GetStorage().IsVibration.GetData());
		}

		protected override void OnClick()
		{
			base.OnClick();

			saveLoad.GetStorage().IsVibration.SetData(IsEnable);
		}
	}
}