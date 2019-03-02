import React from 'react';
import './App.css'
import { Tile } from './Tile';

export const ConnectedApp = () => {
    return (
        <div className="App">
            <Tile name="coin" />
        </div>
    );
}
