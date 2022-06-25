using UnityEngine;

namespace RhinoGame
{
	/// <summary>
	/// Orientates the gameobject, this script is attached to always face the camera.
	/// </summary>
	public class UIBillboard : MonoBehaviour
	{
		/// <summary>
		/// If enabled, scales this object to always stay at the same size,
		/// regardless of the position in the scene i.e. distance to camera.
		/// </summary>
		public bool scaleWithDistance = false;

		/// <summary>
		/// Multiplier applied to the sdistance scale calculation.
		/// </summary>
		public float scaleMultiplier = 1f;

		// Optimize GetComponent calls:
		// Cache reference to camera transform
		private Transform camTrans;
        
		// Cache reference to this transform
		private Transform trans;

		// Calculate size depending on camera distance
		private float size;

		void Awake ()
		{
			camTrans = Camera.main.transform;
			trans = transform;
		}


		// Face the camera
		void Update ()
		{
			transform.LookAt (trans.position + camTrans.rotation * Vector3.forward,
				camTrans.rotation * Vector3.up);

			if (!scaleWithDistance)
				return;
			size = (camTrans.position - transform.position).magnitude;
			transform.localScale = Vector3.one * (size * (scaleMultiplier / 100f));
		}
	}
}