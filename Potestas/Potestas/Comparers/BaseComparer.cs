namespace Potestas.Comparers
{
    /// <summary>
    /// Base class for comparers.
    /// </summary>
    /// <typeparam name="T">The type, which instances, will be compared.</typeparam>
    public abstract class BaseComparer<T> where T : class
    {
        protected int? CompareConsideringNulls(T x, T y)
        {
            switch (x, y)
            {
                case (null, null):
                    return 0;
                case (_, null):
                    return 1;
                case (null, _):
                    return -1;
                default:
                    return null;
            }
        }
    }
}