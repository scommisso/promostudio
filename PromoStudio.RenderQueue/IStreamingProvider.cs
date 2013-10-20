namespace PromoStudio.RenderQueue
{
    public interface IStreamingProvider
    {
        string StoreFile(string downloadUrl, string videoName);
    }
}
