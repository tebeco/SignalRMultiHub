import React from 'react';
import { connect } from 'react-redux';
import './App.css';
import { ConnectivityState } from '../store/appState';

export type AppProps = {
  currentState: any
}

const mapStateToProps = (state: ConnectivityState) => ({
  currentState: state
})

const mapDispatchToProps = (dispatch : any) => {
  // return bindActionCreators(ConnectionAction, dispatch)
}

const AppComponent = (props: AppProps) => {
  return (
    <div className="App">
      <p>{JSON.stringify(props)}</p>
    </div>
  );
}

 export const App = connect(mapStateToProps, mapDispatchToProps)(AppComponent);
