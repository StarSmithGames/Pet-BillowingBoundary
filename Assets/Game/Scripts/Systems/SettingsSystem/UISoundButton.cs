using Game.Managers.StorageManager;
using Game.UI;
using Zenject;

namespace Game.Systems.SettingsSystem
{
	public class UISoundButton : UIButtonToggle
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

			Enable(saveLoad.GetStorage().IsSound.GetData());
		}

		protected override void OnClick()
		{
			base.OnClick();

			saveLoad.GetStorage().IsSound.SetData(IsEnable);
		}
	}
}