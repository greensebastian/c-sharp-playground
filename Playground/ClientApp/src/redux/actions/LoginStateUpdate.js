import ACTION_TYPES from "./ActionTypes";

const loginStateUpdate = (state) => {
  return {
    type: ACTION_TYPES.SET_LOGIN_STATE,
    state: state
  };
};

export default loginStateUpdate;