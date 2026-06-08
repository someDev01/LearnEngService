import styles from '../button_again/button_again.module.css';

function ButtonAgain({isResendDisabled, handleResendCode, leftResendTime}){
    return(
        <button 
            disabled={isResendDisabled}
            onClick={handleResendCode}
            className={styles.button_again}
        >
            {isResendDisabled ? `Повторить через ${leftResendTime}с` : 'Получить код еще раз'}
        </button>
    )
}

export default ButtonAgain;