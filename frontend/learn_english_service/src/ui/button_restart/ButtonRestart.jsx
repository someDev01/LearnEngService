import styles from '../button_restart/button_restart.module.css';

function ButtonRestart({onClick}){
    return(
        <div className={styles.restart} onClick={onClick}>
            Начать заново
        </div>
    )
}

export default ButtonRestart;