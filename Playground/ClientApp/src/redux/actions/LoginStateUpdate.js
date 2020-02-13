import ACTION_TYPES from "./ActionTypes";

const loginStateUpdate = (state) => {
  return {
    type: ACTION_TYPES.SET_LOGIN_STATE,
    loginState: state
  };
};

export default loginStateUpdate;