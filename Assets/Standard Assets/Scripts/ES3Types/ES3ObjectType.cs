using ES3Internal;
using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public abstract class ES3ObjectType : ES3Type
	{
		public ES3ObjectType(Type type)
			: base(type)
		{
		}

		protected abstract void WriteObject(object obj, ES3Writer writer);

		protected abstract object ReadObject<T>(ES3Reader reader);

		protected virtual void ReadObject<T>(ES3Reader reader, object obj)
		{
			throw new NotSupportedException("ReadInto is not supported for type " + type?.ToString());
		}

		public override void Write(object obj, ES3Writer writer)
		{
			if (WriteUsingDerivedType(obj, writer))
			{
				return;
			}
			Type type = ES3Reflection.BaseType(obj.GetType());
			if (type != typeof(object))
			{
				ES3Type orCreateES3Type = ES3TypeMgr.GetOrCreateES3Type(type);
				if (orCreateES3Type.isDictionary || orCreateES3Type.isCollection)
				{
					writer.WriteProperty("_Values", obj, orCreateES3Type);
				}
			}
			WriteObject(obj, writer);
		}

		public override object Read<T>(ES3Reader reader)
		{
			string text = ReadPropertyName(reader);
			if (text == "__type")
			{
				return ES3TypeMgr.GetOrCreateES3Type(reader.ReadType()).Read<T>(reader);
			}
			if (text == null)
			{
				return null;
			}
			reader.overridePropertiesName = text;
			return ReadObject<T>(reader);
		}

		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			while (true)
			{
				string text = ReadPropertyName(reader);
				if (text == "__type")
				{
					break;
				}
				if (text == null)
				{
					return;
				}
				reader.overridePropertiesName = text;
				ReadObject<T>(reader, obj);
			}
			ES3TypeMgr.GetOrCreateES3Type(reader.ReadType()).ReadInto<T>(reader, obj);
		}
	}
}
