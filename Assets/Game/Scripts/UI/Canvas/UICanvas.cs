using System.Linq;
using UnityEngine;

namespace Game.UI
{
	public abstract class UICanvas : MonoBehaviour
	{
		public WindowsRegistrator WindowsRegistrator
		{
			get
			{
				if (windowsRegistrator == null)
				{
					windowsRegistrator = new WindowsRegistrator();
				}

				return windowsRegistrator;
			}
		}
		protected WindowsRegistrator windowsRegistrator;

		public Transform Windows
		{
			get
			{
				if (windows == null)
				{
					windows = transform.Find("Windows");
				}

				return windows;
			}
		}
		[SerializeField] protected Transform windows;
	}

	public class WindowsRegistrator : Registrator<IWindow>
	{
		public bool IsAnyWindowShowing()
		{
			return registers.Any((x) => x.IsShowing);
		}
		public bool IsAllHided()
		{
			return registers.All((x) => !x.IsShowing);
		}

		public void Show<T>() where T : class, IWindow
		{
			GetAs<T>().Show();
		}
		public void Hide<T>() where T : class, IWindow
		{
			GetAs<T>().Hide();
		}

		public void HideAll()
		{
			for (int i = 0; i < registers.Count; i++)
			{
				registers[i].Hide();
			}
		}
	}
}