using Game.Entities;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Zenject;

public class PlayerSheetOrder : MonoBehaviour
{
    [SerializeField] private List<Bonus> bonuses = new List<Bonus>();
    [SerializeField] private List<ActiveSkill> skills = new List<ActiveSkill>();

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

		for (int i = 0; i < skills.Count; i++)
		{
			player.SkillRegistrator.Registrate(skills[i]);
		}
		player.SkillRegistrator.SelectSkill(0);
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
		skills = GetComponentsInChildren<ActiveSkill>().ToList();
	}
}