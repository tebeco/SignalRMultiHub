export type ConnectivityActions =
    | ConnectAction
    | ConnectingAction
    | DisconnectedAction
    | ConnectedAction

export type CONNECT_ACTION = 'CONNECT_ACTION'
export type ConnectAction = {
    type: CONNECT_ACTION
}

export type CONNECTING_ACTION = 'CONNECTING_ACTION'
export type ConnectingAction = {
    type: CONNECTING_ACTION
}

export type CONNECTED_ACTION = 'CONNECTED_ACTION'
export type ConnectedAction = {
    type: CONNECTED_ACTION
}

export type DISCONNECTED_ACTION = 'DISCONNECTED_ACTION'
export type DisconnectedAction = {
    type: DISCONNECTED_ACTION
}