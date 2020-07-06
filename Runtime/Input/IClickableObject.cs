using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace crass
{
public abstract class IClickableObject : MonoBehaviour
{
	public string MouseOverText;
	public abstract void Click();
}
}
