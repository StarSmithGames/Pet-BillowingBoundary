using DG.Tweening;

using Game.Systems.FloatingTextSystem;

using Sirenix.OdinInspector;
using UnityEngine;

using Zenject;

public class ClickerHand : MonoBehaviour
{
	[SerializeField] private Vector3 pointHit;
	[ReadOnly]
	[SerializeField] private Vector3 startPosition;
	[ReadOnly]
	[SerializeField] private Vector3 endPosition;

	private Sequence sequence;
	private ClickerConveyor clickerConveyor;
	private FloatingTextSystem floatingTextSystem;

	[Inject]
	private void Construct(ClickerConveyor clickerConveyor, FloatingTextSystem floatingTextSystem)
	{
		this.clickerConveyor = clickerConveyor;
		this.floatingTextSystem = floatingTextSystem;
	}

	private void Start()
	{
		endPosition = endPosition - (transform.TransformPoint(pointHit) - startPosition);
	}

	public void Punch()
    {
		Kill();

		sequence = DOTween.Sequence();
		sequence
			.Append(transform.DOMove(endPosition, 0.1f))
			.OnComplete(() =>
			{
				clickerConveyor.CurrentClickableObject.Sheet.TapCountBar.CurrentValue -= 1;
				floatingTextSystem.CreateText(clickerConveyor.CurrentClickableObject.GetRandomPoint().position, "+1", type: AnimationType.AdvanceDamage);
			});
	}

	public void Back()
    {
		Kill();

		sequence = DOTween.Sequence();
		sequence
			.Append(transform.DOMove(startPosition, 0.15f));
	}

	public void Kill()
	{
		if (sequence != null)
		{
			if (sequence.IsPlaying())
			{
				sequence.Kill(true);
			}
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if (Application.isPlaying) return;

		startPosition = transform.position;
		Gizmos.color = Color.green;
		if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit))
		{
			endPosition = hit.point;
		}
		else
		{
			Gizmos.color = Color.red;
		}
		Gizmos.DrawRay(transform.position, transform.up * 5);

		Gizmos.DrawSphere(transform.TransformPoint(pointHit), 0.08f);

		Gizmos.DrawSphere(startPosition, 0.05f);
		Gizmos.DrawSphere(endPosition, 0.05f);
	}
#endif
}