using GameJam.Types;
using TBEngine.Components.State;
using TBEngine.Services;

namespace GameJam.Services {
    /// <summary>
    /// States service
    /// </summary>
    public sealed class StateService : StateServiceBase<GameStateType> { 
    
        /// <summary>
        /// Reinitialize all states
        /// </summary>
        /// <param name="content"><see cref="ContentService"/></param>
        /// <param name="config"><see cref="ConfigurationService"/></param>
        public void ReinitializeStates(ContentService content, ConfigurationService config) {
            foreach (State state in States.Values) {
                state.Width = config.ViewWidth;
                state.Height = config.ViewHeight;
                state.Initialize(content);
            }
        }

    }
}
