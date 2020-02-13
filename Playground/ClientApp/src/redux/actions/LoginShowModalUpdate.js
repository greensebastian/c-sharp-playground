import ACTION_TYPES from "./ActionTypes";

const loginShowModalUpdate = (state) => {
  return {
    type: ACTION_TYPES.SET_SHOW_LOGIN_MODAL,
    showModal: state
  };
};

export default loginShowModalUpdate;