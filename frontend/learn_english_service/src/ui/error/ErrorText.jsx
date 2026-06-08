import styles from '../error/error_text.module.css';

function ErrorText({children}){
    return(
        <p className={styles.error_text}>{children}</p>
    )
}

export default ErrorText;