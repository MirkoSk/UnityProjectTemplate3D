using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Generic Singleton class.
    /// </summary>
    public abstract class Singleton<T> : SubscribedBehaviour
    {
        #region Private Variables
        static T instance;

        [Header("Singleton")]
        [Tooltip("Defines what happens with duplicate classes, when there's already one instance of this Singleton. Either destroy the script alone or the gameObject.")]
        [SerializeField] bool destroyGameObject = false;
        #endregion



        #region Public Properties
        public static T Instance
		{
			get
			{
				if (instance == null) 
				{
					Debug.LogError ("Singleton not registered! Make sure the GameObject running your singleton is active in your scene.");
					return default(T);
				}
				return instance;
			}
		}
		#endregion



		#region Public Functions
		/// <summary>
		/// Registers the singleton instance.
		/// </summary>
		protected void RegisterSingleton (T newInstance)
		{
            if (instance == null)
            {
                instance = newInstance;
            }
            else if (!instance.Equals(newInstance))
            {
                if (!destroyGameObject)
                {
                    Destroy(this);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
		}
		#endregion
	}
}