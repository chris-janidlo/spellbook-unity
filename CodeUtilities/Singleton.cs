using System;
using UnityEngine;

namespace crass
{
public class Singleton<T> : MonoBehaviour
{
	private static bool _allowReset = false;

	static T _instance;
	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				throw new Exception("Singleton not initialized; you must set it before trying to get it.");
			}
			return _instance;
		}
		private set
		{
			if (!_allowReset && _instance != null)
			{
				throw new Exception("Singleton already initialized; you cannot set it twice.");
			}
			_instance = value;
		}
	}

	protected void SingletonSetInstance (T instance, bool allowReset)
	{
		Instance = instance;
		_allowReset = allowReset;
	}

	protected T SingletonGetInstance ()
	{
		return _instance;
	}
}
}