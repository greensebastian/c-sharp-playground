import ACTION_TYPES from '../actions/ActionTypes';
import { getLoginState } from '../../components/Login/LoginHandler';

// INITIAL STATE
const initialLoginState = getLoginState();
initialLoginState.showModal = false;

// REDUCER
const loginReducer = (state = initialLoginState, action) => {
  switch (action.type) {
    case ACTION_TYPES.SET_LOGIN_STATE:
      return action.state;
    case ACTION_TYPES.SET_SHOW_LOGIN_MODAL:
      let newState = Object.assign({}, state);
      newState.showModal = action.showModal;
      return newState;
    default:
      return state;
  }
};

export default loginReducer;