namespace ClientHubWebApi.Configuration
{
    public class FolderConfiguration : IDataConfiguration
    {
        public string Folder { get; set; } = "";

        public string GlobbingPattern { get; set; } = "*.us.txt";
    }
}
