using Game.Managers.StorageManager;

using MoreMountains.NiceVibrations;

namespace Game.Managers.VibrationManager
{
    public class VibrationManager
    {
        private ISaveLoad saveLoad;


		public VibrationManager(ISaveLoad saveLoad)
        {
            this.saveLoad = saveLoad;

		}

		public void Vibrate()
        {
            Vibrate(HapticTypes.HeavyImpact);
		}

        public void Vibrate(HapticTypes haptic)
        {
            if (!saveLoad.GetStorage().IsVibration.GetData()) return;

			MMVibrationManager.Haptic(haptic);
		}
	}
}