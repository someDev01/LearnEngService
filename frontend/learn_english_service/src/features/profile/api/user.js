import apiClient from "../../../api/client";

export const userApi = {
    uploadAvatar: async(formData) => {
        try{
            const response = await apiClient.post('upload-avatar', formData);
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
    }
};