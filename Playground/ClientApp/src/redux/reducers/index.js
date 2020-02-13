import { combineReducers } from 'redux';
import loginStateReducer from './LoginStateReducer';

// Combine all reducers into a single one
const allReducers = combineReducers({
  login: loginStateReducer
});

export default allReducers;