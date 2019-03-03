import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import * as serviceWorker from './serviceWorker';
import { HubConnectionBuilder } from '@aspnet/signalr';
import { ConnectedApp } from './ConnectedApp';
import { DisconnectedApp } from './DisconnectedApp';

const clientsEndpoint = "https://localhost:5001/clients"


const getConnection = async (endpoint: string) => {
    const hubConnection = new HubConnectionBuilder()
        .withUrl(endpoint)
        .build();

    hubConnection.onclose((err) => {
        ReactDOM.render(<DisconnectedApp />, document.getElementById('root'));
    });

    try {
        await hubConnection.start()
        ReactDOM.render(<ConnectedApp connection={hubConnection} />, document.getElementById('root'));
    }
    catch{

        ReactDOM.render(<DisconnectedApp />, document.getElementById('root'));
    }
};

getConnection(clientsEndpoint);

// ReactDOM.render(<App clientsEndpoint={clientsEndpoint} />, document.getElementById('root'));

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: http://bit.ly/CRA-PWA
serviceWorker.unregister();
