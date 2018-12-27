﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SecurityDriven.TinyORM.Extensions
{
	using Utils;

	public static class DynamicListExtensions
	{
		/// <summary>Converts a List of RowStore-objects into an array of T-objects on a best-effort-match basis. Parallelized. Does not throw on any mismatches.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] ToObjectArray<T>(this IReadOnlyList<dynamic> listOfDynamic) where T : class, new() => ToObjectArray<T>(listOfDynamic, New<T>.Instance);

		/// <summary>Converts a List of RowStore-objects into an array of T-objects on a best-effort-match basis. Parallelized. Does not throw on any mismatches.</summary>
		public static T[] ToObjectArray<T>(this IReadOnlyList<dynamic> listOfDynamic, Func<T> objectFactory) where T : class
		{
			var listOfRowStore = (List<RowStore>)listOfDynamic;
			int newListCount = listOfRowStore.Count;
			T[] newList = new T[newListCount];
			if (newListCount == 0) return newList;

			RowStore firstElement = listOfRowStore[0];

			var settersArray = new Action<T, object>[firstElement.RowValues.Length];
			var fieldMap = firstElement.Schema.FieldMap;
			var settersEnumerator = ReflectionHelper_Setter<T>.Setters.GetEnumerator();

			while (settersEnumerator.MoveNext())
			{
				var setter = settersEnumerator.Current;
				if (fieldMap.TryGetValue(setter.Key, out var index))
					settersArray[index] = setter.Value;
			}

			Parallel.For(0, newListCount, i =>
			{
				T objT = objectFactory();
				newList[i] = objT;
				object[] rowValues = listOfRowStore[i].RowValues;
				DBNull dbNullValue = DBNull.Value;

				for (i = 0; i < rowValues.Length; ++i)
				{
					var setter = settersArray[i];
					if (setter == null) continue;

					object val = rowValues[i];
					setter(objT, val == dbNullValue ? null : val);
				}//for
			});
			return newList;
		}// ToObjectArray<T>()

		/// <summary>Converts a List of dynamic objects into an array of T-objects using a provided object mapper. Parallelized.</summary>
		public static T[] ToMappedObjectArray<T>(this IReadOnlyList<dynamic> listOfDynamic, Func<dynamic, T> objectMapper)
		{
			unchecked
			{
				var listOfObject = (IReadOnlyList<object>)listOfDynamic;
				int newListCount = listOfObject.Count;
				T[] newList = new T[newListCount];
				if (newListCount == 0) return newList;
				var objectMapperAlias = objectMapper;

				Parallel.For(0, newListCount, i => newList[i] = objectMapperAlias(listOfObject[i]));
				return newList;
			}
		}//ToMappedObjectArray<T>

		/// <summary>Converts any Array or List of T into a single-column named TVP.</summary>
		public static DataTable AsTVP<T>(this IReadOnlyList<T> list, string tvpName)
		{
			var dataTable = new DataTable(tvpName);
			dataTable.Columns.Add();

			var rows = dataTable.Rows;
			var paramsContainer = new object[1];
			ref var val = ref paramsContainer[0];
			var count = list.Count;

			for (int i = 0; i < count; ++i)
			{
				val = list[i];
				rows.Add(paramsContainer);
			}
			return dataTable;
		}//AsTVP<T>

		#region Deconstructors
		///<summary>Deconstructor.</summary>
		public static void Deconstruct<T>(this IReadOnlyList<T> v, out T v0) { v0 = v[0]; }

		///<summary>Deconstructor.</summary>
		public static void Deconstruct<T>(this IReadOnlyList<T> v, out T v0, out T v1) { v0 = v[0]; v1 = v[1]; }

		///<summary>Deconstructor.</summary>
		public static void Deconstruct<T>(this IReadOnlyList<T> v, out T v0, out T v1, out T v2) { v0 = v[0]; v1 = v[1]; v2 = v[2]; }

		///<summary>Deconstructor.</summary>
		public static void Deconstruct<T>(this IReadOnlyList<T> v, out T v0, out T v1, out T v2, out T v3) { v0 = v[0]; v1 = v[1]; v2 = v[2]; v3 = v[3]; }

		///<summary>Deconstructor.</summary>
		public static void Deconstruct<T>(this IReadOnlyList<T> v, out T v0, out T v1, out T v2, out T v3, out T v4) { v0 = v[0]; v1 = v[1]; v2 = v[2]; v3 = v[3]; v4 = v[4]; }

		///<summary>Deconstructor.</summary>
		public static void Deconstruct<T>(this IReadOnlyList<T> v, out T v0, out T v1, out T v2, out T v3, out T v4, out T v5) { v0 = v[0]; v1 = v[1]; v2 = v[2]; v3 = v[3]; v4 = v[4]; v5 = v[5]; }

		///<summary>Deconstructor.</summary>
		public static void Deconstruct<T>(this IReadOnlyList<T> v, out T v0, out T v1, out T v2, out T v3, out T v4, out T v5, out T v6) { v0 = v[0]; v1 = v[1]; v2 = v[2]; v3 = v[3]; v4 = v[4]; v5 = v[5]; v6 = v[6]; }

		///<summary>Deconstructor.</summary>
		public static void Deconstruct<T>(this IReadOnlyList<T> v, out T v0, out T v1, out T v2, out T v3, out T v4, out T v5, out T v6, out T v7) { v0 = v[0]; v1 = v[1]; v2 = v[2]; v3 = v[3]; v4 = v[4]; v5 = v[5]; v6 = v[6]; v7 = v[7]; }

		///<summary>Deconstructor.</summary>
		public static void Deconstruct<T>(this IReadOnlyList<T> v, out T v0, out T v1, out T v2, out T v3, out T v4, out T v5, out T v6, out T v7, out T v8) { v0 = v[0]; v1 = v[1]; v2 = v[2]; v3 = v[3]; v4 = v[4]; v5 = v[5]; v6 = v[6]; v7 = v[7]; v8 = v[8]; }

		///<summary>Deconstructor.</summary>
		public static void Deconstruct<T>(this IReadOnlyList<T> v, out T v0, out T v1, out T v2, out T v3, out T v4, out T v5, out T v6, out T v7, out T v8, out T v9) { v0 = v[0]; v1 = v[1]; v2 = v[2]; v3 = v[3]; v4 = v[4]; v5 = v[5]; v6 = v[6]; v7 = v[7]; v8 = v[8]; v9 = v[9]; }
		#endregion
	}// class DynamicListExtensions
}//ns