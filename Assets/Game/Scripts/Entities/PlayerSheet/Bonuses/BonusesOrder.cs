using Game.Entities;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Zenject;

public class BonusesOrder : MonoBehaviour
{
    [SerializeField] private List<Bonus> bonuses = new List<Bonus>();

	private Player player;

	[Inject]
	public void Construct(Player player)
	{
		this.player = player;
	}

	private void Start()
	{
		for (int i = 0; i < bonuses.Count; i++)
		{
			player.BonusRegistrator.Registrate(bonuses[i]);
		}
	}

	private void OnDestroy()
	{
		if (player == null) return;

		for (int i = 0; i < bonuses.Count; i++)
		{
			player.BonusRegistrator.UnRegistrate(bonuses[i]);
		}
	}

	[Button(DirtyOnClick = true)]
    private void Fill()
    {
        bonuses = GetComponentsInChildren<Bonus>().ToList();
	}
}