import axios from "axios";
import {store} from '../redux/store/store';
import { resetStep, resetTempUser, resetUser } from "../redux/slices/authSlice";

const apiClient = axios.create({
    //baseURL: import.meta.env.VITE_API_URL,
    withCredentials: true
});

apiClient.interceptors.response.use(
    (response) => response,
    (error) => {
        if(error.response?.status === 401){
            store.dispatch(resetUser());
            store.dispatch(resetTempUser());
            store.dispatch(resetStep());
        }

        return Promise.reject(error);
    }
)

export default apiClient;