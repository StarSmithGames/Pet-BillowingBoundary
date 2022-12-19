using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.FloatingTextSystem
{
	public class FloatingPoint : MonoBehaviour
	{
#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Gizmos.DrawSphere(transform.position, 0.08f);	
		}
#endif
	}
}