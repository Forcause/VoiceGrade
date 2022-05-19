using Vosk;

namespace VoiceGradeApi.Models;

public sealed class TranscriberModel
{
    private readonly Model _model;
    public Model Model => _model;

    TranscriberModel()
    {
        _model = new Model(@"AudioModels/vosk-model-ru-0.22_1");
    }

    private static readonly Lazy<TranscriberModel> Lazy = new(() => new TranscriberModel());

    public static TranscriberModel Instance => Lazy.Value;
}