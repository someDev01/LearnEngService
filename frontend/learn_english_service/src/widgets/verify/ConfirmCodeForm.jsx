import { useDispatch, useSelector } from 'react-redux';
import { useCodeInput } from '../../hooks/code_input/useCodeInput';
import styles from '../verify/confirm_code_form.module.css';
import { LEN_CODE_VERIFY } from './../../const/code/lengthCodeVerify';
import { useCodeVerify } from '../../hooks/code_verify/useCodeVerify';
import { resetResendTime, resetTempUser, setError, setResendTime, setStep, setUser } from '../../redux/slices/authSlice';
import { closeModalAuth } from '../../redux/slices/modalSlice';
import { toast } from 'react-toastify';
import CodeInput from '../../ui/code/CodeInput';
import { useEffect, useState } from 'react';
import ButtonAgain from '../../ui/button_again/ButtonAgain';

function ConfirmCodeForm(){

    const dispatch = useDispatch();
    const tempUser = useSelector(state => state.auth.tempUser);
    

    const {
        values,
        inputRefs,
        handleChange,
        onKeyDown,
        code,
        isComplete
    } = useCodeInput(LEN_CODE_VERIFY);

    const {
        leftResendTime,
        verifyCode,
        sendCodeAgain,
        isResendDisabled
    } = useCodeVerify({ tempUser });

    const [isVerifying, setIsVerifying] = useState(false);

    useEffect(() => {
        if (isComplete && tempUser && !isVerifying) {
            handleVerify();
        }
    }, [isComplete, tempUser]);

    const handleVerify = async () => {
        if (!isComplete || !tempUser || isVerifying) return;

        setIsVerifying(true);

        const result = await verifyCode(code);

        setIsVerifying(false);

        if (result.success) {
            dispatch(resetResendTime());
            dispatch(setUser({
                name: result.data.name,
                email: result.data.email,
            }));
            
            dispatch(resetTempUser());
            dispatch(closeModalAuth());
            dispatch(setStep(result.data.step));

            toast.success("Вы успешно вошли");
        } else {
            dispatch(setError(result.error));            
            toast.error(result.error);
        }
    };

    const handleResendCode = async () => {
        const result = await sendCodeAgain();

        if (result.success) {
            dispatch(setResendTime(result.data.resendTime));
            toast.success('Код отправлен повторно');
        } else {
            dispatch(setError(result.error));
            toast.error(result.error);
        }
    };

    return(
        <>
            <div className={styles.text_confirm}>
                <p className={styles.title}>Подтвердите ваш email</p>
                <p className={styles.details}>
                    Мы отправили 5-значный код на <span className={styles.email_text}>{tempUser?.email}</span>
                </p>
                <p className={styles.check_spam}>Не пришел код? Проверьте папку «Спам»</p>

            </div>
            <CodeInput
                values={values}
                refs={inputRefs}
                onChange={handleChange}
                onKeyDown={onKeyDown}
            />

            <ButtonAgain
                isResendDisabled={isResendDisabled}
                handleResendCode={handleResendCode}
                leftResendTime={leftResendTime}
            />
        </>
     
    )
}

export default ConfirmCodeForm;