using Game.Managers.StorageManager;
using Game.UI;
using Zenject;

namespace Game.Systems.SettingsSystem
{
	public class UIMusicButton : UIButtonToggle
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

			Enable(saveLoad.GetStorage().IsMusic.GetData());
		}

		protected override void OnClick()
		{
			base.OnClick();

			saveLoad.GetStorage().IsMusic.SetData(IsEnable);
		}
	}
}