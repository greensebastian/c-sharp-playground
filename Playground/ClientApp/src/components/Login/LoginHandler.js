import { PATHS } from "../../resources/Constants";
import { call } from "../../helpers/Requests";

async function postRegister(jsonData) {
  return await call(PATHS.REGISTER, jsonData);
}
async function postLogin(jsonData){
  return await call(PATHS.LOGIN, jsonData);
}
async function getMe(parameters){
  return await call(PATHS.ME, "", "GET", parameters);
}
async function deleteMe(jsonData){
  return await call(PATHS.ME, jsonData, "DELETE");
}
async function postLogout(){
  return await call(PATHS.LOGOUT);
}

function getLoginState(serviceResponse){
  let loginState;
  if(serviceResponse){
    loginState = {
      loggedIn: true,
      showModal: true,
      username: serviceResponse.username,
      email: serviceResponse.email,
      activitySegmentCount: serviceResponse.activitySegmentCount,
      placeVisitCount: serviceResponse.placeVisitCount
    };
  }else {
    loginState = {
      loggedIn: false,
      showModal: true,
      username: "",
      email: "",
      activitySegmentCount: 0,
      placeVisitCount: 0
    };
  }
  return loginState;
}

export { postRegister, postLogin, getMe, deleteMe, getLoginState, postLogout };