import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import { App } from './App';
import * as serviceWorker from './serviceWorker';
import { HubConnectionBuilder, HubConnectionState } from '@aspnet/signalr';
import { DisconnectedApp } from './DisconnectedApp';

const clientsEndpoint = "https://localhost:5001/clients"
let hubConnectionState = HubConnectionState.Disconnected;

const getConnection = (endpoint: string) => {
    const hubConnection = new HubConnectionBuilder()
        .withUrl(endpoint)
        .build();

    return hubConnection;
};

const onClose = (err: Error | undefined) => {
    console.log('===================== DISCONNECTED =====================');

    hubConnectionState = HubConnectionState.Disconnected;
}

const hubConnection = getConnection(clientsEndpoint);
hubConnection.onclose(onClose);
hubConnection
    .start()
    .then(() => {
        console.log('===================== CONNECTED =====================');
        hubConnectionState = HubConnectionState.Connected;
    })
    .catch((err) => {
        console.log('===================== NOT CONNECTED =====================');
        hubConnectionState = HubConnectionState.Disconnected;
        ;
    });

ReactDOM.render(<App hubConnectionState={hubConnectionState} />, document.getElementById('root'));

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: http://bit.ly/CRA-PWA
serviceWorker.unregister();


