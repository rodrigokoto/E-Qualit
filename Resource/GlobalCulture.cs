using System.Globalization;

namespace Traducao
{
    public static class GlobalCulture
    {
        public static void SetCulture(string culture)
        {
            if (string.IsNullOrEmpty(culture))
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-br");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-br");
            }
            else
            {
                string _culture = string.Empty;

                if (culture == "en")
                    _culture = "en-US"; //Assume o ingles americano como default
                else if (culture == "es")
                    _culture = "es-ES"; //Assume o espanhol da espanha como default
                else
                    _culture = culture;

                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(_culture);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(_culture);
            }
        }

        public static string GetCulture()
        {
            CultureInfo _culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            return _culture.Name;
        }
    }
}
