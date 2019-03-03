import React from 'react';
import './App.css'
import { Tile } from './Tile';
import { HubConnection } from '@aspnet/signalr';

export type SubscribeCallback = (groupName: string) => Promise<void>;
type ConnectedAppProps = {
    connection: HubConnection,
}

export const ConnectedApp = (props: ConnectedAppProps) => {
    return (
        <div className="App">
            <Tile tileId="Some tile name" connection={props.connection} />
            <Tile tileId="Some tile name" connection={props.connection} />
        </div>
    );
}

