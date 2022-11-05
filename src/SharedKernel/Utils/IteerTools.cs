namespace SharedKernel.Utils;

public static class IteerTools
{
    // Copy from https://stackoverflow.com/questions/15150147/all-permutations-of-a-list
    public static IEnumerable<IEnumerable<T>> Permute<T>(
        this IEnumerable<T> sequence
    )
    {
        if (sequence == null)
        {
            yield break;
        }

        var list = sequence.ToList();

        if (!list.Any())
        {
            yield return Enumerable.Empty<T>();
        }
        else
        {
            var startingElementIndex = 0;

            foreach (var startingElement in list)
            {
                var index = startingElementIndex;
                var remainingItems = list.Where((e, i) => i != index);

                foreach (var permutationOfRemainder in remainingItems.Permute())
                {
                    yield return permutationOfRemainder.Prepend(startingElement);
                }

                startingElementIndex++;
            }
        }
    }
}
