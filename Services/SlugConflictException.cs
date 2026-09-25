namespace ToroSolutions.Api.Services
{
    /// <summary>
    /// Thrown when a create or update would give a post a slug another post already has.
    /// Controllers map it to 409 Conflict so publishers (e.g. Verqos) can pick a new slug instead of retrying.
    /// </summary>
    public class SlugConflictException : Exception
    {
        public string Slug { get; }

        public SlugConflictException(string slug)
            : base($"A post with the slug '{slug}' already exists.")
        {
            Slug = slug;
        }
    }
}
