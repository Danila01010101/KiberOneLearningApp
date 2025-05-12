using System;
using UnityEngine;

namespace KiberOneLearningApp
{
	public class ColliderChanger : MonoBehaviour
	{
		[SerializeField] private GameObject roundCollider;
		[SerializeField] private GameObject squadCollider;

		public void ChangeCollider()
		{
			if (roundCollider.activeSelf == false)
			{
				roundCollider.SetActive(true);
				squadCollider.SetActive(false);
			}
			else
			{
				roundCollider.SetActive(false);
				squadCollider.SetActive(true);
			}
		}
	}
}