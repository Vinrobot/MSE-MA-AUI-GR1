using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelector : MonoBehaviour
{
	[Serializable]
	public class OptionData
	{
		[SerializeField]
		public string text;

		[SerializeField]
		public Sprite image;

		[SerializeField]
		public Color color;
	}

	private Dropdown dropdown;

    public List<OptionData> colors = new List<OptionData>();

	public OptionData value => colors[dropdown.value];
	public Color color => colors[dropdown.value].color;

	void Start()
    {
        dropdown = GetComponent<Dropdown>();
		UpdateOptions();
	}

	public void UpdateOptions()
	{
		dropdown.options = colors.Select((e) => new Dropdown.OptionData()
		{
			image = e.image,
			text = e.text,
		}).ToList();
		dropdown.RefreshShownValue();
	}
}
