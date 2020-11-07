using GameJam.Types;
using TBEngine.Components.State;
using TBEngine.Services;

namespace GameJam.Services {
    public sealed class StateService : StateServiceBase<GameStateType> { 
    
        public void ReinitializeStates(ContentService content, ConfigurationService config) {
            foreach (State state in States.Values) {
                state.Width = config.ViewWidth;
                state.Height = config.ViewHeight;
                state.Initialize(content);
            }
        }

    }
}
