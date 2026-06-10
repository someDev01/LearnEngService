import apiClient from "./client";

export const noteApi = {
    createNoteWithContext: async(youtubeVideoId, youtubeId, youtubeVideoTitle, hours, minutes, seconds, word, context) => {
        try{
            const response = await apiClient.post('note/create/with/context', {
                youtubeVideoId,
                youtubeId,
                youtubeVideoTitle,
                hours,
                minutes,
                seconds,
                word,
                context
            });

            return { success: true, data: response.data};
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

    createNote: async(word, translations = [], examples = []) => {
        try{
            const response = await apiClient.post('note/create', {
                word, 
                translations, 
                examples
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

    updateNote: async(noteId, word, translations, examples) => {
        try{
            const response = await apiClient.patch('note/update', {
                noteId,
                word,
                translations,
                examples
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

    getDictionary: async() => {
        try{
            const response = await apiClient.get('note/dictionary');

            return { success: true, data: response.data};
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

    getNotes: async(page, pageSize) => {
        try{
            const response = await apiClient.get('note/all', {
                params: {
                    page,
                    pageSize
                }
            });

            return {success: true, data:response.data};
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

    searchNotes: async(query, page, pageSize) => {
        try{
            const response = await apiClient.get('notes/search', {
                params:{
                    query,
                    page,
                    pageSize
                }
            })

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

    deleteNote: async(noteId) => {
        try{
            const response = await apiClient.delete(`note/delete`, {
                params: {noteId}
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

    updateRepetitionScore: async(noteId, isCorrect) => {
        try{
            const response = await apiClient.patch('note/updateRepetitionScore', {
                noteId,
                isCorrect
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
    }
};