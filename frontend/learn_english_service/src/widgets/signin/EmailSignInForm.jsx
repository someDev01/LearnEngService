import { useDispatch } from 'react-redux';
import ButtonContinue from '../../ui/button_continue/ButtonContinue';
import InputField from '../../ui/input/InputField';
import styles from '../signin/email_signin_form.module.css';
import useEmailSignIn from '../../hooks/email/useEmailSignIn';
import { resetTempUser, setError, setResendTime, setStep, setTempUser } from '../../redux/slices/authSlice';
import { toast } from 'react-toastify';
import ContinueButtonLoader from '../../ui/button_loader/ContinueButtonLoader';
import { useState } from 'react';

function EmailSignInForm(){

    const dispatch = useDispatch();
    const { loading, email, setEmail, emailError, setEmailError, handleSubmit } = useEmailSignIn();

    const [highLightErrorBorder, setHighLightErrorBorder] = useState(false);

    const onShowInvalidInput = () => {setHighLightErrorBorder(true)};
    const onClearInvalidInput = () => {setHighLightErrorBorder(false)};

    const onSubmit = async () => {
        const result = await handleSubmit();
        
        if(result.success){
            onClearInvalidInput();
            dispatch(setTempUser(result.tempUser));
            dispatch(setStep(result.step));
            dispatch(setResendTime(result.resendTime));
        } 
        else{
            onShowInvalidInput();
            dispatch(resetTempUser());
            toast.error(result.error);
            
            dispatch(setError(result.error));
        }
    }

    return(
        <>
            <div className={styles.logo}>VOCLIP</div>
            <div className={styles.title}>Введите почту</div>
            <div className={styles.description}>чтобы войти или создать аккаунт</div>

            <div className={styles.inputs}>
                <InputField 
                    value={email}
                    setValue={setEmail}
                    error={emailError}
                    onErrorClear={() => setEmailError('')}
                    onClearInvalidInput={onClearInvalidInput}
                    highLightErrorBorder={highLightErrorBorder}
                />
            </div>
            <ButtonContinue onClick={onSubmit} disabled={loading}>
                {loading ? <ContinueButtonLoader/>: <p>Продолжить</p>}
            </ButtonContinue>
        </>
    )
}

export default EmailSignInForm;