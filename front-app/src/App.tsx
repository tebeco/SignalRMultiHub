import React, { useReducer, Dispatch } from 'react';
import './App.css';
import { HubConnectionState, HubConnectionBuilder } from '@aspnet/signalr';
import { DisconnectedApp } from './DisconnectedApp';
import { ConnectedApp } from './ConnectedApp';

type AppAction = ConnectedAction | DisconnectedAction;
type DisconnectedAction = {
  type: 'connected'
}
type ConnectedAction = {
  type: 'disconnected'
}

type AppState = { isConnected: boolean };
const initialState: AppState = { isConnected: false };

type AppStateReducer = (state: AppState, action: AppAction) => AppState;

const reducer: AppStateReducer = (state, action): AppState => {
  switch (action.type) {
    case 'connected':
      return { isConnected: true };
    case 'disconnected':
      return { isConnected: false };
    default:
      return state;
  }
}

const getConnection = (endpoint: string, dispacth: Dispatch<AppAction>) => {
  const hubConnection = new HubConnectionBuilder()
    .withUrl(endpoint)
    .build();

  hubConnection.onclose((err) => {
    dispacth({ type: 'disconnected' });
  });

  hubConnection
    .start()
    .then(() => {
      dispacth({ type: 'connected' });
    })
    .catch((err) => {
      dispacth({ type: 'disconnected' });
    });
};

export type AppProps = { clientsEndpoint: string }

export const App = (props: AppProps) => {
  const [state, dispatch] = useReducer(reducer, { isConnected: initialState.isConnected });

  if (!state.isConnected) {
    setTimeout(() => { }, 800)
    getConnection(props.clientsEndpoint, dispatch);
  }

  if (state.isConnected) {
    return (
      <div className="App">
        <ConnectedApp />
      </div>
    );
  }
  else {
    return (
      <div className="App">
        <DisconnectedApp />
      </div>
    );
  }
}
