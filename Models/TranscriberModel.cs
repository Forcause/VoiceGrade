using Vosk;

namespace VoiceGradeApi.Models;

public sealed class TranscriberModel
{
    private static TranscriberModel instance = null;
    private static readonly object padlock = new object();

    public readonly Model _model = new Model(@"AudioModels/vosk-model-ru-0.22");

    TranscriberModel()
    {
    }

    public static TranscriberModel Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new TranscriberModel();
                }
                return instance;
            }
        }
    }
}