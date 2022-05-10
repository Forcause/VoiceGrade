using Vosk;

namespace VoiceGradeApi.Models;

public sealed class TranscriberModel
{
    private static TranscriberModel _instance = null;
    private static readonly object Padlock = new object();

    public readonly Model Model = new Model(@"AudioModels/vosk-model-ru-0.22");

    TranscriberModel()
    {
    }

    public static TranscriberModel Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new TranscriberModel();
                    }

                    return _instance;
                }
            }

            return _instance;
        }
    }
}