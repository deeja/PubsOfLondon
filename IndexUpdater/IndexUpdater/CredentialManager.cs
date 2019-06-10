using CredentialManagement;

namespace IndexUpdater
{
    /// <summary>
    /// https://www.nuget.org/packages/CredentialManagement/
    /// https://stackoverflow.com/questions/17741424/retrieve-credentials-from-windows-credentials-store-using-c-sharp
    /// </summary>
    internal static class CredentialManager
    {
        private static string target = "PubsOfLondonIndexUpdater";

        public static string GetApiKey()
        {
            var cm = new Credential { Target = target };
            if (!cm.Load())
            {
                return null;
            }

            //UserPass is just a class with two string properties for user and pass
            return cm.Password;
        }

        public static bool SetApiKey(string apiKey)
        {
            return new Credential
            {
                Target = target,
                Username = "PubsOfLondon",
                Password = apiKey,
                PersistanceType = PersistanceType.LocalComputer
            }.Save();
        }

        public static bool RemoveCredentials()
        {
            return new Credential { Target = target}.Delete();
        }

    }
}