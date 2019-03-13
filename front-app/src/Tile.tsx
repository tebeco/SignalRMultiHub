import React from 'react';
import { Component } from 'react';
import { HubConnection } from '@aspnet/signalr';

export type TileProps = {
  tileId: string;
  connection: HubConnection;
}

type TileState = {
  textGroupName?: string | string[] | number;
  subscribedStockName: string | undefined;
  lastMessage: string;
}

type ClientData = {
  groupId: string;
  data: string;
}

export type Stock = {
  name: string;
  date: Date;
  open: number;
  low: number;
  high: number;
  close: number;
  openInt: number;
}

export class Tile extends Component<TileProps, TileState> {

  constructor(props: TileProps) {
    super(props);

    this.state = {
      textGroupName: '',
      subscribedStockName: 'msft',
      lastMessage: ''
    };
  }


  handleChange(textGroupName: string) {
    this.setState({ textGroupName: textGroupName });
  }

  render() {
    return (
      <>
        <p>{this.props.tileId}</p>
        <input type="text" value={this.state.textGroupName} onChange={(e) => this.handleChange(e.target.value)} />
        <button onClick={async (e) => {

          var stream = await handleJoin(this.props.connection, this.state.subscribedStockName);
          if (stream != null) {
            stream.subscribe({
              next: (item) => {
                // if (this.state.subscribedStockName === undefined || this.state.subscribedStockName !== payload.name) {
                //   return;
                // }
                this.setState({
                  ...this.state,
                  lastMessage: item.high.toString()
                });
              },
              complete: () => {
                this.setState({
                  ...this.state,
                  lastMessage: "stream has ended"
                });
              },
              error: (err) => {
                this.setState({
                  ...this.state,
                  lastMessage: err.toString()
                });
              }
            });
          }
        }
        }>Join</button>
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

  return connection.stream("GetStockStreamAsync", { underlying: groupName });
}
