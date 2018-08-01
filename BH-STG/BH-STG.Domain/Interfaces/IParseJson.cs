using System.Collections.Generic;
using BHSTG.Domain.Model.ValueObjects.JsonModels;
using BHSTG.SharedKernel.BaseDomainObjects;

namespace BHSTG.Domain.Interfaces
{
    public interface IParseJson
    {
        GameWaves ConvertJsonToGameWave(string json);
        void ReadGameLevels();
        void SetGameLevels();
        List<float> GetWaveTimeFrame();
    }
}
