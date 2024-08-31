using Diuna.Services.State;

namespace Diuna.Services.Managers;

public interface IStateManager
{
    SwitchState GetStateByTag(string tag);
    void UpdateState(string tag, bool isOn);
    void SaveState();
    void LoadState();
}