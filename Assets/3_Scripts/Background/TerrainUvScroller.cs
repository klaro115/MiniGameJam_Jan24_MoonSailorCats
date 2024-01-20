using UnityEngine;

namespace Background
{
	public class TerrainUvScroller : MonoBehaviour
	{
		#region Fields

		public Vector2 scrollSpeed = new(1, 0);
		private Vector2 textureOffset = Vector2.zero;

		private MeshRenderer rend = null;
		private Material mat = null;

		#endregion
		#region Constants

		private const string NAME_TEX_MAIN = "_MainTex";
		private const string NAME_TEX_NORMAL = "_BumpMap";

		#endregion
		#region Methods

		void Start()
		{
			rend = GetComponent<MeshRenderer>();
			mat = rend.material;

			textureOffset = mat.GetTextureOffset(NAME_TEX_MAIN);
		}

		void Update()
		{
			textureOffset += scrollSpeed * Time.deltaTime;

			mat.SetTextureOffset(NAME_TEX_MAIN, textureOffset);
			mat.SetTextureOffset(NAME_TEX_NORMAL, textureOffset);
		}

		#endregion
	}
}
