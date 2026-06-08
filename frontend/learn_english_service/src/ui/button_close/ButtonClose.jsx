import styles from '../button_close/button_close.module.css';

function ButtonClose({onClick}){
    return(
        <div className={styles.button} onClick={onClick}>
            <p className={styles.icon_close}>X</p>
        </div>
    )
}

export default ButtonClose;