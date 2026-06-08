import { createSlice } from "@reduxjs/toolkit"

const initialState = {
    isOpenVideoModal: false,
    openedVideo: null,

    isOpenModalViewNote: false,
    isOpenModalEditNote: false,
    isOpenModalWordPopup: false,
    isOpenModalTraining: false,
    isOpenModalAuth: false
}

const modalSlice = createSlice({
    name: 'modal',
    initialState,
    reducers:{
        openVideoModal(state, action){
            state.isOpenVideoModal = true;
            state.openedVideo = action.payload;
        },
        closeVideoModal(state){
            state.isOpenVideoModal = false;
            state.openedVideo = null;
        },

        openModalViewNote(state){
            state.isOpenModalViewNote = true;
        },
        closeModalViewNote(state){
            state.isOpenModalViewNote = false;
        },

        openModalEditNote(state){
            state.isOpenModalEditNote = true;
        },
        closeModalEditNote(state){
            state.isOpenModalEditNote = false;
        },

        openModalWordPopup(state){
            state.isOpenModalWordPopup = true;
        },
        closeModalWordPopup(state){
            state.isOpenModalWordPopup = false;
        },

        openModalTraining(state){
            state.isOpenModalTraining = true;
        },
        closeModalTrainig(state){
            state.isOpenModalTraining = false;
        },

        openModalAuth(state){
            state.isOpenModalAuth = true;
        },
        closeModalAuth(state){
            state.isOpenModalAuth = false;
        }
    }
});

export const {
    openVideoModal,
    closeVideoModal, 
    openModalViewNote, 
    closeModalViewNote, 
    openModalEditNote, 
    closeModalEditNote, 
    openModalWordPopup,
    closeModalWordPopup,
    openModalTraining,
    closeModalTrainig,
    openModalAuth, 
    closeModalAuth}
 = modalSlice.actions;
export default modalSlice.reducer;