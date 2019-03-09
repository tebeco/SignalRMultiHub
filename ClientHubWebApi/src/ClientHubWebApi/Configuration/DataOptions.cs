namespace ClientHubWebApi.Configuration
{
    public class DataOptions
    {
        public FolderConfiguration Stock { get; set; } = new FolderConfiguration();

        public FolderConfiguration Etf { get; set; } = new FolderConfiguration();
    }
}
