import { createStore, applyMiddleware, combineReducers, compose, AnyAction } from 'redux';
import thunk from 'redux-thunk';
import { composeWithDevTools } from 'redux-devtools-extension';
import { AppState, defaultState } from './appState';
import { AppActions } from '../actions/app';
import { connectivityReducer } from '../reducers/connectivity';

const rootReducer = combineReducers({ connectivity : connectivityReducer });

export const configureStore = (preloadedState: AppState = defaultState) => {
    const enhancer = composeWithDevTools(applyMiddleware(thunk));

    const store = createStore<AppState, AppActions, {}, {}>(
        rootReducer,
        preloadedState,
        enhancer
    );

    if (module.hot) {
        // Enable Webpack hot module replacement for reducers
        module.hot.accept('../reducers/connectivity', () => {
            store.replaceReducer(rootReducer)
        });
    }

    return store;
}
