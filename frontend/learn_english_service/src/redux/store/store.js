import { configureStore } from "@reduxjs/toolkit";
import modalReducer from '../slices/modalSlice';
import menuReducer from '../slices/menuSlice';
import authReducer from '../slices/authSlice';

export const store = configureStore({
    reducer:{
        modal: modalReducer,
        menu: menuReducer,
        auth: authReducer,
    }
})