export type AppState = {
    connectivity: ConnectivityState
}

export type ConnectivityState = {
    current: string
}

export const defaultConnectivityState: ConnectivityState = {
    current: 'disconnected'
};

//default state is marked as disconnected
export const defaultState: AppState = {
    connectivity: defaultConnectivityState
}
