using UnityEngine;

public static class GameObjectExtension
{
	public static T GetOrAddComponent<T>(this GameObject gameObject)
		where T : Component
	{
		if (gameObject == null)
		{
			return null;
		}

		if (!gameObject.TryGetComponent(out T result))
		{
			result = gameObject.AddComponent<T>();
		}

		return result;
	}

	public static Vector3 NearestVertexTo(this GameObject gameObject, Vector3 point)
	{
		// convert point to local space
		point = gameObject.transform.InverseTransformPoint(point);

		Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
		float minDistanceSqr = Mathf.Infinity;
		Vector3 nearestVertex = Vector3.zero;

		// scan all vertices to find nearest
		foreach (Vector3 vertex in mesh.vertices)
		{
			Vector3 diff = point - vertex;
			float distSqr = diff.sqrMagnitude;

			if (distSqr < minDistanceSqr)
			{
				minDistanceSqr = distSqr;
				nearestVertex = vertex;
			}
		}

		// convert nearest vertex back to world space
		return gameObject.transform.TransformPoint(nearestVertex);

	}

	public static GameObject FindChild(this GameObject gameObject, string name = null, bool recursive = true)
	{
		if (gameObject.FindChild<Transform>(name, recursive) is Transform transform)
		{
			return transform.gameObject;
		}

		return null;
	}

	public static T FindChild<T>(this GameObject gameObject, string name = null, bool recursive = true)
		where T : Object
	{
		if (gameObject == null)
		{
			return null;
		}

		if (recursive)
		{
			foreach (T component in gameObject.GetComponentsInChildren<T>(true))
			{
				if (string.IsNullOrEmpty(name) || component.name == name)
				{
					return component;
				}
			}
		}
		else
		{
			for (int index = 0; index < gameObject.transform.childCount; index++)
			{
				Transform transform = gameObject.transform.GetChild(index);
				if (string.IsNullOrEmpty(name) || transform.name == name)
				{
					if (transform.TryGetComponent(out T component))
					{
						return component;
					}
				}
			}
		}

		return null;
	}
}
