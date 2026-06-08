import { createSlice } from "@reduxjs/toolkit"

const initialState = {
    isOpenMenu: false
}

const menuSlice = createSlice({
    name: 'menu',
    initialState,
    reducers:{
        openMenu(state){
            state.isOpenMenu = true;
        },
        closeMenu(state){
            state.isOpenMenu = false;
        }
    }
});

export const {openMenu, closeMenu} = menuSlice.actions;
export default menuSlice.reducer;