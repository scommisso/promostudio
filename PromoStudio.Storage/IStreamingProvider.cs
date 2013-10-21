namespace PromoStudio.Storage
{
    public interface IStreamingProvider
    {
        string StoreFile(string downloadUrl, string videoName, string videoDescription);
    }
}
