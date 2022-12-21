namespace RadzenHelper.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> SplitIntoNChunks<T>(this IEnumerable<T> source, int numberOfChunks)
        {
            if (numberOfChunks <= 0) throw new ArgumentException("number of chunks must be greater than zero.", "numberOfChunks");

            var pos = 0;

            int minChunksize = (int)(source.Count() / 150.0);

            var rest = source.Count() % 150;

            IEnumerable<T>[] result = new IEnumerable<T>[numberOfChunks];

            for (int i = 0; i < numberOfChunks; i++)
            {
                int take = i < rest ? minChunksize + 1 : minChunksize;

                result[i] = source.Skip(pos).Take(take);

                pos += take;
            }

            return result;
        }

        public static IEnumerable<IEnumerable<T>> GetChunksWithSize<T>(this IEnumerable<T> source, int chunksize)
        {
            if (chunksize <= 0) throw new ArgumentException("Chunk size must be greater than zero.", "chunksize");

            var pos = 0;
            while (source.Skip(pos).Any())
            {
                yield return source.Skip(pos).Take(chunksize);
                pos += chunksize;
            }
        }

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int N)
        {
            return source.Skip(Math.Max(0, source.Count() - N));
        }
    }
}
