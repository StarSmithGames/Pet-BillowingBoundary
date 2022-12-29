using Game.Entities;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

using Zenject;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Managers.ClickManager
{
	public class СonveyorInstaller : MonoInstaller
	{
		[HideLabel]
		public Conveyor conveyor;

		public override void InstallBindings()
		{
			Container.BindInstance(conveyor);
		}

#if UNITY_EDITOR
		[Button(DirtyOnClick = true)]
		private void Refresh()
		{
			conveyor.objects.Clear();
			transform.DestroyChildren(true);

			for (int i = 0; i < conveyor.datas.Count; i++)
			{
				var clickable = Create(conveyor.datas[i].prefab);
				clickable.transform.localPosition = new Vector3(conveyor.spacing, 0, 0) * i;
				conveyor.objects.Add(clickable);
			}
			ClickableObject Create(ClickableObject prefab)
			{
				var obj = PrefabUtility.InstantiatePrefab(prefab);
				var clickable = obj.GetComponent<ClickableObject>();
				clickable.transform.SetParent(transform);
				clickable.transform.localScale = Vector3.one;
				return clickable;
			}
		}
#endif
	}

	[System.Serializable]
	public class Conveyor
	{
		[AssetSelector]
		public List<TargetData> datas = new List<TargetData>();
		public float spacing = 5f;
		[ReadOnly]
		public List<ClickableObject> objects = new List<ClickableObject>();
	}
}