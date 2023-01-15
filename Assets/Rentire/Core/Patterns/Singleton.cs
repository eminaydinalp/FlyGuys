// Copyright Gamelogic (c) http://www.gamelogic.co.za

using MEC;
using UnityEngine;

namespace Rentire.Core
{
    
    /// <summary>
    /// Generic Implementation of a Singleton MonoBehaviour.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    using HelpURLAttribute = UnityEngine.HelpURLAttribute;
    
	[AddComponentMenu("Rentire/Extensions/Singleton")]
	public class Singleton<T> : RMonoBehaviour where T : MonoBehaviour
	{
		#region  Properties

		/// <summary>
		/// Returns the instance of this singleton.
		/// </summary>
		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = (T)FindObjectOfType(typeof(T));

					if (instance == null)
					{
						//Log.Warning("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
					}
				}

				return instance;
			}
		}

		#endregion

		private static T instance;

	}
}
