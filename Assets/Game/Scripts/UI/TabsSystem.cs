using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
	public class TabsSystem : MonoBehaviour
	{
		[SerializeField] private int initIndex = 0;
		[SerializeField] private TabBehavior tabBehavior = TabBehavior.Index;
		[SerializeField] private List<Tab> tabs = new List<Tab>();
		[ShowIf("tabBehavior", TabBehavior.Next)]
		[SerializeField] private Button next;

		private void Start()
		{
			next?.onClick.AddListener(Next);

			for (int i = 0; i < tabs.Count; i++)
			{
				tabs[i].Init();
				if(tabBehavior == TabBehavior.Index)
				{
					tabs[i].onClicked += OnTabClicked;
				}
			}

			Open(initIndex);
		}

		private void OnDestroy()
		{
			for (int i = 0; i < tabs.Count; i++)
			{
				tabs[i].onClicked -= OnTabClicked;
			}

			next?.onClick.RemoveAllListeners();
		}

		public void Open(int index)
		{
			for (int i = 0; i < tabs.Count; i++)
			{
				tabs[i].Enable(false);
			}

			tabs[index].Enable(true);
		}


		private void OnTabClicked(Tab tab)
		{
			if(tabBehavior == TabBehavior.Index)
			{
				Open(tabs.IndexOf(tab));
			}
			else
			{
				Next();
			}
		}

		[Button(DirtyOnClick = true)]
		private void Next()
		{
			initIndex = (initIndex + 1) % tabs.Count;

			Open(initIndex);
		}
	}

	[System.Serializable]
	public class Tab
	{
		public UnityAction<Tab> onClicked;

		public GameObject tab;
		public bool isHasButton = true;
		[ShowIf("isHasButton")]
		public Button tabButton;
		[ShowIf("isHasButton")]
		public Color tabDisableColor;

		public void Init()
		{
			if (isHasButton)
			{
				tabButton.onClick.AddListener(OnClicked);
			}
		}

		public void Enable(bool trigger)
		{
			tab.SetActive(trigger);
			if (isHasButton)
			{
				tabButton.image.color = trigger ? Color.white : tabDisableColor;
			}

			if (trigger)
			{
				tab.transform.SetAsLastSibling();
				tabButton?.transform.SetAsLastSibling();
			}
		}

		private void OnClicked()
		{
			onClicked?.Invoke(this);
		}
	}

	public enum TabBehavior
	{
		Index,
		Next,
	}
}