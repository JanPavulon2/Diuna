using Diuna.Models.State;

namespace Diuna.Services.Managers;

public interface IStateManager
{
    SwitchState GetStateByTag(string tag);
    void UpdateInMemoryState(string tag, bool isOn);
    void SaveStateToFile();
    void LoadStateFromFile(Dictionary<string, SwitchState> defaultState);
}