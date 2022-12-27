using Game.Managers.VibrationManager;
using Game.Systems.LocalizationSystem;
using Game.UI;

using Zenject;

namespace Game.Systems.SettingsSystem
{
	public class UILanguageButton : UIButton
	{
		private UISubCanvas subCanvas;

		[Inject]
		private void Construct(UISubCanvas subCanvas)
		{
			this.subCanvas = subCanvas;
		}

		protected override void OnClick()
		{
			subCanvas.WindowsRegistrator.Hide<SettingsWindow>();
			subCanvas.WindowsRegistrator.Show<LanguageWindow>();

			base.OnClick();
		}
	}
}