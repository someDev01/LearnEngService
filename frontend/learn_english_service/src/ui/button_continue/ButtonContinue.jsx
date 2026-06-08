import styles from '../button_continue/button_continue.module.css';

function ButtonContinue({onClick, children, disabled}){
    return(
        <div className={styles.button_block}>
            <button className={styles.btn_continue} onClick={onClick} disabled={disabled}>
                {children}
            </button>
        </div>
    )
}

export default ButtonContinue;