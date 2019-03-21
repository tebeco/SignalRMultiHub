import { createStore, applyMiddleware, combineReducers, compose } from 'redux';
import thunk from 'redux-thunk';
import { composeWithDevTools } from 'redux-devtools-extension';

const rootReducer = combineReducers({});

export const configureStore = () => {
    const enhancer = composeWithDevTools(applyMiddleware(thunk));

    const store = createStore(
        rootReducer,
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
