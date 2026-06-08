import apiClient from "./client";

export const searchContentApi = {
    search: async(title) => {
        try{
            const response = await apiClient.get('api/learningContetns/search', {
                params: {title}
            });
            return {success: true, data: response.data};
        }
        catch(e){
            let message = 'Ошибка сервера';
            
            if (e.response) {
                if (e.response.data?.errors) {
                    message = Object.values(e.response.data.errors).flat().join(', ');
                } 
                else if (e.response.data?.message) {
                    message = e.response.data.message; 
                } 
                else {
                    message = e.response.data || message;
                }
            } 
            else if (e.message) {
                message = e.message;
            }

            return { success: false, error: message };
        }
    },
     getContents: async() => {
            try{
            const response = await apiClient.get('api/learningContetns/all');
            return {success: true, data: response.data};
        }
        catch(e){
            let message = 'Ошибка сервера';
            
            if (e.response) {
                if (e.response.data?.errors) {
                    message = Object.values(e.response.data.errors).flat().join(', ');
                } 
                else if (e.response.data?.message) {
                    message = e.response.data.message; 
                } 
                else {
                    message = e.response.data || message;
                }
            } 
            else if (e.message) {
                message = e.message;
            }

            return { success: false, error: message };
        }
     }
}