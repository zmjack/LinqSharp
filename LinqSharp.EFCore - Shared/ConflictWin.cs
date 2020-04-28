namespace LinqSharp
{
    public enum ConflictWin
    {
        Throw,

        /// <summary>
        /// Client Win.
        /// </summary>
        Client,

        /// <summary>
        /// Store Win.
        /// </summary>
        Store,

        /// <summary>
        /// If orginal values are different from stored values, Store Win, else Client Win.
        /// </summary>
        Combine,
    }
}
