import { createSlice } from "@reduxjs/toolkit"

const initialState = {
    step: '',
    resendTime: null,
    user: null,
    tempUser: null,
    error: null,
    success: null,
}

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers:{
        setTempUser(state, action){
            state.tempUser = action.payload;
        },
        resetTempUser(state){
            state.tempUser = null;
        },

        setUser(state, action){
            state.user = action.payload;
        },
        resetUser(state){
            state.user = null;
        },

        setStep(state, action){
            state.step = action.payload;
        },
        resetStep(state){
            state.step = ''
        },

        setResendTime(state, action){
            state.resendTime = action.payload;
        },
        resetResendTime(state){
            state.resendTime = null
        },

        setError(state, action){
            state.error = action.payload;
        },
        resetError(state){
            state.error = null;
        },

        setSuccess(state, action){
            state.success = action.payload;
        },
        resetSuccess(state){
            state.success = null;
        },

        logout(state){
            state.user = null,
            state.step = ''
        }
    }
});
export const {
    setTempUser, resetTempUser, 
    setUser, resetUser, 
    setStep, resetStep, 
    setResendTime, resetResendTime, 
    setError, resetError, 
    setSuccess, resetSuccess, 
    logout
} = authSlice.actions;
export default authSlice.reducer;