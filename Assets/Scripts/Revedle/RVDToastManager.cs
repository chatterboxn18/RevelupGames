using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Revedle
{
	public enum RVDToast
	{
		CopyToClipboard =0,
		WrongWord = 1,
		ChangeTheme =2
	}
	public class RVDToastManager : ToastManager<RVDToast>
	{

	}
}