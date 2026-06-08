import styles from '../button_signin/button_signin.module.css';

function ButtonSignIn({onClick}){
    return(
        <div className={styles.button_signin} onClick={onClick}>
            <p>Войти</p>
        </div>
    )
}

export default ButtonSignIn;