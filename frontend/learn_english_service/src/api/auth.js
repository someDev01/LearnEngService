import apiClient from "./client";

export const authApi = {
    sendCode: async (email) => {
        try{
            const response = await apiClient.post('auth/send-code', { email });
            return {success: true, data: response.data};
        }
        catch(e){
            let message = 'Ошибка сервера';
            
            if (e.response) {
                if (e.response.data?.errors) {
                    message = Object.values(e.response.data.errors).flat().join(', ');
                } else if (e.response.data?.message) {
                    message = e.response.data.message; 
                } else {
                    message = e.response.data || message;
                }
            } else if (e.message) {
                message = e.message;
            }

            return { success: false, error: message };
        }
    },

    verifyCode: async (email, code) => {
        try{
            const response = await apiClient.post('auth/verify-code', { email, code });
            return {success: true, data: response.data}
        }
        catch(e){
            let message = 'Ошибка сервера';
            
            if (e.response) {
                if (e.response.data?.errors) {
                    message = Object.values(e.response.data.errors).flat().join(', ');
                } else if (e.response.data?.message) {
                    message = e.response.data.message; 
                } else {
                    message = e.response.data || message;
                }
            } else if (e.message) {
                message = e.message;
            }

            return { success: false, error: message };
        }
    },

    me: async () => {
        try{
            const response = await apiClient.get('auth/me');
            return {success: true, data: response.data}
        }
        catch(e){
            let message = 'Ошибка сервера';
            
            if (e.response) {
                if (e.response.data?.errors) {
                    message = Object.values(e.response.data.errors).flat().join(', ');
                } else if (e.response.data?.message) {
                    message = e.response.data.message;
                } else {
                    message = e.response.data || message;
                }
            } else if (e.message) {
                message = e.message;
            }

            return { success: false, error: message };
        }
    },

    logout: async () => {
        try{
            const response =  await apiClient.post('auth/logout');
            return {success: true, data: response.data}
        }
        catch(e){
            let message = 'Ошибка сервера';
            
            if (e.response) {
                if (e.response.data?.errors) {
                    message = Object.values(e.response.data.errors).flat().join(', ');
                } else if (e.response.data?.message) {
                    message = e.response.data.message;
                } else {
                    message = e.response.data || message;
                }
            } else if (e.message) {
                message = e.message;
            }

            return { success: false, error: message };
        }
    }
}