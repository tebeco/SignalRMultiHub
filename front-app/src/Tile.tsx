import React from 'react';
import { Component } from 'react';
import { SubscribeCallback } from './ConnectedApp';

export type TileProps = {
  name: string,
  onSubscribe: SubscribeCallback,
}

export type TileState = {
  groupName?: string | string[] | number;
}

export class Tile extends Component<TileProps, TileState> {

  constructor(props: TileProps) {
    super(props);

    this.state = { groupName: '' };
  }

  handleChange(textGroupName: string) {
    this.setState({ groupName: textGroupName });
  }

  handleJoin() {
    if (this.state.groupName === undefined || this.state.groupName === '') {
      console.log("can't do that");
      return;
    }

    const groupName = this.state.groupName.toString();
    this.props.onSubscribe(groupName);
  }

  render() {
    return (
      <>
        <p>{this.props.name}</p>
        <input type="text" value={this.state.groupName} onChange={(e) => this.handleChange(e.target.value)} />
        <button onClick={(e) => this.handleJoin()}>
          Join
      </button>
      </>
    );
  }
}