namespace PromoStudio.Common.Enumerations
{
    public enum CustomerVideoRenderStatus : sbyte
    {
        Pending = 1,
        InProgressTemplatePreview,
        CompletedTemplatePreview,
        InProgressVideoPreview,
        CompletedVideoPreview,
        UploadingPreview,
        PendingVoiceTalent,
        CompletedVoiceTalent,
        InProgressTemplate,
        CompletedTemplate,
        InProgressFinalRender,
        CompletedFinalRender,
        UploadingFinalRender,
        Completed,
        Canceled
    }
}
