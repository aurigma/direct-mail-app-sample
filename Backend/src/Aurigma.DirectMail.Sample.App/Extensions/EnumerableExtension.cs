namespace Aurigma.DirectMail.Sample.App.Extensions;

public static class EnumerableExtension
{
    public static T PickRandom<T>(this IEnumerable<T> source)
    {
        return source.PickRandom(1).Single();
    }

    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
    {
        return source.Shuffle().Take(count);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => Guid.NewGuid());
    }

    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self) =>
        self?.Select((item, index) => (item, index)) ?? new List<(T, int)>();
}
