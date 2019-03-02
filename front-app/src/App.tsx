import React from 'react';
import './App.css';
import { Tile } from './Tile';
import { HubConnectionState } from '@aspnet/signalr';
import { DisconnectedApp } from './DisconnectedApp';

export type AppProps = { hubConnectionState: HubConnectionState }

export const App = (props: AppProps) => {
  if (props.hubConnectionState === HubConnectionState.Connected) {
    return (
      <div className="App">
        <Tile name="coin" />
      </div>
    );
  }
  else {
    return (
      <div className="App">
        <DisconnectedApp />
      </div >
    );
  }
}


