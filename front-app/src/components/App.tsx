import './App.css'
import React from 'react';
import { DisconnectedApp } from './DisconnectedApp';
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'





// const getConnection = async (endpoint: string) => {
//     const hubConnection = new HubConnectionBuilder()
//         .withUrl(endpoint)
//         .build();
//     hubConnection.onclose((err) => {
//         ReactDOM.render(<DisconnectedApp />, document.getElementById('root'));
//     });
//     try {
//         await hubConnection.start()
//         ReactDOM.render(<ConnectedApp connection={hubConnection} />, document.getElementById('root'));
//     }
//     catch{
//         ReactDOM.render(<DisconnectedApp />, document.getElementById('root'));
//     }
// };
// getConnection("https://localhost:5001/clients");







const mapStateToProps = (state) => ({
  counter: state.counter
})

const mapDispatchToProps = (dispatch) => {
  return bindActionCreators(CounterActions, dispatch)
}

const App = () => {
  return (
      <Provider store={store}>
          <DisconnectedApp />
      </Provider>
  );
}


export default connect(mapStateToProps, mapDispatchToProps)(App)
