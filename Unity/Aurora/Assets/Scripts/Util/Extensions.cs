using System;
using System.Collections.Generic;
using System.Linq;

public static class Extensions
{
    public static void AddIfNotNull<T>(this List<T> list, T value)
    {
        if ((object)value != null)
            list.Add(value);
    }



    public static void swapInts(ref int a, ref int b)
    {
      int temp = a;
      a = b;
      b = temp;
    }

     private static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource item)
    {
        if (source == null)
            throw new ArgumentNullException("source");

        yield return item;

        foreach (var element in source)
            yield return element;
    }

    public static IEnumerable<IEnumerable<TSource>> Permutations<TSource>(this IEnumerable<TSource> source)
    {
        if (source == null)
            throw new ArgumentNullException("source");

        var list = source.ToList();

        if (list.Count > 1)
            return from s in list
                   from p in Permutations(list.Take(list.IndexOf(s)).Concat(list.Skip(list.IndexOf(s) + 1)))
                   select p.Prepend(s);

        return new[] { list };
    }

    public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
{
    if (source == null)
    {
        throw new ArgumentNullException("source");
    }
    return new List<TSource>(source);
}
}