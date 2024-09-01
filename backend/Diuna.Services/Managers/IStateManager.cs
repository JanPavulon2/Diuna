using Diuna.Services.State;

namespace Diuna.Services.Managers;

public interface IStateManager
{
    SwitchState GetStateByTag(string tag);
    void UpdateInMemoryState(string tag, bool isOn);
    void SaveStateToFile();
    void LoadState(Dictionary<string, SwitchState> defaultState);
}