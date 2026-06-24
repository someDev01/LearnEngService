import styles from '../button_navigate/button_navigate.module.css';

function ButtonNavigate({onClick, title, children}){
    return(
        <div className={styles.button} onClick={onClick}>
            {children}
            <div className={styles.title}>{title}</div>
        </div>
    )
}

export default ButtonNavigate;