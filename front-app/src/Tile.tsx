import React from 'react';
import { Component } from 'react';
import { SubscribeCallback } from './ConnectedApp';
import { HubConnection } from '@aspnet/signalr';

export type TileProps = {
  tileId: string,
  connection: HubConnection,
}

export type TileState = {
  textGroupName?: string | string[] | number;
  groupName: string| undefined;
  lastMessage: string;
}
type ClientData = {
  groupId:string,
  data: string
}
export class Tile extends Component<TileProps, TileState> {

  constructor(props: TileProps) {
    super(props);

    props.connection.on("foo", (payload: ClientData) => {

      if(this.state.groupName === undefined || this.state.groupName !== payload.groupId){
        return;
      }

      const newState: TileState = {
        ...this.state,
        lastMessage: payload.data
      };
      this.setState(newState);
    });

    this.state = {
      textGroupName: '',
      groupName: undefined,
      lastMessage: ''
    };
  }


  handleChange(textGroupName: string) {
    this.setState({ groupName: textGroupName });
  }

  render() {
    return (
      <>
        <p>{this.props.tileId}</p>
        <input type="text" value={this.state.groupName} onChange={(e) => this.handleChange(e.target.value)} />
        <button onClick={async (e) => await handleJoin(this.props.connection, this.state.groupName)}>Join</button>
        <p>Last message : {this.state.lastMessage}</p>
      </>
    );
  }
}

const handleJoin = async (connection: HubConnection, groupName?: string | string[] | number) => {
  if (groupName === undefined || groupName === '') {
    console.log("can't do that");
    return;
  }

  console.log(`joining group : ${groupName}`);
  await connection.send('requestGroup', groupName.toString());
}
