namespace Plateforme.AlloTabib.DomainLayer.Helpers
{
    public static class Common
    {

        public static string ValidateQueryString(string q)
        {

            if (q.Contains(" "))
            {
                q = q.Replace(" ", "\\ ");
            }

            if (q.Contains(":"))
            {
                q = q.Replace(":", "\\:");
            }

            if (q.Contains("["))
            {
                q = q.Replace("[", "\\[");
            }
            if (q.Contains("]"))
            {
                q = q.Replace("]", "\\]");
            }
            if (q.Contains("{"))
            {
                q = q.Replace("{", "\\{");
            }
            if (q.Contains("}"))
            {
                q = q.Replace("}", "\\}");
            }
            if (q.Contains("("))
            {
                q = q.Replace("(", "\\(");
            }
            if (q.Contains(")"))
            {
                q = q.Replace(")", "\\)");
            }

            return q;
        }
    }
}
