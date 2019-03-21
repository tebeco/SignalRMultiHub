import { ConnectivityState, defaultConnectivityState } from "../store/appState";
import { ConnectivityActions } from "../actions/connection";

export const connectivityReducer = (state: ConnectivityState = defaultConnectivityState, action: ConnectivityActions) => {
    return state;
}
