import React from 'react';
import { connect } from 'react-redux';
import './App.css';
import { ConnectivityState } from '../store/appState';

const mapStateToProps = (state: ConnectivityState) => ({
  currentState: state
})

const AppComponent = (props:any) => {
  return (
    <div className="App">
      <p>{JSON.stringify(props)}</p>
    </div>
  );
}

 export const App = connect(mapStateToProps)(AppComponent);
