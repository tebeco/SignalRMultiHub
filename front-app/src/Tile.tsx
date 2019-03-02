import React from 'react';
import { HubConnection } from '@aspnet/signalr';

export type TileProps = {
  name: string,
}

export const Tile = (props: TileProps) => {
  return (
    <div>
      <p>{props.name}</p>
      <button onClick={(e) => onConnect()}>
        connect
      </button>
    </div>
  );
}

const onConnect = () => {
}
