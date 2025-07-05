using ES3Internal;
using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public abstract class ES3ComponentType : ES3UnityObjectType
	{
		protected const string gameObjectPropertyName = "goID";

		public ES3ComponentType(Type type)
			: base(type)
		{
		}

		protected abstract void WriteComponent(object obj, ES3Writer writer);

		protected abstract void ReadComponent<T>(ES3Reader reader, object obj);

		protected override void WriteUnityObject(object obj, ES3Writer writer)
		{
			Component component = obj as Component;
			if (obj != null && component == null)
			{
				throw new ArgumentException("Only types of UnityEngine.Component can be written with this method, but argument given is type of " + obj.GetType()?.ToString());
			}
			ES3ReferenceMgrBase current = ES3ReferenceMgrBase.Current;
			if (current != null)
			{
				writer.WriteProperty("goID", current.Add(component.gameObject).ToString(), ES3Type_string.Instance);
			}
			WriteComponent(component, writer);
		}

		protected override void ReadUnityObject<T>(ES3Reader reader, object obj)
		{
			ReadComponent<T>(reader, obj);
		}

		protected override object ReadUnityObject<T>(ES3Reader reader)
		{
			throw new NotImplementedException();
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			ES3ReferenceMgrBase current = ES3ReferenceMgrBase.Current;
			long id = -1L;
			UnityEngine.Object @object = null;
			foreach (string property in reader.Properties)
			{
				if (!(property == "_ES3Ref"))
				{
					if (property == "goID")
					{
						long id2 = reader.Read_ref();
						GameObject gameObject = (GameObject)current.Get(id2, type);
						if (gameObject == null)
						{
							gameObject = new GameObject("Easy Save 3 Loaded GameObject");
							current.Add(gameObject, id2);
						}
						@object = GetOrAddComponent(gameObject, type);
						current.Add(@object, id);
					}
					else
					{
						reader.overridePropertiesName = property;
						if (@object == null)
						{
							GameObject gameObject2 = new GameObject("Easy Save 3 Loaded GameObject");
							@object = GetOrAddComponent(gameObject2, type);
							current.Add(@object, id);
							current.Add(gameObject2);
						}
					}
					break;
				}
				id = reader.Read_ref();
				@object = current.Get(id, suppressWarnings: true);
			}
			if (@object != null)
			{
				ReadComponent<T>(reader, @object);
			}
			return @object;
		}

		private static Component GetOrAddComponent(GameObject go, Type type)
		{
			Component component = go.GetComponent(type);
			if (component != null)
			{
				return component;
			}
			return go.AddComponent(type);
		}

		public static Component CreateComponent(Type type)
		{
			GameObject gameObject = new GameObject("Easy Save 3 Loaded Component");
			if (type == typeof(Transform))
			{
				return gameObject.GetComponent(type);
			}
			return GetOrAddComponent(gameObject, type);
		}
	}
}
