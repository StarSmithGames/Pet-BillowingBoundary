using Game.Entities;
using Game.Managers.ClickManager;

using Sirenix.OdinInspector;

using Unity.VisualScripting;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

using Zenject;

public class ClickerConveyor : MonoBehaviour
{
	public ClickableObject CurrentClickableObject { get => currentClickableObject; }

	[HideLabel]
	[OnValueChanged("Refresh", true)]
	[SerializeField] private ConveyorSettings settings;
	[Space]
	[SerializeField] private Transform clickableContent;
	[Space]
	[SerializeField] private ClickerHand clickerLeftHand;
	[SerializeField] private ClickerHand clickerRightHand;

	[ReadOnly]
	[SerializeField] private ClickableObject currentClickableObject;


	private SignalBus signalBus;
	private Player player;

	[Inject]
	private void Construct(SignalBus signalBus, Player player)
	{
		this.signalBus = signalBus;
		this.player = player;
	}

	private void Start()
	{
		signalBus?.Subscribe<SignalTouchChanged>(OnTouchChanged);
	}

	private void OnDestroy()
	{
		signalBus?.Unsubscribe<SignalTouchChanged>(OnTouchChanged);
	}

	private void OnTouchChanged(SignalTouchChanged signal)
	{
		Touch touch = signal.touch;

		if (player.TapCount.CurrentValue % 2 == 0)
		{
			if (touch.phase == TouchPhase.Began)
			{
				clickerLeftHand.Punch();
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				clickerLeftHand.Back();
				clickerRightHand.Back();
			}
		}
		else
		{
			if (touch.phase == TouchPhase.Began)
			{
				clickerRightHand.Punch();
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				clickerLeftHand.Back();
				clickerRightHand.Back();
			}
		}
	}

	[Button(DirtyOnClick = true)]
	private void Refresh()
	{
		if (settings.startEnemy == null || clickableContent == null) return;

		clickableContent.DestroyChildren(true);
		var obj = PrefabUtility.InstantiatePrefab(settings.startEnemy.prefab);
		var clickable = obj.GetComponent<ClickableObject>();
		clickable.transform.SetParent(clickableContent);
		clickable.transform.localScale = Vector3.one;
		currentClickableObject = clickable;
	}
}

[System.Serializable]
public class ConveyorSettings
{
	public EnemyData startEnemy;
}