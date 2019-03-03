import React from 'react';
import './App.css'
import { Tile } from './Tile';
import { HubConnection } from '@aspnet/signalr';

type ConnectedAppProps = {
    connection: HubConnection
}

export type SubscribeCallback = (group: string) => void;

export const ConnectedApp = (props: ConnectedAppProps) => {
    return (
        <div className="App">
            <Tile name="Some tile name" onSubscribe={(group) => subscribe(group)} />
        </div>
    );
}

const subscribe: SubscribeCallback = (groupName) => {
    console.log(`joining group : ${groupName}`);
}
