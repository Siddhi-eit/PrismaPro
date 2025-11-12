import axios from 'axios';
import { APIRooT, connectionHubURL } from 'constants/defaultValues';
import AvailityDefaultValues from 'containers/form-validations/AvailityDefaultValues';

const backendHost = process.env.REACT_APP_Haute_API_URL;
const imageURL = process.env.REACT_APP_Haute_IMAGE_URL;
// const connectionHubURL = process.env.Haute_SignalR_URL;
export const API_ROOT = APIRooT;
export const IMAGE_ROOT = `${imageURL}`;
export const CONNECTION_Hub_URL = connectionHubURL;
export const APP_VERSION = process.env.REACT_APP_VERSION;
export const ReCaptchaPublicKey = '6LeIz64UAAAAAEzO6zmhP2bpcoq0O8i63xgSoHUv';

export default axios.create({
  baseURL: API_ROOT,
  withCredentials: false,
  headers: {
    'Content-type': 'application/json',
  },
});
