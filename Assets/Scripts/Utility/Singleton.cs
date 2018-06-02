using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Generic Singleton class.
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour
    {
        #region Private Variables
        static Singleton<T> instance;

        [Tooltip("Defines what happens with duplicate classes, when there's already one instance of this Singleton. Either destroy the script alone or the gameObject.")]
        [SerializeField] bool destroyGameObject = false;
        #endregion



        #region Public Properties
        public static Singleton<T> Instance
		{
			get
			{
				if (instance == null) 
				{
					Debug.LogError ("Singleton not registered! Make sure the GameObject running your singleton is active in your scene.");
					return null;
				}
				return instance;
			}
		}
		#endregion



		#region Unity Event Functions
		/// <summary>
		/// Registers the singleton instance.
		/// </summary>
		private void Awake ()
		{
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
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
        /// <summary>
        /// Awake function for all inheriting classes.
        /// </summary>
        protected virtual void InheritedAwake() { }
		#endregion
	}
}