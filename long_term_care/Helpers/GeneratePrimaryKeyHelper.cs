namespace long_term_care.Helpers
{
    public class GeneratePrimaryKeyHelper
    {
        private static string GeneratePrimaryKeys(string prefix, int nextId)
        {
            string formattedId = nextId.ToString().PadLeft(3, '0');
            return prefix + formattedId;
        }
    }
}
