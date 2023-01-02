using Game.Entities;
using Game.Systems.WaveRoadSystem;
using Zenject;

namespace Game.Managers.StorageManager
{
	public class SaveHandler : IInitializable
	{
		private ISaveLoad saveLoad;
		private SignalBus signalBus;
		private WaveRoad waveRoad;
		private Player player;

		public SaveHandler(ISaveLoad saveLoad,
			SignalBus signalBus,
			WaveRoad waveRoad,
			Player player)
		{
			this.saveLoad = saveLoad;
			this.signalBus = signalBus;
			this.waveRoad = waveRoad;
			this.player = player;
		}

		public void Initialize()
		{
			signalBus?.Subscribe<SignalSave>(OnSaveRequired);
			waveRoad.onChanged += OnSaveRequired;
		}

		private void OnSaveRequired()
		{
			var profile = saveLoad.GetStorage().Profile;

			profile.GetData().waveRoadData = waveRoad.GetData();
			profile.GetData().playerData = player.GetData();

			saveLoad.GetStorage().IsWasHere.SetData(true);

			saveLoad.Save();
		}
	}
}