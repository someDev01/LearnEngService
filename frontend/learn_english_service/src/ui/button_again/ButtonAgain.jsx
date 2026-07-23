import { Clock, Send } from 'lucide-react';
import styles from '../button_again/button_again.module.css';

function ButtonAgain({isResendDisabled, handleResendCode, leftResendTime}){
    return(
        <button 
            onClick={isResendDisabled ? () => {} : handleResendCode}
            className={`${styles.button_again} ${isResendDisabled ? styles.disabled : ''}`}
        > 
            {isResendDisabled ? (
                <>
                    <Clock size={18}/>
                    <span>Новый код через</span> {leftResendTime}
                </> 
            ) : (
                <>
                    <Send size={18}/>
                    Отправить код повторно
                </>
            )}           
        </button>
    )
}

export default ButtonAgain;