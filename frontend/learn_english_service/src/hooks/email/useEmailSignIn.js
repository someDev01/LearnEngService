import { useState } from "react";
import { EMAIL_REGEX } from "../../utils/email/emailValidate";
import { authApi } from "../../api/auth";
import { convertToTimeSeconds } from "../../utils/convert_time/convertToTimeSeconds";

function useEmailSignIn(){
    const [email, setEmail] = useState('');
    const [emailError, setEmailError] = useState('');
    const [loading, setLoading] = useState(false);

    const validate = () => {
        setEmailError('');

        if(!email?.trim()){
            setEmailError('Укажите email');
            return false;
        }

        if(!email?.trim()){
            setEmailError('Укажите email');
            return false;
        }

        if(email && !EMAIL_REGEX.test(email)){
            setEmailError('email указан некорректно');
            return false;
        }

        return true;
    }

    const handleSubmit = async() => {
        if(!validate()) return {success: false};
        setLoading(true);

        try{
            const response = await authApi.sendCode(email);
            if(!response.success){
                return {
                    success: false,
                    error: response.error
                };
            }

            const data = response.data;
            const seconds = convertToTimeSeconds(data.time);

            return {
                success: true,
                step: data.step,
                resendTime: Date.now() + seconds * 1000,
                tempUser: {email}
            };
        }
        catch (e){
            return{
                success : false,
                error: e.message
            };
        }

        finally{
            setLoading(false);
        }
    }

    return {
        loading,
        email, setEmail, emailError, setEmailError,
        validate, handleSubmit
    }
}

export default useEmailSignIn;